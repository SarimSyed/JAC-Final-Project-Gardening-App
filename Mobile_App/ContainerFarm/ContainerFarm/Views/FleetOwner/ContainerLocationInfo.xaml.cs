using ContainerFarm.Models;

namespace ContainerFarm.Views.FleetOwner;

public partial class ContainerLocationInfo : ContentPage
{
    public ContainerLocationInfo(Container container)
    {
        InitializeComponent();
        BindingContext = container.GeoLocationDetails;
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