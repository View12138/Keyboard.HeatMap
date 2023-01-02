using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Composition;
using Microsoft.Win32;
using WinRT;
using PIncoke;
using Windows.UI;

namespace DontTouchKeyboard.UI.Core;

// Based on https://stackoverflow.com/a/62811758/5001796
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

    public static bool OnThemeChange(Window window, FrameworkElement? rootElement, ElementTheme theme = ElementTheme.Default)
    {
        var hwnd = WindowNative.GetWindowHandle(window);
        int useImmersiveDarkMode = theme switch
        {
            ElementTheme.Light => 0,
            ElementTheme.Dark => 1,
            _ => GetAppTheme() == ApplicationTheme.Light ? 0 : 1,
        };
        if (rootElement != null)
        {
            rootElement.RequestedTheme = theme;
        }
        return DwmSetWindowAttribute(hwnd, DWMWAImmersiveDarkMode, ref useImmersiveDarkMode, sizeof(int)) == 0;
    }

    /// <summary>
    /// 尝试设置背景
    /// </summary>
    /// <param name="window"></param>
    /// <returns></returns>
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

        MicaController? m_micaController = null;
        DesktopAcrylicController? m_acrylicController = null;
        if (MicaController.IsSupported() || DesktopAcrylicController.IsSupported())
        {
            var m_wsdqHelper = new WindowsSystemDispatcherQueueHelper();
            m_wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

            // Create the policy object.
            var m_configurationSource = new SystemBackdropConfiguration();
            window.Activated += (sender, args) =>
            {
                if (m_configurationSource != null)
                {
                    m_configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
                }
            };
            window.Closed += (sender, args) =>
            {
                if (m_micaController != null)
                {
                    m_micaController.Dispose();
                    m_micaController = null;
                }
                if (m_acrylicController != null)
                {
                    m_acrylicController.Dispose();
                    m_acrylicController = null;
                }
                m_configurationSource = null;
            };
            if (rootElement != null)
            {
                rootElement.ActualThemeChanged += (sender, args) => SetConfigurationSourceTheme(m_configurationSource, sender);
            }

            // Initial configuration state.
            m_configurationSource.IsInputActive = true;
            SetConfigurationSourceTheme(m_configurationSource, rootElement);

            if (micaMode && MicaController.IsSupported())
            {
                if (window.Content is Panel panel)
                { panel.Background = null; }

                m_micaController = new MicaController();

                m_micaController.AddSystemBackdropTarget(window.As<ICompositionSupportsSystemBackdrop>());
                m_micaController.SetSystemBackdropConfiguration(m_configurationSource);
            }
            else if (acrylicMode && DesktopAcrylicController.IsSupported())
            {
                if (window.Content is Panel panel)
                { panel.Background = null; }

                m_acrylicController = new DesktopAcrylicController();
                m_acrylicController.AddSystemBackdropTarget(window.As<ICompositionSupportsSystemBackdrop>());
                m_acrylicController.SetSystemBackdropConfiguration(m_configurationSource);
            }
            return true;
        }
        return false;

        void SetConfigurationSourceTheme(SystemBackdropConfiguration? m_configurationSource, FrameworkElement? rootElement)
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

    public static bool TrySetCustomTitleBar(Window window, FrameworkElement? rootElement, ICustomTitleBar? customTitleBar)
    {
        if (AppWindowTitleBar.IsCustomizationSupported())
        {
            var hWnd = WindowNative.GetWindowHandle(window);
            var wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = AppWindow.GetFromWindowId(wndId);

            appWindow.TitleBar.ExtendsContentIntoTitleBar = true;

            if (customTitleBar != null)
            {
                customTitleBar.GetAppTitleBar().Loaded += (sender, e) => SetDragRegionForCustomTitleBar(appWindow, customTitleBar);
                customTitleBar.GetAppTitleBar().SizeChanged += (sender, e) => SetDragRegionForCustomTitleBar(appWindow, customTitleBar);
            }
            if (rootElement != null)
            {
                rootElement.ActualThemeChanged += (sender, args) => SetTitleBarTheme(window, sender, appWindow.TitleBar);
            }
            SetTitleBarTheme(window, rootElement, appWindow.TitleBar);
            return true;
        }
        return false;

        void SetDragRegionForCustomTitleBar(AppWindow appWindow, ICustomTitleBar? customTitleBar)
        {
            if (customTitleBar != null && appWindow.TitleBar.ExtendsContentIntoTitleBar && AppWindowTitleBar.IsCustomizationSupported())
            {
                var scaleAdjustment = GetScaleAdjustment(window);
                appWindow.TitleBar.SetDragRectangles(customTitleBar.GetDragRects(window, scaleAdjustment).ToArray());
            }
        }

        void SetTitleBarTheme(Window window, FrameworkElement? rootElement, AppWindowTitleBar titleBar)
        {
            if (titleBar != null)
            {
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                ApplicationTheme currentTheme = rootElement?.RequestedTheme == ElementTheme.Dark ? ApplicationTheme.Dark : ApplicationTheme.Light;
                if (rootElement?.RequestedTheme == ElementTheme.Default)
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
        }

        double GetScaleAdjustment(Window window)
        {
            var hWnd = WindowNative.GetWindowHandle(window);
            var wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            var displayArea = DisplayArea.GetFromWindowId(wndId, DisplayAreaFallback.Primary);
            var hMonitor = Win32Interop.GetMonitorFromDisplayId(displayArea.DisplayId);

            // Get DPI.
            var result = Shcore.GetDpiForMonitor(hMonitor, MonitorDPIType.MDT_Default, out var dpiX, out var _);
            if (result != 0)
            {
                throw new Exception("Could not get DPI for monitor.");
            }

            var scaleFactorPercent = (uint)(((long)dpiX * 100 + (96 >> 1)) / 96);
            return scaleFactorPercent / 100.0;
        }
    }
}


public enum Backdrop
{
    Auto,
    Mica,
    Acrylic,
    Custom,
}