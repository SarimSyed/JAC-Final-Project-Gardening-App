using ContainerFarm.Services;
using ContainerFarm.Views;
using Microsoft.Azure.Devices.Common.Exceptions;

namespace ContainerFarm;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
    }

    /// <summary>
    /// Logs the user out when clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Logout_MenuItem_Clicked(object sender, EventArgs e)
    {
        try 
        {
            //while (D2CService.Processor.IsRunning)
            //{
            //    Console.WriteLine(D2CService.Processor.IsRunning);
            //}

            //await D2CService.Processor.StopProcessingAsync(new CancellationToken(true));
                     
            // Validates that there's a signed in user
            if (AuthService.Client.User == null)
                return;

            // Sign out
            AuthService.Client.SignOut();

            // Correctly logged in
            Shell.Current.FlyoutIsPresented = false;

            await Shell.Current.GoToAsync("//Login");

            
        }
        catch (IotHubCommunicationException ex)
        {
            // Display alert message
            await DisplayAlert("Invalid information", $"{ex.Message}", "OK");
        }
        catch (Exception ex)
        {
            // Display alert message
            await DisplayAlert("Invalid information", $"{ex.Message}", "OK");
        }
    }

    /// <summary>
    /// Navigates to the settings page when clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Settings_MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Validates that there's a signed in user
            if (AuthService.Client.User == null)
                return;

            // Push the settings page
            
            await Navigation.PushAsync(new Settings());
            Shell.Current.FlyoutIsPresented = false;
        }
        catch (Exception ex)
        {
            // Display alert message
            await DisplayAlert("Invalid information", $"{ex.Message}", "OK");
        }
    }
}
