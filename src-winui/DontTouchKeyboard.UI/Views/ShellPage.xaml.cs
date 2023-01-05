using Microsoft.UI.Xaml.Media.Animation;
using Windows.Graphics;

namespace DontTouchKeyboard.UI.Views;

public sealed partial class ShellPage : UserControl, ICustomTitleBar
{
    public ShellPage()
    {
        InitializeComponent();
    }

    public FrameworkElement GetAppTitleBar() => AppTitleBar;
    public List<RectInt32> GetDragRects(Window window, double scaleAdjustment)
    {
        var hWnd = WindowNative.GetWindowHandle(window);
        var wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        var appWindow = AppWindow.GetFromWindowId(wndId);

        RightPaddingColumn.Width = new GridLength(appWindow.TitleBar.RightInset / scaleAdjustment);
        LeftPaddingColumn.Width = new GridLength(appWindow.TitleBar.LeftInset / scaleAdjustment);

        List<RectInt32> dragRectsList = new();

        RectInt32 dragRectL;
        dragRectL.X = (int)(LeftPaddingColumn.ActualWidth * scaleAdjustment);
        dragRectL.Y = 0;
        dragRectL.Height = (int)(AppTitleBar.ActualHeight * scaleAdjustment);
        dragRectL.Width = (int)((IconColumn.ActualWidth
                                + TitleColumn.ActualWidth
                                + LeftDragColumn.ActualWidth) * scaleAdjustment);
        dragRectsList.Add(dragRectL);

        RectInt32 dragRectR;
        dragRectR.X = (int)((LeftPaddingColumn.ActualWidth
                            + IconColumn.ActualWidth
                            + TitleTextBlock.ActualWidth
                            + LeftDragColumn.ActualWidth
                            + SearchColumn.ActualWidth) * scaleAdjustment);
        dragRectR.Y = 0;
        dragRectR.Height = (int)(AppTitleBar.ActualHeight * scaleAdjustment);
        dragRectR.Width = (int)(RightDragColumn.ActualWidth * scaleAdjustment);
        dragRectsList.Add(dragRectR);
        return dragRectsList;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        ContentFrame.Navigate(typeof(SettingPage), null, new DrillInNavigationTransitionInfo());
    }
}
