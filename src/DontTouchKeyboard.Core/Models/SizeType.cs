namespace DontTouchKeyboard.Core.Models;

/// <summary>
/// 按键尺寸
/// </summary>
public enum SizeType
{
    /// <summary>
    /// 0.5 Default
    /// </summary>
    Mini,
    /// <summary>
    /// 1 Default
    /// </summary>
    Default,
    /// <summary>
    /// 1.5 Default
    /// </summary>
    Medium,
    /// <summary>
    /// 2 Default
    /// </summary>
    Large,
    /// <summary>
    /// 2.5 Default
    /// </summary>
    XLarge,
    /// <summary>
    /// 3 Default
    /// </summary>
    XXLarge,
    /// <summary>
    /// 5 Default
    /// </summary>
    XXXLarge,
    /// <summary>
    /// 1 Default &amp; Height
    /// </summary>
    Function,
    /// <summary>
    /// 1.5 Default &amp; 0.8 Height
    /// </summary>
    FunctionMedium,
    /// <summary>
    /// 1 Default &amp; 2 Height
    /// </summary>
    VerticalLarge,
    CapsLock,
}

/// <summary>
/// Visibility
/// </summary>
public enum VisibleType
{
    /// <summary>
    /// Display the element.
    /// </summary>
    Visible,
    /// <summary>
    /// Do not display the element, and do not reserve space for it in layout.
    /// </summary>
    Collapsed
}

public enum KeyState : uint
{
    /// <summary>
    /// The key is up or in no specific state.
    /// </summary>
    None = 0x0u,
    /// <summary>
    /// The key is pressed down for the input event.
    /// </summary>
    Down = 0x1u,
    /// <summary>
    /// The key is in a toggled or modified state (for example, Caps Lock) for the input event.
    /// </summary>
    Locked = 0x2u
}

public readonly struct KeyStates
{
    private readonly KeyState Shift;
    private readonly KeyState CapsLock;

    public KeyStates(KeyState shift, KeyState capsLock)
    {
        Shift = shift;
        CapsLock = capsLock;
    }

    /// <summary>
    /// 获取一个值，指示当前是否是大写环境
    /// </summary>
    /// <returns></returns>
    public bool IsUpper()
    {
        if (Shift.IsDown() && !CapsLock.IsDownOrLocked())
        {
            return true;
        }
        else if (!Shift.IsDown() && CapsLock.IsDownOrLocked())
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获取一个值，指示 Shift 键是否按下
    /// </summary>
    /// <returns></returns>
    public bool IsShiftDown()
    {
        return Shift.IsDown();
    }
}