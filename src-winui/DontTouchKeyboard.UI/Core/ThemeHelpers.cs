using System.Linq;

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
    public static bool TrySetWindowTheme(Window window, FrameworkElement? rootElement, ElementTheme theme = ElementTheme.Default)
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

    /// <summary>
    /// 尝试设置背景
    /// </summary>
    /// <param name="window">要设置背景的窗体</param>
    /// <param name="rootElement">窗体内的根元素</param>
    /// <param name="backdrop">背景类型</param>
    /// <returns>是否设置成功</returns>
    public static bool TrySetSystemBackdrop(Window window, FrameworkElement? rootElement, Backdrop backdrop = Backdrop.Auto)
    {
        if (backdrop == Backdrop.Custom)
        {
            return true;
        }
        bool micaMode = backdrop switch
        {
            Backdrop.Auto or Backdrop.Mica => true,
            _ => false,
        };
        bool acrylicMode = backdrop switch
        {
            Backdrop.Auto or Backdrop.Acrylic => true,
            _ => false,
        };

        MicaController? micaController = null;
        DesktopAcrylicController? acrylicController = null;
        if (MicaController.IsSupported() || DesktopAcrylicController.IsSupported())
        {
            var wsdqHelper = new WindowsSystemDispatcherQueueHelper();
            wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

            // Create the policy object.
            var configurationSource = new SystemBackdropConfiguration();
            window.Activated += (sender, args) =>
            {
                if (configurationSource != null)
                {
                    configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
                }
            };
            window.Closed += (sender, args) =>
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
            };
            if (rootElement != null)
            {
                rootElement.ActualThemeChanged += (sender, args) => SetConfigurationSourceTheme(configurationSource, sender);
            }

            // Initial configuration state.
            configurationSource.IsInputActive = true;
            SetConfigurationSourceTheme(configurationSource, rootElement);

            if (micaMode && MicaController.IsSupported())
            {
                if (window.Content is Panel panel)
                { panel.Background = null; }

                micaController = new MicaController();

                micaController.AddSystemBackdropTarget(window.As<ICompositionSupportsSystemBackdrop>());
                micaController.SetSystemBackdropConfiguration(configurationSource);
            }
            else if (acrylicMode && DesktopAcrylicController.IsSupported())
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

        static void SetConfigurationSourceTheme(SystemBackdropConfiguration? m_configurationSource, FrameworkElement? rootElement)
        {
            if (m_configurationSource != null)
            {
                switch (rootElement?.ActualTheme)
                {
                    case ElementTheme.Dark: m_configurationSource.Theme = SystemBackdropTheme.Dark; break;
                    case ElementTheme.Light: m_configurationSource.Theme = SystemBackdropTheme.Light; break;
                    case ElementTheme.Default: m_configurationSource.Theme = SystemBackdropTheme.Default; break;
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
    public static bool TrySetCustomTitleBar(Window window, FrameworkElement? rootElement, ICustomTitleBar customTitleBar)
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
}

/// <summary>
/// 窗体背景类型
/// </summary>
public enum Backdrop
{
    /// <summary>
    /// 自动 <c>( Mica &gt; Acrylic &gt; Custom )</c>
    /// </summary>
    Auto,
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