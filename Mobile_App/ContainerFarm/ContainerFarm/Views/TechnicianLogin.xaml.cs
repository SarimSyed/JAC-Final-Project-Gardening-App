using ContainerFarm.Services;
using Firebase.Auth;

namespace ContainerFarm.Views;

public partial class TechnicianLogin : ContentPage
{
	//Only using this to test the login page, login functionality will shortly be implemented
	private const string USERNAME = "user@email.com";
	private const string PASSWORD = "password";
	public TechnicianLogin()
	{
		InitializeComponent();
        Debug_Options();
	}

    private async void SignInBtn_Clicked(object sender, EventArgs e)
    {

        
		try
		{

            //Check internet connection
            NetworkAccess networkAccess = Connectivity.Current.NetworkAccess;

            if (networkAccess != NetworkAccess.Internet)
            {
                throw new Exception("No Internet connection.");
            }
            var client = AuthService.Client;
            var result = await client.FetchSignInMethodsForEmailAsync(usernameEntry.Text);


            if (result.UserExists && result.AllProviders.Contains(FirebaseProviderType.EmailAndPassword))
            {
                AuthService.UserCreds = await client.SignInWithEmailAndPasswordAsync(usernameEntry.Text, passwordEntry.Text);
                await Shell.Current.GoToAsync("//Technician");

            }
            else
            {
                errorLbl.Text = "Email and/or password incorrect";
            }

            //if (passwordEntry.Text == PASSWORD && usernameEntry.Text == USERNAME)
            //{
            //    await Shell.Current.GoToAsync("//Technician");
            //}


        }
        catch (Exception ex)
		{

            await DisplayAlert("Exception Thrown", $"{ex.Message}", "OK");
		}


    }
    private void Debug_Options()
    {
        usernameEntry.Text = USERNAME; 
        passwordEntry.Text = PASSWORD;
    }
}