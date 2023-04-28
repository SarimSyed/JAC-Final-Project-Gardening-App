namespace ContainerFarm.Views.FleetOwner;

public partial class ContainerViewLocation : ContentPage
{
	public ContainerViewLocation()
	{
		InitializeComponent();
		containerCollection.ItemsSource = App.Repo.Containers;
	}

    private async void Edit_Container(object sender, EventArgs e)
    {
		await DisplayAlert("Yay", "Edit worked", "Ok");
    }

    private async void SwipeItem_Invoked(object sender, EventArgs e)
    {
        await DisplayAlert("Yay", "Delette worked", "Ok");

    }
}