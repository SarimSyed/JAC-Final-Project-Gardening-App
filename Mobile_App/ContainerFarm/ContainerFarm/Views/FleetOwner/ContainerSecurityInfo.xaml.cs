using ContainerFarm.Models;

namespace ContainerFarm.Views.FleetOwner;

public partial class ContainerSecurityInfo : ContentPage
{
    public ContainerSecurityInfo(Container container)
    {
        InitializeComponent();
        BindingContext = container.SecurityDetails;
    }

    private void DoorLockSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        Switch doorLockSwitch = sender as Switch;
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
        Switch buzzerSwitch = sender as Switch;
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