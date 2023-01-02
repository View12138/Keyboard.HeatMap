// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Composition;
using PIncoke;
using Windows.UI;
using WinRT;
using Microsoft.UI.Xaml;
using Windows.Graphics;

namespace DontTouchKeyboard.UI.Views;

public sealed partial class ShellPage : UserControl, ICustomTitleBar
{
    public ShellPage()
    {
        InitializeComponent();
    }

    private void ComboBox_Theme_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ThemeHelpers.OnThemeChange(App.Current.MainWindow, ThemeSelector.SelectedIndex switch
        {
            0 => ElementTheme.Dark,
            1 => ElementTheme.Light,
            _ => ElementTheme.Default,
        });
    }

    public FrameworkElement GetAppTitleBar()
    {
        return AppTitleBar;
    }
    public List<RectInt32> GetDragRects(Window window)
    {
        var hWnd = WindowNative.GetWindowHandle(window);
        var wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        var appWindow = AppWindow.GetFromWindowId(wndId);

        var scaleAdjustment = ThemeHelpers.GetScaleAdjustment(window);

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

}
