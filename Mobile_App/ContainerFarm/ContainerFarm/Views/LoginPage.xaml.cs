using ContainerFarm.Enums;
using ContainerFarm.Services;
using Firebase.Auth;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ContainerFarm.Views;

/// Connected Tractors (Team #5)
/// Winter 2023 - April 28th
/// AppDev III 
/// <summary>
/// THis class is used for login in to the app with a fleet owner or farm technician access.
/// It uses an authentication api to verify if the user enters the correct username and password.
/// </summary>
public partial class LoginPage : ContentPage
{
    private const string USERNAME = "user@email.com";
    private const string PASSWORD = "password";

    private LoginOptions currentOption;

    public LoginPage()
	{
		InitializeComponent();
        Debug_Options();

        currentOption = LoginOptions.FleetOwner;
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
                //await Shell.Current.GoToAsync("//FleetOwner");

                switch (currentOption)
                {
                    case LoginOptions.FleetOwner:
                        await Shell.Current.GoToAsync("//FleetOwner");
                        break;
                    case LoginOptions.FarmTechnician:
                        await Shell.Current.GoToAsync("//Technician");
                        break;
                }
            }
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

    private void Debug_Options()
    {
        username.Text = USERNAME; 
        password.Text = PASSWORD;
    }

    /// <summary>
    /// Select the fleet owner as the one to login.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Fleet_Owner_Clicked(object sender, EventArgs e)
    {
        login_image.Source = "containers.png";

        farm_technician_btn.TextColor = Colors.Black;
        farm_technician_btn.BorderColor = Colors.Black;

        fleet_owner_btn.TextColor = Color.FromArgb("512BD4");
        fleet_owner_btn.BorderColor = Color.FromArgb("512BD4");

        currentOption = LoginOptions.FleetOwner;
    }

    /// <summary>
    /// Select the farm technician as the one to login.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Farm_Technician_Clicked(object sender, EventArgs e)
    {
        login_image.Source = "greenhouse.png";

        fleet_owner_btn.TextColor = Colors.Black;
        fleet_owner_btn.BorderColor = Colors.Black;

        farm_technician_btn.TextColor = Color.FromArgb("512BD4");
        farm_technician_btn.BorderColor = Color.FromArgb("512BD4");

        currentOption = LoginOptions.FarmTechnician;
    }
}