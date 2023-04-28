namespace ContainerFarm.Views.FleetOwner;
using Map = Microsoft.Maui.Controls.Maps.Map;


public partial class MapView : ContentPage
{
	public MapView()
	{
		InitializeComponent();

        Map map = new Map();
        Content = map;
    }
}