using Microsoft.UI.Input;
using Windows.System;
using Windows.UI.Core;

namespace DontTouchKeyboard.Controls.Core
{
    public class KeyStates
    {
        public KeyStates()
        {
            Shift = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Shift);
            CapsLock = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.CapitalLock);
        }

        public KeyStates(CoreVirtualKeyStates shift, CoreVirtualKeyStates capsLock)
        {
            Shift = shift;
            CapsLock = capsLock;
        }

        public CoreVirtualKeyStates Shift { get; }
        public CoreVirtualKeyStates CapsLock { get; }

    }
}
