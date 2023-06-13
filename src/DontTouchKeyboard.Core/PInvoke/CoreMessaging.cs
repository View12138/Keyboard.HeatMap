using System.Runtime.InteropServices;

namespace PInvoke;

public static class CoreMessaging
{
    [DllImport("CoreMessaging.dll")]
    public static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object? dispatcherQueueController);
}


[StructLayout(LayoutKind.Sequential)]
public struct DispatcherQueueOptions
{
    public int dwSize;
    public int threadType;
    public int apartmentType;
}

public static class User32Ext
{
    /// <summary>
    /// 转换为 ACSII 字符
    /// </summary>
    /// <param name="uVirtKey"></param>
    /// <param name="uScanCode"></param>
    /// <param name="lpbKeyState"></param>
    /// <param name="lpwTransKey"></param>
    /// <param name="fuState"></param>
    /// <returns></returns>
    [DllImport("user32.dll", EntryPoint = "ToAscii")]
    public static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpKeyState, byte[] lpChar, int uFlags);

    /// <summary>
    /// 获取按键状态
    /// </summary>
    /// <param name="pbKeyState"></param>
    /// <returns></returns>
    [DllImport("user32.dll", EntryPoint = "GetKeyboardState")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetKeyboardState(byte[] pbKeyState);
}