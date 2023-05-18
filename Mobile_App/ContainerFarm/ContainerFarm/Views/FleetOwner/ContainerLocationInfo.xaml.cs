using ContainerFarm.Models;

namespace ContainerFarm.Views.FleetOwner;

/// <summary>
/// This class inizializes a container location details view where the user can view all the information for the location and manage devices.
/// The information shown to the user contains the address, pitch, roll angle, vibration level and buzzer status.
/// The user can also manage the buzzer (on/off)
/// </summary>
public partial class ContainerLocationInfo : ContentPage
{
    public ContainerLocationInfo(Container container)
    {
        InitializeComponent();
        BindingContext = container.Location;
        Title = "Geo-Location: " + container.Name;
    }

    private void BuzzerSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        //Shows the user confirmation that the buzzer has been turned on or off.
        Switch buzzerSwitch = sender as Switch;

        if (buzzerSwitch == null || buzzerStatus == null)
            return;

        App.Repo.Containers[0].Location.BuzzerActuator.IsChanged = true;

        if (buzzerSwitch.IsToggled)
        {
            buzzerStatus.Text = "ON";
            buzzerStatus.TextColor = Colors.DarkGreen;
        }
        else
        {
            buzzerStatus.Text = "OFF";
            buzzerStatus.TextColor = Colors.Red;
        }
    }
}