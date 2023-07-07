namespace Keyboard.HeatMap;

/// <summary>
/// MigrationWindows.xaml 的交互逻辑
/// </summary>
public partial class MigrationWindows : Window
{
    public MigrationWindows()
    {
        InitializeComponent();
        Loaded += MigrationWindows_Loaded;
    }

    private async void MigrationWindows_Loaded(object sender, RoutedEventArgs e)
    {
        await DbHandle.ChangeDb();
        DialogResult = true;
    }
}
