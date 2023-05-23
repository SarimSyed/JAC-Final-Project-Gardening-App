using ContainerFarm.Models;
using ContainerFarm.Repos;

namespace ContainerFarm.Views.FleetOwner;

/// <summary>
/// This class inizializes and manages the content page for container views on the geo-location tab.
/// It shows a list of all the containers and allows the user to view the location details of a container.
/// </summary>
public partial class ContainerViewLocation : ContentPage
{    public ContainerViewLocation()
    {
        InitializeComponent();
        containerCollection.ItemsSource = App.Repo.Containers;
    }

    //private async void Edit_Container(object sender, EventArgs e)
    //{
    //    //Check if user can edit container
    //    await DisplayAlert("Yay", "Edit worked", "Ok");
    //}

    //private async void SwipeItem_Invoked(object sender, EventArgs e)
    //{
    //    //Check if user can delete container
    //    await DisplayAlert("Yay", "Delette worked", "Ok");

    //}

    private void Container_Farm_GeoLocation_Tapped(object sender, TappedEventArgs e)
    {
        Frame frame = sender as Frame;
        Label label = frame.FindByName("nameBtn") as Label;

        //Checks which container was clicked by the user
        Container containerClicked = App.Repo.Containers.Where(c => c.Name == label.Text).FirstOrDefault();

        //Navigates to the information details for that container
        Navigation.PushAsync(new ContainerLocationInfo(containerClicked));
    }

    private async void Add_Btn_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Add Container", "Adding container feature coming soon!", "OK");
    }
}