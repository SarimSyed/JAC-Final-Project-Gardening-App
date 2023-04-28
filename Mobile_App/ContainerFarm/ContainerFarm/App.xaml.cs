using ContainerFarm.Repos;

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
}
