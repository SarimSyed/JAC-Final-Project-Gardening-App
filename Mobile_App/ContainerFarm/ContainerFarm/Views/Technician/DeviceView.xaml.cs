using ContainerFarm.Repos;

namespace ContainerFarm.Views.Technician;

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
        BindingContext = repo.plant;
    }

    private void FanSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        //Shows the confimation to the user that the fan has been turned on or off
        Switch fanSwitch = sender as Switch;
        if (fanSwitch.IsToggled)
        {
            fanStatus.Text = "ON";
            fanStatus.TextColor = Colors.DarkGreen;
        }
        else
        {
            fanStatus.Text = "OFF";
            fanStatus.TextColor = Colors.Red;
        }
    }

    private void LightSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        //Shows the confimation to the user that the fan has been turned on or off
        Switch lightSwitch = sender as Switch;
        if (lightSwitch.IsToggled)
        {
            lightStatus.Text = "ON";
            lightStatus.TextColor = Colors.DarkGreen;
        }
        else
        {
            lightStatus.Text = "OFF";
            lightStatus.TextColor = Colors.Red;
        }
    }
}