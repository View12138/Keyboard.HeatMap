using System;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DontTouchKeyboard.UI.Core
{
    public class KeyEventArgs : EventArgs
    {
        public KeyEventArgs(VirtualKey key) => Key = key;
        public VirtualKey Key { get; }
    }
}
