using Android.Hardware;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using ContainerFarm.Models.Actuators;
using ContainerFarm.Repos;
using ContainerFarm.Repos;
using ContainerFarm.Views.FleetOwner;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Diagnostics;


namespace ContainerFarm.Services
{
    public static class D2CService
    {
        public static EventProcessorClient Processor { get; set; }

        public static async Task Initialize()
        {
            // Get the IoT Hub information
            var storageConnectionString = App.Settings.StorageConnectionString;
            var blobContainerName = App.Settings.BlobContainerName;
            var eventHubsConnectionString = App.Settings.EventHubConnectionString;
            var eventHubName = App.Settings.EventHubName;
            var consumerGroup = App.Settings.ConsumerGroup;

            // Create the storage client
            var storageClient = new BlobContainerClient(
                storageConnectionString,
                blobContainerName);

            // Create the processor
            Processor = new EventProcessorClient(
                storageClient,
                consumerGroup,
                eventHubsConnectionString,
                eventHubName);

            var partitionEventCount = new ConcurrentDictionary<string, int>();

            Processor.ProcessEventAsync += processEventHandler;
            Processor.ProcessErrorAsync += processErrorHandler;

            // Processes the events
            async Task processEventHandler(ProcessEventArgs args)
            {
                try
                {
                    // Don't continue if cancelled
                    if (args.CancellationToken.IsCancellationRequested)
                        return;

                    string partition = args.Partition.PartitionId;
                    byte[] eventBody = args.Data.EventBody.ToArray();

                    string eventBodyString = System.Text.Encoding.Default.GetString(eventBody);
                    string eventBodyCleaned = eventBodyString.Replace("\n", "");

                    UpdateReadings(eventBodyCleaned);


                    int eventsSinceLastCheckpoint = partitionEventCount.AddOrUpdate(
                        key: partition,
                        addValue: 1,
                        updateValueFactory: (_, currentCount) => currentCount + 1);

                    if (eventsSinceLastCheckpoint >= 50)
                    {
                        await args.UpdateCheckpointAsync();
                        partitionEventCount[partition] = 0;
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    // It is very important that you always guard against
                    // exceptions in your handler code; the processor does
                    // not have enough understanding of your code to
                    // determine the correct action to take.  Any
                    // exceptions from your handlers go uncaught by
                    // the processor and will NOT be redirected to
                    // the error handler.
                }
            }

            // Processes the Errors
            Task processErrorHandler(ProcessErrorEventArgs args)
            {
                try
                {
                    Debug.WriteLine("Error in the EventProcessorClient");
                    Debug.WriteLine($"\tOperation: {args.Operation}");
                    Debug.WriteLine($"\tException: {args.Exception}");
                    Debug.WriteLine("");
                }
                catch
                {
                    // It is very important that you always guard against
                    // exceptions in your handler code; the processor does
                    // not have enough understanding of your code to
                    // determine the correct action to take.  Any
                    // exceptions from your handlers go uncaught by
                    // the processor and will NOT be handled in any
                    // way.
                }

                return Task.CompletedTask;
            }

        }

        private static void UpdateReadings(string readings)
        {
            JObject sensorJson = JObject.Parse(readings);
            JArray jArray = (JArray)sensorJson["sensors"];



            for (int i = 0; i < jArray.Count; i++)
            {
                JObject oneSensorObject = JObject.Parse(jArray[i].ToString());
                string door_value = "";
                string motion_value = "";
                string noise_value = "";
                string luminosity_value = "";

                if (oneSensorObject.ToString().Contains("door"))
                {
                    door_value = oneSensorObject["door"]["value"].ToString();

                  if(door_value == "open")
                        App.Repo.Containers[0].Security.DoorSensor.Value = 0;
                    else
                        App.Repo.Containers[0].Security.DoorSensor.Value = 1;  
                }
                else if (oneSensorObject.ToString().Contains("motion"))
                {
                    motion_value = oneSensorObject["motion"]["value"].ToString();
                    if (motion_value == "open")
                        App.Repo.Containers[0].Security.MotionSensor.Value = 0;
                    else
                        App.Repo.Containers[0].Security.MotionSensor.Value = 1;
                }
                else if (oneSensorObject.ToString().Contains("noise"))
                {
                    noise_value = oneSensorObject["noise"]["value"].ToString();
                    if (Convert.ToInt32(noise_value) <= 100 || Convert.ToInt32(noise_value) > 180)
                        App.Repo.Containers[0].Security.NoiseSensor.Value = 0;
                    else
                        App.Repo.Containers[0].Security.NoiseSensor.Value = 1;
                }
                else if (oneSensorObject.ToString().Contains("luminosity"))
                {
                    luminosity_value = oneSensorObject["luminosity"]["value"].ToString();
                    if (Convert.ToInt32(luminosity_value) <= 30)
                        App.Repo.Containers[0].Security.LuminositySensor.Value = 0;
                    else
                        App.Repo.Containers[0].Security.LuminositySensor.Value = 1;
                }
            }
         
        }
    }
    
}
