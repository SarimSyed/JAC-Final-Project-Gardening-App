using ContainerFarm.Enums;
using ContainerFarm.Helpers;
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
            // Check internet connection
            NetworkAccess networkAccess = Connectivity.Current.NetworkAccess;

            // Throw exception if no internet access
            if (networkAccess != NetworkAccess.Internet)
                throw new AggregateException($"No internet connection. Please connect to the internet.");

            var client = AuthService.Client;
            var result = await client.FetchSignInMethodsForEmailAsync(username.Text);

            // Validate email with Firebase
            if (!result.UserExists || !result.AllProviders.Contains(FirebaseProviderType.EmailAndPassword))
                throw new Exception("No email exists for that username.");

            // Validate the password
            AuthService.UserCreds = await client.SignInWithEmailAndPasswordAsync(username.Text, password.Text);

            switch (currentOption)
            {
                // Login as Fleet Owner
                case LoginOptions.FleetOwner:
                    await Shell.Current.GoToAsync("//FleetOwner");
                    break;
                // Login as Farm Technician
                case LoginOptions.FarmTechnician:
                    await Shell.Current.GoToAsync("//Technician");
                    break;
            }

            // Display successful login
            ShowSnackbar.NewSnackbar($"Logged in successfully!");
        }
        catch (AggregateException ex)
        {
            // Display alert message
            await DisplayAlert("No Internet Connection", $"{ex.Message}", "OK");
        }
        catch (FirebaseAuthException ex)
        {
            // Display alert message
            await DisplayAlert("Invalid information", $"{ex.Reason}", "OK");
        }
        catch (Exception ex)
        {
            // Display alert message
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