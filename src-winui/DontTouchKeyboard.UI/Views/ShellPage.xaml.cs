namespace DontTouchKeyboard.UI.Views;

public sealed partial class ShellPage : UserControl, ICustomTitleBar
{
    public ShellPage()
    {
        InitializeComponent();
        ContentFrame.Navigate(typeof(StandardKeyboardPage));
    }

    public FrameworkElement GetAppTitleBar() => AppTitleBar;
    public List<RectInt32> GetDragRects(Window window, double scaleAdjustment)
    {
        var hWnd = WindowNative.GetWindowHandle(window);
        var wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        var appWindow = AppWindow.GetFromWindowId(wndId);

        LeftInsetColumn.Width = new GridLength(appWindow.TitleBar.LeftInset / scaleAdjustment);
        RightInsetColumn.Width = new GridLength(appWindow.TitleBar.RightInset / scaleAdjustment);

        List<RectInt32> dragRectsList = new();

        RectInt32 dragRectLeft;
        dragRectLeft.X = (int)((LeftInsetColumn.ActualWidth
                                + ActionColumn.ActualWidth) * scaleAdjustment);
        dragRectLeft.Y = 0;
        dragRectLeft.Height = (int)(AppTitleBar.ActualHeight * scaleAdjustment);
        dragRectLeft.Width = (int)((TitleColumn.ActualWidth
                                + LeftDragColumn.ActualWidth) * scaleAdjustment);
        dragRectsList.Add(dragRectLeft);

        RectInt32 dragRectRight;
        dragRectRight.X = (int)((LeftInsetColumn.ActualWidth
                            + ActionColumn.ActualWidth
                            + TitleColumn.ActualWidth
                            + LeftDragColumn.ActualWidth
                            + SearchColumn.ActualWidth) * scaleAdjustment);
        dragRectRight.Y = 0;
        dragRectRight.Height = (int)(AppTitleBar.ActualHeight * scaleAdjustment);
        dragRectRight.Width = (int)(RightDragColumn.ActualWidth * scaleAdjustment);
        dragRectsList.Add(dragRectRight);
        return dragRectsList;
    }

    private void ActionButton_Clicked(object sender, RoutedEventArgs e)
    {
        if (ContentFrame.CanGoBack)
        {
            ContentFrame.GoBack();
        }
        else if (ContentFrame.CanGoForward)
        {
            ContentFrame.GoForward();
        }
        else
        {
            ContentFrame.Navigate(typeof(OtherPage), true);
        }
    }
}
