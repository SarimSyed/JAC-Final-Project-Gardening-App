using ContainerFarm.Repos;

namespace ContainerFarm.Views.Technician;

public partial class DeviceView : ContentPage
{
    PlantsRepo plantsRepo;
    public DeviceView()
    {
        InitializeComponent();

        plantsRepo = new PlantsRepo();
        BindingContext = plantsRepo.plant;
    }

    private void FanSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
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