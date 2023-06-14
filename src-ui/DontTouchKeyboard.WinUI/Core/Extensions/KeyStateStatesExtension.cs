using DontTouchKeyboard.Core.Models;

namespace DontTouchKeyboard.UI.Core.Extensions;

internal static class KeyStateStatesExtension
{
    public static VisibleType ToLockedVisibility(this KeyState states)
    {
        return states.IsLock() ? VisibleType.Visible : VisibleType.Collapsed;
    }
}
