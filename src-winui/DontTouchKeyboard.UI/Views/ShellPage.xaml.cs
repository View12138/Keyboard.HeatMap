// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DontTouchKeyboard.UI.Views;

public sealed partial class ShellPage : UserControl
{
    public ShellPage()
    {
        this.InitializeComponent();
    }

    private void TextBlock_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
    {

    }
}
