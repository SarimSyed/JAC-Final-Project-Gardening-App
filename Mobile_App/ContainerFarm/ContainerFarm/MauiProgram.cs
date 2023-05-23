using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using SkiaSharp.Views.Maui.Controls.Hosting;
using ContainerFarm.Views;

namespace ContainerFarm;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>()
            .UseSkiaSharp()
            .UseMauiMaps()
            .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        }).UseMauiCommunityToolkit();
        builder.Services.AddSingleton<Settings>();
        builder.Services.AddSingleton<LoginPage>();
#if DEBUG
        builder.Logging.AddDebug();
#endif

        var a = Assembly.GetExecutingAssembly();
        using var stream = a.GetManifestResourceStream("ContainerFarm.appsettings.json");

        var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();
        builder.Configuration.AddConfiguration(config);
        var app = builder.Build();
        Services = app.Services;
        return app;
    }

    //Service Property need to access the services in the app 
    public static IServiceProvider Services { get; private set; }
}