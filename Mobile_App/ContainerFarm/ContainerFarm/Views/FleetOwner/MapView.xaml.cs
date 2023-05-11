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

        Location location = new Location(45.406389035136094, -73.9417282);
        MapSpan mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(0.44));
        Map map = new Map(mapSpan);
        map.MoveToRegion(mapSpan);

        map.Pins.Add(new Pin()
        {
            Label = "Cegep John Abbott",
            Address = "Maple street",
            Type = PinType.Place,
            Location = new Location(45.406389035136094, -73.9417282)
        });
        map_app = map;
    }

    /// <summary>
    /// Asks the user for permission to access their location when app loads.
    /// </summary>
    protected async override void OnAppearing()
    {
        await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
    }
}