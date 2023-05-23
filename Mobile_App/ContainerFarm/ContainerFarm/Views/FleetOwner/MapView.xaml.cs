namespace ContainerFarm.Views.FleetOwner;

using ContainerFarm.Models;
using Microsoft.Maui.Controls.Maps;

using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;


public partial class MapView : ContentPage, IQueryAttributable
{
	public MapView()
	{
		InitializeComponent();

        AddContainersToMap();
        PanToContainerLocationAsync(App.Repo.Containers[0].Location.GpsSensor.Address);
    }

    /// <summary>
    /// Asks the user for permission to access their location when app loads.
    /// </summary>
    protected async override void OnAppearing()
    {
        await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
    }

    /// <summary>
    /// Adds the containers from the repo to the map.
    /// </summary>
    public async void AddContainersToMap()
    {
        foreach (Container container in App.Repo.Containers)
        {
            map_app.Pins.Add(new Pin
            {
                Address = container.Location.GpsSensor.Address,
                Location = await GetContainerMapCoordinates(container.Location.GpsSensor.Address),
                Label = container.Location.GpsSensor.Name,
                Type = PinType.Place
            });
        }
    }

    /// <summary>
    /// Gets the container's location from the specified address.
    /// </summary>
    /// <param name="containerAddress">The container's address.</param>
    /// <returns>The container's location from the specified address.</returns>
    private async Task<Location> GetContainerMapCoordinates(string containerAddress)
    {
        IEnumerable<Location> locations = await Geocoding.Default.GetLocationsAsync(containerAddress);

        Location location = locations?.FirstOrDefault();

        return location == null
            ? new Location(45.40872533174768, -74.15082292759962)
            : location;
    }

    /// <summary>
    /// Pans to the first container's location on the map.
    /// </summary>
    private void PanToContainerLocation(Location location)
    {
        if (location == null) return;

        MapSpan mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(0.44));
        map_app.MoveToRegion(mapSpan);
    }

    /// <summary>
    /// Pans to the first container's location on the map asynchronously.
    /// </summary>
    /// <param name="address"></param>
    private async void PanToContainerLocationAsync(string address)
    {
        Location location = await GetContainerMapCoordinates(address);
        PanToContainerLocation(location);
    }

    /// <summary>
    /// Gets the query attributes from the location.
    /// </summary>
    /// <param name="query"></param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query == null) return;

        string address = query["address"] as string;
        PanToContainerLocationAsync(address);
    }
}