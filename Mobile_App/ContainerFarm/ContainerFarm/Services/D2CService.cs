using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using ContainerFarm.Enums;
using ContainerFarm.Models.Actuators;
using ContainerFarm.Repos;
using ContainerFarm.Views.FleetOwner;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace ContainerFarm.Services
{
    public static class D2CService
    {
        public static EventProcessorClient Processor { get; set; }

        /// <summary>
        /// Initializes the Device to Cloud communication readings.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AggregateException"></exception>
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
                // Check internet connection
                NetworkAccess networkAccess = Connectivity.Current.NetworkAccess;

                // Throw exception if no internet access
                if (networkAccess != NetworkAccess.Internet)
                    throw new AggregateException($"No internet connection. Please connect to the internet.");

                try
                {
                    // Don't continue if cancelled
                    if (args.CancellationToken.IsCancellationRequested)
                        return;

                    string partition = args.Partition.PartitionId;
                    byte[] eventBody = args.Data.EventBody.ToArray();

                    string eventBodyString = System.Text.Encoding.Default.GetString(eventBody);
                    string eventBodyCleaned = eventBodyString.Replace("\n", "");

                    // Update Readings
                    App.Repo.UpdateReadings(eventBodyCleaned, args.Data);

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
    }
    
}
