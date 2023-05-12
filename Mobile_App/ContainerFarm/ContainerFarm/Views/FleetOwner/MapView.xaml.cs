namespace ContainerFarm.Views.FleetOwner;

using Microsoft.Maui.Controls.Maps;

//using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;


public partial class MapView : ContentPage
{
	public MapView()
	{
		InitializeComponent();

        Location location = new Location(45.40872533174768, -74.15082292759962);
        MapSpan mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(0.44));
        map_app = new Map(mapSpan);
        map_app.MoveToRegion(mapSpan);

        map_app.Pins.Add(new Pin()
        {
            Label = "Cegep John Abbott",
            Address = "Maple street",
            Type = PinType.Place,
            Location = new Location(45.40872533174768, -74.15082292759962)
        });

        this.BindingContext = map_app;
    }

    /// <summary>
    /// Asks the user for permission to access their location when app loads.
    /// </summary>
    protected async override void OnAppearing()
    {
        await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
    }
}