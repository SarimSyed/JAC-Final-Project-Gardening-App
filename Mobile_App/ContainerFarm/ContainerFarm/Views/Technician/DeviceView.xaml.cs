using Azure;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Producer;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ContainerFarm.Repos;
using static System.Reflection.Metadata.BlobBuilder;

namespace ContainerFarm.Views.Technician;

/// Connected Tractors (Team #5)
/// Winter 2023 - April 28th
/// AppDev III
/// <summary>
/// This class initializes and manages the plant devices for a container. 
/// The user can view the readings of the sensors (temperature, humidity, water level and soil level, and they can
/// manage the actuators (light and fan)
/// </summary>
public partial class DeviceView : ContentPage
{
    PlantsRepo repo = new PlantsRepo();

    public DeviceView()
    {
        InitializeComponent();

        //Using the index 0 of the repo since technicians will only have access to one container
        BindingContext = App.Repo.Containers[0].Plant;

        //Thread eventDataThread = new Thread(Do);
        //eventDataThread.Start();
    }

    private static async void Do()
    {
        string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

        await using (var producer = new EventHubProducerClient(App.Settings.EventHubConnectionString, App.Settings.EventHubName))
        {
            string[] partitionIds = await producer.GetPartitionIdsAsync();
            Console.WriteLine(partitionIds);

            foreach (string partitionId in partitionIds)
            {
                var partitionProperties = await producer.GetPartitionPropertiesAsync(partitionId);
                Console.WriteLine(partitionProperties);
            }
        }

        await using (var consumer = new EventHubConsumerClient(consumerGroup, App.Settings.EventHubConnectionString, App.Settings.EventHubName))
        {
            using var cancellationSource = new CancellationTokenSource();
            cancellationSource.CancelAfter(TimeSpan.FromSeconds(45));

            await foreach (PartitionEvent receivedEvent in consumer.ReadEventsAsync(cancellationSource.Token))
            {
                // At this point, the loop will wait for events to be available in the Event Hub.  When an event
                // is available, the loop will iterate with the event that was received.  Because we did not
                // specify a maximum wait time, the loop will wait forever unless cancellation is requested using
                // the cancellation token.
                Console.WriteLine(receivedEvent);
                Console.WriteLine(receivedEvent.Partition);
                Console.WriteLine(receivedEvent.Data.EventBody);
            }
        }
    }

    private void FanSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        App.Repo.Containers[0].Plant.FanActuator.IsChanged = true;

        //Shows the confimation to the user that the fan has been turned on or off
        Switch fanSwitch = sender as Switch;
        SetSwitchTextStatus(fanSwitch, fanStatus);
    }

    private void LightSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        //Shows the confimation to the user that the fan has been turned on or off
        Switch lightSwitch = sender as Switch;
        SetSwitchTextStatus(lightSwitch, lightStatus);
    }

    private void SetSwitchTextStatus(Switch actuatorSwitch, Label actuatorText)
    {
        if (actuatorSwitch.IsToggled)
        {
            actuatorText.Text = "ON";
            actuatorText.TextColor = Colors.DarkGreen;
        }
        else
        {
            actuatorText.Text = "OFF";
            actuatorText.TextColor = Colors.Red;
        }
    }
}