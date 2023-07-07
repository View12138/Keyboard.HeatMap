namespace DontTouchKeyboard.WinUI.Core;

public class DtkKeyEventArgs : EventArgs
{
    public DtkKeyEventArgs(VirtualKey key)
    {
        Key = key;
    }

    public VirtualKey Key
    {
        get;
    }
}
