namespace DontTouchKeyboard.UI.Core;

public readonly struct KeyStates
{
    private readonly CoreVirtualKeyStates Shift;
    private readonly CoreVirtualKeyStates CapsLock;

    public KeyStates()
    {
        Shift = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Shift);
        CapsLock = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.CapitalLock);
        //Shift = pressKey == VirtualKey.Shift ? InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Shift) : CoreVirtualKeyStates.None;
        //CapsLock = pressKey == VirtualKey.CapitalLock ? InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.CapitalLock) : CoreVirtualKeyStates.None;
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
