// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DontTouchKeyboard.UI.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SettingPage : Page
{
    public SettingPage()
    {
        InitializeComponent();
    }

    private void OpenColorsSettings_Click(object sender, RoutedEventArgs e)
    {
        StartProcessHelper.Start(OtherApp.Settings_Colors);
    }

    private void OpenBackgroundsSettings_Click(object sender, RoutedEventArgs e)
    {
        StartProcessHelper.Start(OtherApp.Settings_Backgrounds);
    }
}
