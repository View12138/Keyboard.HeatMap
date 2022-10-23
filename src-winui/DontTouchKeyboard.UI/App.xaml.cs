// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DontTouchKeyboard.UI;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App() => InitializeComponent();

    /// <summary>
    /// Invoked when the application is launched normally by the end user.  Other entry points
    /// will be used such as when the application is launched to open a specific file.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        var host = Host.CreateDefaultBuilder(Environment.GetCommandLineArgs())
                       .ConfigureLogging(loggingBuilder => loggingBuilder.AddConfiguration())
                       .ConfigureServices(services => ConfigureServices(services))
                       .Build();
        Ioc.Default.ConfigureServices(host.Services);
        await host.RunAsync();
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient(typeof(MainWindow));

        services.AddSingleton(typeof(ShellViewModel));
        services.AddSingleton(typeof(SystemInfoViewModel));

        // AddHostedService
        services.AddHostedService<AppHost>();
    }
}
