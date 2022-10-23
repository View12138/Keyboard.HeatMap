
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DontTouchKeyboard.UI;

internal class AppHost : ObservableObject, IHostedService
{
    private readonly IServiceProvider provider;

    public AppHost(IServiceProvider provider)
    {
        this.provider = provider;
    }

    public static event EventHandler<DtkKeyEventArgs> StatusChanged;
    public static void OnStatusChanged(object source, VirtualKey key) => StatusChanged?.Invoke(source, new DtkKeyEventArgs(key));

    public Task StartAsync(CancellationToken cancellationToken)
    {
        provider.GetService<MainWindow>().Activate();
        Application.Current.UnhandledException += (s, e)
            => provider.GetService<SystemInfoViewModel>().SystemInfos.Add(new SystemInfoModel(e));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
