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
        doorLockSwitch.BindingContext = App.Repo.Containers[0].Security;
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
        App.Repo.Containers[0].Plant.LightActuator.IsChanged = true;

        //Shows the confimation to the user that the fan has been turned on or off
        Switch lightSwitch = sender as Switch;
        SetSwitchTextStatus(lightSwitch, lightStatus);
    }
    
    private void DoorLockSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        App.Repo.Containers[0].Security.DoorlockActuator.IsChanged = true;

        //Shows the confimation to the user that the door lock has been turned on or off
        Switch doorLockSwitch = sender as Switch;
        SetSwitchTextStatus(doorLockSwitch, doorLockStatus);
    }

    private void SetSwitchTextStatus(Switch actuatorSwitch, Label actuatorText)
    {
        if (actuatorSwitch == null || actuatorText == null) return;

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