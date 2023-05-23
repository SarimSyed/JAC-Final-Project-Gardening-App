using ContainerFarm.Config;
using ContainerFarm.Enums;
using ContainerFarm.Helpers;
using ContainerFarm.Models;
using ContainerFarm.Services;
using Firebase.Auth;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using Microsoft.Azure.Devices.Shared;
using System.Threading;
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

    public static LoginOptions currentOption;

    public LoginPage()
	{
		InitializeComponent();
        Debug_Options();

        currentOption = LoginOptions.FleetOwner;
        Shell.Current.FlyoutIsPresented = false;
	}

    private async void SignInBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            signInBtn.IsEnabled = false;

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

            #region Service Client (IoT Hub) authentication

            ServiceClient serviceClient;

            try
            {
                // Instantiate the service client IoT Hub
                serviceClient = ServiceClient.CreateFromConnectionString($"{App.Settings.HubConnectionString}");
            }
            // Throw any errors with the IoT Hub connection string
            catch (ArgumentNullException ex)
            {
                throw new ArgumentException("IoT Hub Service Connection String cannot be null.");
            }
            // Throw any errors wrong with the IoT Hub
            catch (Exception ex)
            {
                throw new ArgumentException("Problem with IoT Hub. Can't connect to IoT Hub. Please verify all information is correct.");
            }

            #endregion

            // Invoke Direct Methods
            //await InvokeMethodAsync($"{App.Settings.DeviceId}", serviceClient, "lights-on");

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

            // Set values from preferences
            PreferencesService.SetValuesFromPreferences();

            // Display successful login
            ShowSnackbar.NewSnackbar($"Logged in successfully!");

            if(D2CService.Processor == null)
            {
                // Initialize the service
                await D2CService.Initialize();
                await D2CService.Processor.StartProcessingAsync();
            }

            // Create the twin thread
            CreateDeviceTwinThread();

            signInBtn.IsEnabled = true;
        }
        catch (AggregateException ex)
        {
            // Display alert message
            await DisplayAlert("No Internet Connection", $"{ex.Message}", "OK");
            signInBtn.IsEnabled = true;
        }
        catch (FirebaseAuthException ex)
        {
            // Display alert message
            await DisplayAlert("Invalid information", $"{ex.Reason}", "OK");
            signInBtn.IsEnabled = true;

        }
        catch (ArgumentException ex)
        {
            // Display alert message
            await DisplayAlert("IoT Hub Error", $"{ex.Message}", "OK");
            signInBtn.IsEnabled = true;

        }
        catch (Exception ex)
        {
            // Display alert message
            await DisplayAlert("Exception Thrown", $"{ex.Message}", "OK");
            signInBtn.IsEnabled = true;

        }
    }

    #region Device Twin - IoT Hub

    /// <summary>
    /// Creates a new thread to run the twin in the background off the main UI thread.
    /// </summary>
    private static void CreateDeviceTwinThread()
    {
        try
        {
            // Create the Registry Manager
            ActuatorsDeviceTwinService.RegistryManager ??= RegistryManager.CreateFromConnectionString(App.Settings.HubConnectionString);

            // Create a new thread for the twin readings
            Thread twinThread = new Thread(ProcessTwinProperties)
            {
                Name = "twinThread"
            };
            twinThread.Start();
        }
        catch (AggregateException ex)
        {
            throw;
        }
        catch (IotHubCommunicationException ex)
        {
            // Create a new thread for the twin readings
            Thread twinThread = new Thread(ProcessTwinProperties)
            {
                Name = "twinThread"
            };
            twinThread.Start();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// Handles the processing of the device twin via IoT Hub in forever loop.
    /// </summary>
    private static async void ProcessTwinProperties()
    {
        while (true)
        {
            try
            {
                // Create the twin with the specified device ID
                Twin twin = await ActuatorsDeviceTwinService.RegistryManager.GetTwinAsync(App.Settings.DeviceId);

                if (twin == null || twin.ETag == null)
                    continue;

                // Read and update values
                ActuatorsDeviceTwinService.DeviceTwinLoop(twin).Wait();
                Thread.Sleep(TelemetryIntervalModel.TelemetryInterval * 1000);
                Console.WriteLine($"Sleeping: {TelemetryIntervalModel.TelemetryInterval} seconds");
            }
            catch (AggregateException ex)
            {
                throw;
            }
            catch (IotHubCommunicationException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    #endregion

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