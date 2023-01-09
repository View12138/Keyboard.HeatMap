using DontTouchKeyboard.UI.Views;
using Microsoft.UI.Xaml.Media.Animation;

namespace DontTouchKeyboard.UI;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    private FrameworkElement GetRootElement() => RootGrid;
    private ICustomTitleBar GetCustomTitleBar() => ShellPage;

    public MainWindow()
    {
        InitializeComponent();

        var settings = Ioc.Default.GetRequiredService<SettingViewModel>();

        this.TrySetSystemBackdrop(GetRootElement(), settings.GetBackdrop());

        this.TrySetCustomTitleBar(GetRootElement(), GetCustomTitleBar());

        this.TrySetWindowTheme(GetRootElement(), settings.GetTheme());
    }

    public bool TrySetWindowTheme(ElementTheme theme) => this.TrySetWindowTheme(GetRootElement(), theme);

    public bool TrySetSystemBackdrop(Backdrop backdrop) => this.TrySetSystemBackdrop(GetRootElement(), backdrop);

    private void Grid_PreviewKeyUp(object sender, KeyRoutedEventArgs e)
    {
        App.OnStatusChanged(sender, e.Key);
    }

    private void Grid_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
    {
        App.OnStatusChanged(sender, e.Key);
    }
}
