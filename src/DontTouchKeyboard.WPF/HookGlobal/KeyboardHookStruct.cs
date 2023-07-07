namespace Keyboard.HeatMap.HookGlobal;

/// <summary>
/// 声明键盘钩子的封送结构类型
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public class KeyboardHookStruct
{
    /// <summary>
    /// 表示一个在1到254间的虚似键盘码
    /// </summary>
    public int vkCode; //表示一个在1到254间的虚似键盘码
    /// <summary>
    /// 表示硬件扫描码
    /// </summary>
    public int scanCode; //表示硬件扫描码
    public int flags;
    public int time;
    public int dwExtraInfo;
}
