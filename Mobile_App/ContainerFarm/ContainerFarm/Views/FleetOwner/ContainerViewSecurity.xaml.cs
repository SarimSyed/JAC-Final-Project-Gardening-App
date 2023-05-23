using ContainerFarm.Models;
using ContainerFarm.Repos;

namespace ContainerFarm.Views.FleetOwner;

/// <summary>
/// This class inizializes and manages the content page for container views on the security tab.
/// It shows a list of all the containers and allows the user to view the security details of a container.
/// </summary>
public partial class ContainerViewSecurity : ContentPage
{
    public ContainerViewSecurity()
	{
		InitializeComponent();

		securityCollection.ItemsSource = App.Repo.Containers;
    }

    private void Container_Farm_Security_Tapped(object sender, TappedEventArgs e)
    {
        Frame frame = sender as Frame;
        Label label = frame.FindByName("nameBtn") as Label;

        //Check which container is clicked
        Container containerClicked = App.Repo.Containers.Where(c => c.Name == label.Text).FirstOrDefault();

        //Navigates to security info datails of that container
        Navigation.PushAsync(new ContainerSecurityInfo(containerClicked));
    }

    private async void Issues_Warning_Btn_Clicked(object sender, EventArgs e)
    {
        // Only show alert if issues
        if (App.Repo.Containers[0].Security.IssuesCount > 0)
            // Display issues alert
            await DisplayAlert("Security Issues", App.Repo.Containers[0].Security.IssuesMessage(), "OK");
    }

    private async void Add_Btn_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Add Container", "Adding container feature coming soon!", "OK");
    }
}