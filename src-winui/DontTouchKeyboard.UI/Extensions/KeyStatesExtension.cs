using DontTouchKeyboard.UI.Core;

namespace DontTouchKeyboard.UI.Extensions
{
    internal static class KeyStatesExtension
    {
        public static bool IsUpper(this KeyStates states)
        {
            if (states.Shift.IsDown() && !states.CapsLock.IsDownOrLocked())
            {
                return true;
            }
            else if (!states.Shift.IsDown() && states.CapsLock.IsDownOrLocked())
            {
                return true;
            }
            return false;
        }

        public static bool IsShiftDown(this KeyStates states)
        {
            return states.Shift.IsDown();
        }
    }
}
