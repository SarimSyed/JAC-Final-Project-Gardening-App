using ContainerFarm.Models;
using ContainerFarm.Repos;

namespace ContainerFarm.Views.FleetOwner;

public partial class ContainerViewSecurity : ContentPage
{
    ContainerRepo repo;

    public ContainerViewSecurity()
	{
		InitializeComponent();
		securityCollection.ItemsSource = App.Repo.Containers;
        repo = new ContainerRepo();
    }

    private void ContainerButton_Clicked(object sender, EventArgs e)
    {
        Button button = sender as Button;
        Container containerClicked = repo.Containers.Where(c => c.Name == button.Text).FirstOrDefault();
        Navigation.PushAsync(new ContainerSecurityInfo(containerClicked));
    }
}