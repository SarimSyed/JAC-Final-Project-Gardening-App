using ContainerFarm.Config;
using ContainerFarm.Repos;
using ContainerFarm.Services;
using Microsoft.Extensions.Configuration;

namespace ContainerFarm;


public partial class App : Application
{

	private static ContainerRepo _containerRepo;

	public App()
	{
		InitializeComponent();
		_containerRepo = new ContainerRepo();
		MainPage = new AppShell();
		
	}

    public int MyProperty { get; set; }

	internal static ContainerRepo Repo
	{
		get { return _containerRepo; }
	}

    public static Settings Settings { get; private set; }
	= MauiProgram.Services.GetService<IConfiguration>()
	.GetRequiredSection(nameof(Settings)).Get<Settings>();

    protected override void OnStart()
    {
        Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
    }

    protected async override void OnSleep()
    {
        Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;

        if (D2CService.Processor != null)
            await D2CService.Processor.StopProcessingAsync();
    }

    protected override void OnResume()
    {
        Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
    }

    void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        Page page;

        if (MainPage is NavigationPage)
        {
            page = ((NavigationPage)MainPage).CurrentPage;
        }
        else
        {
            page = MainPage;
        }

        page.DisplayAlert("No Internet", $"No internet connection. Please connect to the internet.", "OK");
    }
}
