using ContainerFarm.Models;
using ContainerFarm.Repos;

namespace ContainerFarm.Views.FleetOwner;

public partial class ContainerViewLocation : ContentPage
{
    ContainerRepo repo = new ContainerRepo();
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

    private void View_Clicked(object sender, EventArgs e)
    {
        Button button = sender as Button;
        Container containerClicked = repo.Containers.Where(c => c.Name == button.Text).FirstOrDefault();
        Navigation.PushAsync(new ContainerLocationInfo(containerClicked));
    }
}