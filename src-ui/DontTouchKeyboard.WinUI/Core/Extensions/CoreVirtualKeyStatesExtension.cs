using DontTouchKeyboard.Core.Models;

namespace DontTouchKeyboard.UI.Core.Extensions;

internal static class CoreVirtualKeyStatesExtension
{
    public static KeyState ToKeyState(this CoreVirtualKeyStates keyStates)
    {
        return keyStates switch
        {
            CoreVirtualKeyStates.None => KeyState.None,
            CoreVirtualKeyStates.Down => KeyState.Down,
            CoreVirtualKeyStates.Locked => KeyState.Locked,
            _ => throw new NotImplementedException(),
        };
    }
}
