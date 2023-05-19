using ContainerFarm.Services;

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
            await D2CService.Processor.StopProcessingAsync();
         
            // Validates that there's a signed in user
            if (AuthService.Client.User == null)
                return;

            // Sign out
            AuthService.Client.SignOut();

            // Correctly logged in
            await Shell.Current.GoToAsync("//Login");
        }
        catch (Exception ex)
        {
            // Display alert message
            await DisplayAlert("Invalid information", $"{ex.Message}", "OK");
        }
    }
}
