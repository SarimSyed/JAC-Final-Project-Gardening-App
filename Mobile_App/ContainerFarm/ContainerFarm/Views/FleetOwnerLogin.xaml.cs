using ContainerFarm.Services;
using Firebase.Auth;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

            //Check internet connection
            NetworkAccess networkAccess = Connectivity.Current.NetworkAccess;

            if (networkAccess != NetworkAccess.Internet)
            {
                throw new Exception("No Internet connection.");
            }
            var client = AuthService.Client;
            var result = await client.FetchSignInMethodsForEmailAsync(username.Text);
            if (result.UserExists && result.AllProviders.Contains(FirebaseProviderType.EmailAndPassword))
            {
                AuthService.UserCreds = await client.SignInWithEmailAndPasswordAsync(username.Text, password.Text);
                await Shell.Current.GoToAsync("//FleetOwner");

            }
            else
            {
                errorLbl.Text = "Email and/or password incorrect";
            }



            //login using hardcoded value
            //if (password.Text == PASSWORD && username.Text == USERNAME)
            //{
            //    await Shell.Current.GoToAsync("//Technician");

            //}
            //else
            //{
            //    await DisplayAlert("Error", "Username or Password incorrect", "Ok");
            //}

        }
        catch(FirebaseAuthException ex)
        {
            await DisplayAlert("Error", $"{ex.Reason}", "Ok");


        }
        catch (Exception ex)
        {

            await DisplayAlert("Exception Thrown", $"{ex.Message}", "OK");
        }


    }

}