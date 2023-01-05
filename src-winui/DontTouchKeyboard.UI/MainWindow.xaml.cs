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

        ThemeHelpers.TrySetSystemBackdrop(this, GetRootElement(), Backdrop.Auto);

        ThemeHelpers.TrySetCustomTitleBar(this, GetRootElement(), GetCustomTitleBar());

        ThemeHelpers.TrySetWindowTheme(this, GetRootElement());
    }

    public bool TrySetWindowTheme(ElementTheme theme) => ThemeHelpers.TrySetWindowTheme(this, GetRootElement(), theme);


    private void Grid_PreviewKeyUp(object sender, KeyRoutedEventArgs e)
    {
        App.OnStatusChanged(sender, e.Key);
    }

    private void Grid_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
    {
        App.OnStatusChanged(sender, e.Key);
    }
}
