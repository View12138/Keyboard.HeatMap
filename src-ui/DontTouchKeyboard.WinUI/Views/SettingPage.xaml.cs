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

    private void Setting_Appearance_Backdrop_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox comboBox)
        {
            Setting_Appearance_Backdrop.IsExpanded = comboBox.SelectedIndex == 2;
        }
    }
}
