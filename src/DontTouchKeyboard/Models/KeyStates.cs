namespace DontTouchKeyboard.Models;

public readonly struct KeyStates
{
    private readonly KeyState Shift;
    private readonly KeyState CapsLock;

    public KeyStates() => throw new InvalidOperationException();

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