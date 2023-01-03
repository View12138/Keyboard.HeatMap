namespace DontTouchKeyboard.UI;
/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{


    public static event EventHandler<DtkKeyEventArgs>? StatusChanged;
    public static void OnStatusChanged(object source, VirtualKey key) => StatusChanged?.Invoke(source, new DtkKeyEventArgs(key));

    private static App? current;
    public static new App Current => (App)Application.Current;

    public MainWindow MainWindow { get; private set; }

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        current = this;
        InitializeComponent();
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        Ioc.Default.ConfigureServices(ConfigureServices());

        MainWindow = new MainWindow();
        MainWindow.Activate();
    }

    private static IServiceProvider ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton(typeof(MainWindow));

        services.AddSingleton(typeof(ShellViewModel));
        services.AddSingleton(typeof(SystemInfoViewModel));

        return services.BuildServiceProvider();
    }
}
