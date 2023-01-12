using System.IO;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.UI.Xaml.Media.Imaging;

namespace DontTouchKeyboard.UI;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    private Panel GetRootElement() => RootGrid;
    private ICustomTitleBar GetCustomTitleBar() => ShellPage;

    public MainWindow()
    {
        InitializeComponent();

        var settings = Ioc.Default.GetRequiredService<SettingViewModel>();

        //this.GetAppWindows().SetIcon("ms-appx:///Assets/SmallTile.png");

        TrySetSystemBackdrop(settings.GetBackdrop());

        this.TrySetCustomTitleBar(GetRootElement(), GetCustomTitleBar());

        TrySetWindowTheme(settings.GetTheme());
    }

    public bool TrySetWindowTheme(ElementTheme theme) => this.TrySetWindowTheme(GetRootElement(), theme);

    public bool TrySetSystemBackdrop(Backdrop backdrop) => this.TrySetSystemBackdrop(GetRootElement(), backdrop, SetBackgroundImage);

    public async void SetBackgroundImage()
    {
        var settings = Ioc.Default.GetRequiredService<SettingViewModel>();
        if (settings.BackgroundPath != null && File.Exists(settings.BackgroundPath.Path))
        {
            using var fileStream = await settings.BackgroundPath?.OpenReadAsync();
            BitmapImage bitmapImage = new();
            await bitmapImage.SetSourceAsync(fileStream);
            GaussianBlurEffect blurEffect = new GaussianBlurEffect();
            var brush = new ImageBrush()
            {
                ImageSource = bitmapImage
            };
            RootGrid.Background = brush;
        }
    }

    private void Grid_PreviewKeyUp(object sender, KeyRoutedEventArgs e)
    {
        App.OnStatusChanged(sender, e.Key);
    }

    private void Grid_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
    {
        App.OnStatusChanged(sender, e.Key);
    }
}
