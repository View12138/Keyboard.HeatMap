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
    /// <param name="rootPanel">窗体内的根元素</param>
    /// <param name="theme">要设置的主题</param>
    /// <returns>是否设置成功</returns>
    public static bool TrySetWindowTheme(this Window window, Panel? rootPanel, ElementTheme theme = ElementTheme.Default)
    {
        var hwnd = WindowNative.GetWindowHandle(window);
        int useImmersiveDarkMode = theme switch
        {
            ElementTheme.Light => 0,
            ElementTheme.Dark => 1,
            _ => GetAppTheme() == ApplicationTheme.Light ? 0 : 1,
        };
        var result = DwmSetWindowAttribute(hwnd, DWMWAImmersiveDarkMode, ref useImmersiveDarkMode, sizeof(int)) == 0;
        if (result && rootPanel != null)
        {
            rootPanel.RequestedTheme = theme;
        }
        return result;
    }


    private static SystemBackdropConfiguration? configurationSource = null;
    private static MicaController? micaController = null;
    private static DesktopAcrylicController? acrylicController = null;
    private static Action? setBackground = null;

    /// <summary>
    /// 尝试设置背景
    /// </summary>
    /// <param name="window">要设置背景的窗体</param>
    /// <param name="rootPanel">窗体内的根元素</param>
    /// <param name="backdrop">背景类型</param>
    /// <returns>是否设置成功</returns>
    public static bool TrySetSystemBackdrop(this Window window, Panel? rootPanel, Backdrop backdrop = Backdrop.Mica, Action? setBackground = null)
    {
        if (setBackground != null)
        {
            ThemeHelpers.setBackground = setBackground;
        }
        CleanBackdrop(window, rootPanel);
        if (backdrop == Backdrop.Custom)
        {
            if (rootPanel != null)
            {
                rootPanel.ActualThemeChanged += RootElement_CustomActualThemeChanged;
            }
            return true;
        }
        ThemeHelpers.setBackground?.Invoke();
        if (MicaController.IsSupported() || DesktopAcrylicController.IsSupported())
        {
            configurationSource = new SystemBackdropConfiguration();
            var wsdqHelper = new WindowsSystemDispatcherQueueHelper();
            wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

            // Create the policy object.
            window.Activated += Window_Activated;
            window.Closed += Window_Closed;
            if (rootPanel != null)
            {
                rootPanel.ActualThemeChanged += RootElement_ActualThemeChanged;
                rootPanel.Background = null;
                RootElement_ActualThemeChanged(rootPanel, new object());
            }

            // Initial configuration state.
            configurationSource.IsInputActive = true;

            if (backdrop == Backdrop.Mica && MicaController.IsSupported())
            {
                micaController = new MicaController();

                micaController.AddSystemBackdropTarget(window.As<ICompositionSupportsSystemBackdrop>());
                micaController.SetSystemBackdropConfiguration(configurationSource);
            }
            else if (backdrop == Backdrop.Acrylic && DesktopAcrylicController.IsSupported())
            {
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
            CleanBackdrop(sender as Window, (sender as Window)?.Content as Panel);
        }

        static void CleanBackdrop(Window? window, Panel? rootPanel)
        {
            micaController?.Dispose();
            micaController = null;
            acrylicController?.Dispose();
            acrylicController = null;
            if (window != null)
            {
                window.Activated -= Window_Activated;
                window.Closed -= Window_Closed;
            }
            if (rootPanel != null)
            {
                rootPanel.ActualThemeChanged -= RootElement_ActualThemeChanged;
                rootPanel.ActualThemeChanged -= RootElement_CustomActualThemeChanged;
            }
            configurationSource = null;
        }

        static void RootElement_ActualThemeChanged(FrameworkElement sender, object args)
        {
            if (configurationSource != null)
            {
                configurationSource.Theme = sender.ActualTheme switch
                {
                    ElementTheme.Dark => SystemBackdropTheme.Dark,
                    ElementTheme.Light => SystemBackdropTheme.Light,
                    _ => SystemBackdropTheme.Default
                };
            }
        }

        static void RootElement_CustomActualThemeChanged(FrameworkElement sender, object args)
        {
            if (sender is Panel rootPanel)
            {
                int useImmersiveDarkMode = rootPanel.ActualTheme switch
                {
                    ElementTheme.Light => 0,
                    ElementTheme.Dark => 1,
                    _ => GetAppTheme() == ApplicationTheme.Light ? 0 : 1,
                };
                var color = useImmersiveDarkMode == 0 ? Color.FromArgb(255, 242, 242, 242) : Color.FromArgb(255, 32, 32, 32);
                rootPanel.SetValue(Panel.BackgroundProperty, new SolidColorBrush(color));
                ThemeHelpers.setBackground?.Invoke();
            }
        }
    }

    /// <summary>
    /// 尝试设置自定义标题栏
    /// </summary>
    /// <param name="window">要设置自定义标题栏的窗体</param>
    /// <param name="rootPanel">窗体内的根元素</param>
    /// <param name="customTitleBar">自定义标题栏<para>为 null 时设置为系统标题栏</para></param>
    /// <returns>是否设置成功</returns>
    public static bool TrySetCustomTitleBar(this Window window, Panel? rootPanel, ICustomTitleBar customTitleBar)
    {
        if (AppWindowTitleBar.IsCustomizationSupported())
        {
            var appWindow = window.GetAppWindows();

            appWindow.TitleBar.ExtendsContentIntoTitleBar = true;

            customTitleBar.GetAppTitleBar().Loaded += (sender, e) => SetDragRegionForCustomTitleBar(window, customTitleBar);
            customTitleBar.GetAppTitleBar().SizeChanged += (sender, e) => SetDragRegionForCustomTitleBar(window, customTitleBar);
            if (rootPanel != null)
            {
                rootPanel.ActualThemeChanged += (sender, args) => SetTitleBarTheme(window, (sender as Panel)!);
                SetTitleBarTheme(window, rootPanel);
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

        static void SetTitleBarTheme(Window window, Panel rootPanel)
        {
            var appWindow = window.GetAppWindows();

            var titleBar = appWindow.TitleBar;

            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            ApplicationTheme currentTheme = rootPanel.RequestedTheme == ElementTheme.Dark ? ApplicationTheme.Dark : ApplicationTheme.Light;
            if (rootPanel.RequestedTheme == ElementTheme.Default)
            {
                currentTheme = GetAppTheme();
            }
            if (currentTheme == ApplicationTheme.Dark)
            {
                titleBar.BackgroundColor = Colors.Black;
                titleBar.InactiveBackgroundColor = Colors.Black;

                titleBar.ButtonForegroundColor = Colors.White;
                titleBar.ButtonHoverForegroundColor = Colors.White;
                titleBar.ButtonPressedForegroundColor = Colors.White;
                titleBar.ButtonInactiveForegroundColor = Colors.Gray;

                titleBar.ButtonHoverBackgroundColor = Colors.Gray;
                titleBar.ButtonPressedBackgroundColor = Color.FromArgb(255, 96, 96, 96);
            }
            else
            {
                titleBar.BackgroundColor = Colors.White;
                titleBar.InactiveBackgroundColor = Colors.White;

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
            var result = SHCore.GetDpiForMonitor(hMonitor, MONITOR_DPI_TYPE.MDT_DEFAULT, out var dpiX, out var _);
            if (result.Value != HResult.Code.S_OK)
            {
                ThrowHelper.ThrowInvalidOperationException("Could not get DPI for monitor.");
            }

            var scaleFactorPercent = (uint)(((long)dpiX * 100 + (96 >> 1)) / 96);
            return scaleFactorPercent / 100.0;
        }
    }

    public static T? GetResource<T>(this FrameworkElement element, string name) where T : class
    {
        if (element.Resources.TryGetValue(name, out object resource))
        {
            return resource as T;
        }
        return null;
    }

    public static T GetRequiredResource<T>(this FrameworkElement element, string name)
    {
        if (!element.Resources.TryGetValue(name, out object resource))
        {
            ThrowHelper.ThrowInvalidOperationException($"资源 '{name}' 不存在");
        }
        if (resource is not T)
        {
            ThrowHelper.ThrowInvalidOperationException($"资源 '{name}' 不是 '{typeof(T).Name}' 类型");
        }
        return (T)resource;
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
