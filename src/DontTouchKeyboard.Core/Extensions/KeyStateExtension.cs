namespace DontTouchKeyboard.Core.Models;

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