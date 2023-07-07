namespace DontTouchKeyboard.Models;

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


public static class KeyStateExtension
{

public static bool IsLock(this KeyState states)
{
    return (states & KeyState.Locked) == KeyState.Locked;
}

public static bool IsDown(this KeyState states)
{
    return (states & KeyState.Down) == KeyState.Down;
}

public static bool IsDownOrLocked(this KeyState states)
{
    return states.IsDown() || states.IsLock();
}
}