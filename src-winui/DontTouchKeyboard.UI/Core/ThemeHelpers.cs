using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.UI.Composition.SystemBackdrops;

namespace DontTouchKeyboard.UI.Core;

// Based on https://stackoverflow.com/a/62811758/5001796
/// <summary>
/// 主题帮助类
/// </summary>
public static class ThemeHelpers
{
    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

    private const string HKeyRoot = "HKEY_CURRENT_USER";
    private const string HkeyWindowsTheme = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes";
    private const string HkeyWindowsPersonalizeTheme = $@"{HkeyWindowsTheme}\Personalize";
    private const string HValueAppTheme = "AppsUseLightTheme";
    private const int DWMWAImmersiveDarkMode = 20;

    // based on https://stackoverflow.com/questions/51334674/how-to-detect-windows-10-light-dark-mode-in-win32-application
    private static ApplicationTheme GetAppTheme()
    {
        int theme = (int)(Registry.GetValue($"{HKeyRoot}\\{HkeyWindowsPersonalizeTheme}", HValueAppTheme, 1) ?? 1);
        return theme == 1 ? ApplicationTheme.Light : ApplicationTheme.Dark;
    }

    /// <summary>
    /// 尝试设置主题
    /// </summary>
    /// <param name="window">要设置主题的窗体</param>
    /// <param name="rootElement">窗体内的根元素</param>
    /// <param name="theme">要设置的主题</param>
    /// <returns>是否设置成功</returns>
    public static bool TrySetWindowTheme(this Window window, FrameworkElement? rootElement, ElementTheme theme = ElementTheme.Default)
    {
        var hwnd = WindowNative.GetWindowHandle(window);
        int useImmersiveDarkMode = theme switch
        {
            ElementTheme.Light => 0,
            ElementTheme.Dark => 1,
            _ => GetAppTheme() == ApplicationTheme.Light ? 0 : 1,
        };
        var result = DwmSetWindowAttribute(hwnd, DWMWAImmersiveDarkMode, ref useImmersiveDarkMode, sizeof(int)) == 0;
        if (result && rootElement != null)
        {
            rootElement.RequestedTheme = theme;
        }
        return result;
    }


    private static SystemBackdropConfiguration? configurationSource = null;
    private static MicaController? micaController = null;
    private static DesktopAcrylicController? acrylicController = null;
    /// <summary>
    /// 尝试设置背景
    /// </summary>
    /// <param name="window">要设置背景的窗体</param>
    /// <param name="rootElement">窗体内的根元素</param>
    /// <param name="backdrop">背景类型</param>
    /// <returns>是否设置成功</returns>
    public static bool TrySetSystemBackdrop(this Window window, FrameworkElement? rootElement, Backdrop backdrop = Backdrop.Mica)
    {
        micaController?.Dispose();
        micaController = null;
        acrylicController?.Dispose();
        acrylicController = null;
        window.Activated -= Window_Activated;
        window.Closed -= Window_Closed;
        if (rootElement != null)
        {
            rootElement.ActualThemeChanged -= RootElement_ActualThemeChanged;
        }
        configurationSource = null;

        if (backdrop == Backdrop.Custom)
        {
            return true;
        }
        if (MicaController.IsSupported() || DesktopAcrylicController.IsSupported())
        {
            configurationSource = new SystemBackdropConfiguration();
            var wsdqHelper = new WindowsSystemDispatcherQueueHelper();
            wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

            // Create the policy object.
            window.Activated += Window_Activated;
            window.Closed += Window_Closed;
            if (rootElement != null)
            {
                rootElement.ActualThemeChanged += RootElement_ActualThemeChanged;
            }

            // Initial configuration state.
            configurationSource.IsInputActive = true;
            if (rootElement != null)
            {
                RootElement_ActualThemeChanged(rootElement, new object());
            }

            if (backdrop == Backdrop.Mica && MicaController.IsSupported())
            {
                if (window.Content is Panel panel)
                { panel.Background = null; }

                micaController = new MicaController();

                micaController.AddSystemBackdropTarget(window.As<ICompositionSupportsSystemBackdrop>());
                micaController.SetSystemBackdropConfiguration(configurationSource);
            }
            else if (backdrop == Backdrop.Acrylic && DesktopAcrylicController.IsSupported())
            {
                if (window.Content is Panel panel)
                { panel.Background = null; }

                acrylicController = new DesktopAcrylicController();
                acrylicController.AddSystemBackdropTarget(window.As<ICompositionSupportsSystemBackdrop>());
                acrylicController.SetSystemBackdropConfiguration(configurationSource);
            }
            return true;
        }
        return false;

        static void Window_Activated(object sender, Microsoft.UI.Xaml.WindowActivatedEventArgs args)
        {
            if (configurationSource != null)
            {
                configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
            }
        }

        static void Window_Closed(object sender, WindowEventArgs args)
        {
            if (micaController != null)
            {
                micaController.Dispose();
                micaController = null;
            }
            if (acrylicController != null)
            {
                acrylicController.Dispose();
                acrylicController = null;
            }
            configurationSource = null;
        }

        static void RootElement_ActualThemeChanged(FrameworkElement sender, object args)
        {
            if (configurationSource != null)
            {
                switch (sender.ActualTheme)
                {
                    case ElementTheme.Dark: configurationSource.Theme = SystemBackdropTheme.Dark; break;
                    case ElementTheme.Light: configurationSource.Theme = SystemBackdropTheme.Light; break;
                    case ElementTheme.Default: configurationSource.Theme = SystemBackdropTheme.Default; break;
                }
            }
        }
    }

    /// <summary>
    /// 尝试设置自定义标题栏
    /// </summary>
    /// <param name="window">要设置自定义标题栏的窗体</param>
    /// <param name="rootElement">窗体内的根元素</param>
    /// <param name="customTitleBar">自定义标题栏<para>为 null 时设置为系统标题栏</para></param>
    /// <returns>是否设置成功</returns>
    public static bool TrySetCustomTitleBar(this Window window, FrameworkElement? rootElement, ICustomTitleBar customTitleBar)
    {
        if (AppWindowTitleBar.IsCustomizationSupported())
        {
            var appWindow = window.GetAppWindows();

            appWindow.TitleBar.ExtendsContentIntoTitleBar = true;

            customTitleBar.GetAppTitleBar().Loaded += (sender, e) => SetDragRegionForCustomTitleBar(window, customTitleBar);
            customTitleBar.GetAppTitleBar().SizeChanged += (sender, e) => SetDragRegionForCustomTitleBar(window, customTitleBar);
            if (rootElement != null)
            {
                rootElement.ActualThemeChanged += (sender, args) => SetTitleBarTheme(window, sender);
                SetTitleBarTheme(window, rootElement);
            }
            return true;
        }
        else
        {
            return false;
        }

        static void SetDragRegionForCustomTitleBar(Window window, ICustomTitleBar customTitleBar)
        {
            var appWindow = window.GetAppWindows();

            if (customTitleBar != null && appWindow.TitleBar.ExtendsContentIntoTitleBar && AppWindowTitleBar.IsCustomizationSupported())
            {
                var scaleAdjustment = GetScaleAdjustment(window);
                var rects = customTitleBar.GetDragRects(window, scaleAdjustment);
                if (rects.Any())
                {
                    appWindow.TitleBar.SetDragRectangles(rects.ToArray());
                }
            }
        }

        static void SetTitleBarTheme(Window window, FrameworkElement rootElement)
        {
            var appWindow = window.GetAppWindows();

            var titleBar = appWindow.TitleBar;

            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            ApplicationTheme currentTheme = rootElement.RequestedTheme == ElementTheme.Dark ? ApplicationTheme.Dark : ApplicationTheme.Light;
            if (rootElement.RequestedTheme == ElementTheme.Default)
            {
                currentTheme = GetAppTheme();
            }
            if (currentTheme == ApplicationTheme.Dark)
            {
                titleBar.ButtonForegroundColor = Colors.White;
                titleBar.ButtonHoverForegroundColor = Colors.White;
                titleBar.ButtonPressedForegroundColor = Colors.White;
                titleBar.ButtonInactiveForegroundColor = Colors.Gray;

                titleBar.ButtonHoverBackgroundColor = Colors.Gray;
                titleBar.ButtonPressedBackgroundColor = Color.FromArgb(255, 96, 96, 96);
            }
            else
            {
                titleBar.ButtonForegroundColor = Colors.Black;
                titleBar.ButtonHoverForegroundColor = Colors.Black;
                titleBar.ButtonPressedForegroundColor = Colors.Black;
                titleBar.ButtonInactiveForegroundColor = Colors.LightGray;

                titleBar.ButtonHoverBackgroundColor = Colors.LightGray;
                titleBar.ButtonPressedBackgroundColor = Color.FromArgb(255, 96, 96, 96);
            }
        }

        static double GetScaleAdjustment(Window window)
        {
            var hWnd = WindowNative.GetWindowHandle(window);
            var wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            var displayArea = DisplayArea.GetFromWindowId(wndId, DisplayAreaFallback.Primary);
            var hMonitor = Win32Interop.GetMonitorFromDisplayId(displayArea.DisplayId);

            // Get DPI.
            var result = Shcore.GetDpiForMonitor(hMonitor, MonitorDPIType.MDT_Default, out var dpiX, out var _);
            if (result != 0)
            {
                ThrowHelper.ThrowInvalidOperationException("Could not get DPI for monitor.");
            }

            var scaleFactorPercent = (uint)(((long)dpiX * 100 + (96 >> 1)) / 96);
            return scaleFactorPercent / 100.0;
        }
    }

    public static IEnumerable<RectInt32> Clip(this RectInt32 parentRect, IEnumerable<RectInt32> childRects)
    {
        var _childRects = childRects.OrderBy(x => x.X).ToArray();
        List<RectInt32> results = new List<RectInt32>();
        if (!childRects.Any()) { return results; }
        for (int index = 0; index <= childRects.Count(); index++)
        {
            if (index != childRects.Count())
            {
                results.Add(new(_childRects[index].X, parentRect.Y,
                    _childRects[index].Width, parentRect.Height - _childRects[index].Y));
                results.Add(new(_childRects[index].X, _childRects[index].Y + _childRects[index].Height,
                    _childRects[index].Width, parentRect.Height - _childRects[index].Y - _childRects[index].Height));
            }

            if (index == 0)
            {
                results.Add(new(parentRect.X, parentRect.Y, _childRects[index].X, parentRect.Height));
            }
            else if (index == childRects.Count())
            {
                results.Add(new(_childRects[index - 1].X + _childRects[index - 1].Width, parentRect.Y,
                    parentRect.Width - _childRects[index - 1].X - _childRects[index - 1].Width, parentRect.Height));
            }
            else
            {
                results.Add(new(_childRects[index - 1].X + _childRects[index - 1].Width, parentRect.Y,
                    _childRects[index].X - _childRects[index - 1].X - _childRects[index - 1].Width, parentRect.Height));
            }
        }
        return results;
    }

    private static bool Contains(this RectInt32 parentRect, RectInt32 childRect)
    {
        return childRect.X >= parentRect.X &&
            childRect.Y >= parentRect.Y &&
            childRect.Width <= parentRect.Width &&
            childRect.Height <= parentRect.Height;
    }
}

/// <summary>
/// 窗体背景类型
/// </summary>
public enum Backdrop
{
    /// <summary>
    /// 如果支持的话，使用 Mica 背景
    /// </summary>
    Mica,
    /// <summary>
    /// 如果支持的话，使用 Acrylic 背景
    /// </summary>
    Acrylic,
    /// <summary>
    /// 不设置背景
    /// </summary>
    Custom,
}

public enum TitleBar
{
    System,
    Custom,
}
