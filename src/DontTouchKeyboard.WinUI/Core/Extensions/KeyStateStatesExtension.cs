namespace DontTouchKeyboard.WinUI.Core.Extensions;

internal static class KeyStateStatesExtension
{
    public static VisibleType ToLockedVisibility(this KeyState states)
    {
        return states.IsLock() ? VisibleType.Visible : VisibleType.Collapsed;
    }
}
