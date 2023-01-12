namespace DontTouchKeyboard.UI;
/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{


    public static event EventHandler<DtkKeyEventArgs>? StatusChanged;
    public static void OnStatusChanged(object source, VirtualKey key) => StatusChanged?.Invoke(source, new DtkKeyEventArgs(key));
    public static new App Current => (App)Application.Current;

#if DEBUG
    public static string ProductId => "9WZDNCRFHVJL"; // OneNote
#else
    public static string ProductId => "9P7G9DHQ9CPP";
#endif

    private static MainWindow? mainWindow;

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        Ioc.Default.ConfigureServices(ConfigureServices());
        mainWindow = new MainWindow();
        mainWindow?.Activate();
    }

    public static bool TrySetWindowTheme(ElementTheme theme) => mainWindow?.TrySetWindowTheme(theme) ?? false;
    public static bool TrySetSystemBackdrop(Backdrop backdrop) => mainWindow?.TrySetSystemBackdrop(backdrop) ?? false;

    public static async Task<StorageFile?> PickSingleFileDialog()
    {
        if (mainWindow != null)
        {
            var openPicker = new FileOpenPicker(); // FolderPicker
            var hwnd = WindowNative.GetWindowHandle(mainWindow);
            InitializeWithWindow.Initialize(openPicker, hwnd);
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            return await openPicker.PickSingleFileAsync();
        }
        return null;
    }


    public static T? GetResource<T>(string name) where T : class
    {
        if (Current.Resources.TryGetValue(name, out object resource))
        {
            return (T?)resource;
        }
        return null;
    }

    public static T GetRequiredResource<T>(string name)
    {
        if (!Current.Resources.TryGetValue(name, out object resource))
        {
            ThrowHelper.ThrowInvalidOperationException($"资源 '{name}' 不存在");
        }
        if (resource is not T)
        {
            ThrowHelper.ThrowInvalidOperationException($"资源 '{name}' 不是 '{typeof(T).Name}' 类型");
        }
        return (T)resource;
    }

    private static IServiceProvider ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton(typeof(SystemInfoViewModel));
        services.AddSingleton(typeof(SettingViewModel));

        return services.BuildServiceProvider();
    }
}
