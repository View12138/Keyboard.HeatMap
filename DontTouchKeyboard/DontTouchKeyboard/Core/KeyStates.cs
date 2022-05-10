using Microsoft.UI.Input;
using Windows.System;
using Windows.UI.Core;

namespace DontTouchKeyboard.Core
{
    public class KeyStates
    {
        public static KeyStates Instance => new(CoreVirtualKeyStates.None, CoreVirtualKeyStates.None);
        public KeyStates() : this(InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Shift), InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.CapitalLock)) { }

        public KeyStates(CoreVirtualKeyStates shift, CoreVirtualKeyStates capsLock)
        {
            Shift = shift;
            CapsLock = capsLock;
        }

        public CoreVirtualKeyStates Shift { get; }
        public CoreVirtualKeyStates CapsLock { get; }

    }
}
