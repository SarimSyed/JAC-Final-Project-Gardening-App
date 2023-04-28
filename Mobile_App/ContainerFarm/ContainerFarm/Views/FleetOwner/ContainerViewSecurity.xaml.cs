namespace ContainerFarm.Views.FleetOwner;

public partial class ContainerViewSecurity : ContentPage
{
	public ContainerViewSecurity()
	{
		InitializeComponent();
		securityCollection.ItemsSource = App.Repo.Containers;
	}
}