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

    private void ContainerButton_Clicked(object sender, EventArgs e)
    {
        Button button = sender as Button;

        //Check which container is clicked
        Container containerClicked = App.Repo.Containers.Where(c => c.Name == button.Text).FirstOrDefault();

        //Navigates to security info datails of that container
        Navigation.PushAsync(new ContainerSecurityInfo(containerClicked));
    }
}