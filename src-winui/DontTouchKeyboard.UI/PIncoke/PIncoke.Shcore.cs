namespace PIncoke;
internal static class Shcore
{
    [DllImport("Shcore.dll", SetLastError = true)]
    public static extern int GetDpiForMonitor(IntPtr hmonitor, MonitorDPIType dpiType, out uint dpiX, out uint dpiY);
}

internal enum MonitorDPIType : int
{
    MDT_Effective_DPI = 0,
    MDT_Angular_DPI = 1,
    MDT_Raw_DPI = 2,
    MDT_Default = MDT_Effective_DPI
}