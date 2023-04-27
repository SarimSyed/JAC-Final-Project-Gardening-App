namespace ContainerFarm.Views;

public partial class FleetOwnerLogin : ContentPage
{
    private const string USERNAME = "userF";
    private const string PASSWORD = "passF";
    public FleetOwnerLogin()
	{
		InitializeComponent();
	}
    private async void SignInBtn_Clicked(object sender, EventArgs e)
    {


        try
        {
            if (password.Text == PASSWORD && username.Text == USERNAME)
            {
                await Shell.Current.GoToAsync("//Technician");
            }
            else
            {
                await DisplayAlert("Error", "Username or Password incorrect", "Ok");
            }

        }
        catch (Exception ex)
        {

            await DisplayAlert("Exception Thrown", $"{ex.Message}", "OK");
        }


    }
}