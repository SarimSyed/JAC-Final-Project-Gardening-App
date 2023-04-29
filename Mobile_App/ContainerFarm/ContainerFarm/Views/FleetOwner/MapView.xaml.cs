namespace ContainerFarm.Views.FleetOwner;

using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;


public partial class MapView : ContentPage
{
	public MapView()
	{
		InitializeComponent();

        Location location = new Location(45.406389035136094, -73.9417282);
        MapSpan mapSpan = new MapSpan(location, 0.01, 0.01);
        Map map = new Map(mapSpan);
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