namespace ContainerFarm.Views;

public partial class TechnicianLogin : ContentPage
{
	//Only using this to test the login page, login functionality will shortly be implemented
	private const string USERNAME = "userT";
	private const string PASSWORD = "passT";
	public TechnicianLogin()
	{
		InitializeComponent();
	}

    private async void SignInBtn_Clicked(object sender, EventArgs e)
    {

        
		try
		{
            if (passwordEntry.Text == PASSWORD && usernameEntry.Text == USERNAME)
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