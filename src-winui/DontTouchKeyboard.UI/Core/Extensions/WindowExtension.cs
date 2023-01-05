namespace DontTouchKeyboard.UI.Core.Extensions;

internal static class WindowExtension
{
    public static AppWindow GetAppWindows(this Window window)
    {
        var hWnd = WindowNative.GetWindowHandle(window);
        var wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        return AppWindow.GetFromWindowId(wndId);
    }
}