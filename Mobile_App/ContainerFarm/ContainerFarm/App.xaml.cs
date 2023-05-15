using ContainerFarm.Config;
using ContainerFarm.Repos;
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
}
