namespace PInvoke;

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