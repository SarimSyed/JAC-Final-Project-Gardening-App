using ContainerFarm.Models;

namespace ContainerFarm.Views.FleetOwner;

/// <summary>
/// This class inizializes a container security details view where the user can view all the information for security and manage devices.
/// The information shown to the user contains the noise, luminosity and motion detection, and door, door lock and buzzer status.
/// The user can also manage the buzzer and door lock (on/off)
/// </summary>
public partial class ContainerSecurityInfo : ContentPage
{
    public ContainerSecurityInfo(Container container)
    {
        InitializeComponent();
        BindingContext = container.Security;
        Title = "Security: " + container.Name;
    }

    private void DoorLockSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        //Shows the user confirmation that the door lock has been turned on or off.
        Switch doorLockSwitch = sender as Switch;

        if (doorLockSwitch == null || doorLockStatus == null)
            return;

        App.Repo.Containers[0].Security.DoorlockActuator.IsChanged = true;

        if (doorLockSwitch.IsToggled)
        {
            doorLockStatus.Text = "ON";
            doorLockStatus.TextColor = Colors.DarkGreen;
        }
        else
        {
            doorLockStatus.Text = "OFF";
            doorLockStatus.TextColor = Colors.Red;
        }
    }

    private void BuzzerSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        //Shows the user confirmation that the buzzer has been turned on or off.
        Switch buzzerSwitch = sender as Switch;

        if (buzzerSwitch == null || buzzerStatus == null)
            return;

        App.Repo.Containers[0].Security.BuzzerActuator.IsChanged = true;

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

    /// <summary>
    /// Navigates to the map location of the container.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Show_On_Map_Btn_Clicked(object sender, EventArgs e)
    {
        // Create the address dictionnary
        var navigationParameter = new Dictionary<string, object>
        {
            { "address", App.Repo.Containers[0].Location.GpsSensor.Address.ToString() }
        };

        // Go to the map
        await Shell.Current.GoToAsync($"//FleetOwner//Map", navigationParameter);
    }
}