namespace DontTouchKeyboard.WinUI.Core;

public readonly struct KeyStatesFactory
{
    public static KeyStates KeyStates => new(InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Shift).ToKeyState(), InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.CapitalLock).ToKeyState());
}
