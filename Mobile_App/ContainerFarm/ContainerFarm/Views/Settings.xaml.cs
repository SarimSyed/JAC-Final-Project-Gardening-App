namespace ContainerFarm.Views;

public partial class Settings : ContentPage
{
	public Settings()
	{
		InitializeComponent();

		this.BindingContext = App.Settings;
	}
}