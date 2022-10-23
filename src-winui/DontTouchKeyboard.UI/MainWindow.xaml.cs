

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DontTouchKeyboard.UI;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{

    private AppWindow appWindow;
    public MainWindow()
    {
        this.InitializeComponent();
        var hWnd = WindowNative.GetWindowHandle(this);
        var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
        appWindow = AppWindow.GetFromWindowId(windowId);
        //appWindow.SetPresenter(AppWindowPresenterKind.Overlapped);
        appWindow.Title = "别敲键盘";
        appWindow.SetIcon("Logo.ico");

        //var topWindow = AppWindow.Create(CompactOverlayPresenter.Create());
        //topWindow.Show();

        //ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);

        //Task.Run(() =>
        //{
        //    CimSession cimSession = CimSession.Create("localhost");
        //    IEnumerable<CimInstance> enumeratedInstances = cimSession.EnumerateInstances(@"root\cimv2", "Win32_Keyboard");
        //    foreach (CimInstance instance in enumeratedInstances)
        //    {
        //        var p = instance.CimInstanceProperties;
        //    }
        //});
    }

    private void Grid_PreviewKeyUp(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key is VirtualKey.CapitalLock
             or VirtualKey.NumberKeyLock
             or VirtualKey.Insert
             or VirtualKey.Scroll
             or VirtualKey.Shift
             or VirtualKey.LeftShift
             or VirtualKey.RightShift
             or VirtualKey.Control
             or VirtualKey.LeftControl
             or VirtualKey.RightControl
             )
        {
            AppHost.OnStatusChanged(sender, e.Key);
        }
    }

    private void Grid_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key is VirtualKey.CapitalLock
             or VirtualKey.NumberKeyLock
             or VirtualKey.Insert
             or VirtualKey.Scroll
             or VirtualKey.Shift
             or VirtualKey.LeftShift
             or VirtualKey.RightShift
             or VirtualKey.Control
             or VirtualKey.LeftControl
             or VirtualKey.RightControl)
        {
            AppHost.OnStatusChanged(sender, e.Key);
        }
        else
        {
            throw new Exception("一个非常难以处理的错误, 并且不知道怎么发生的");
        }
    }
}
