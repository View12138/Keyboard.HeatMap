using PInvoke;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DontTouchKeyboard.Core.Internals;

public class KeyBoardHook : IDisposable
{
    /// <summary>
    /// keyDown Event
    /// </summary>
    public event KeyEventHandler? OnKeyDownEvent;
    /// <summary>
    /// KeyUp Event
    /// </summary>
    public event KeyEventHandler? OnKeyUpEvent;
    /// <summary>
    /// KeyPress Event
    /// </summary>
    public event KeyPressEventHandler? OnKeyPressEvent;

    #region Initialization
    private static User32.SafeHookHandle keyboardHandle = User32.SafeHookHandle.Null;
    private readonly User32.WindowsHookDelegate? keyboardHook;
    public KeyBoardHook(ProcessModule mainModule)
    {
        if (keyboardHandle.IsInvalid)
        {
            keyboardHook = KeyboardHook;
            GC.KeepAlive(keyboardHook);
            IntPtr moduleHandle = Kernel32.GetModuleHandle(mainModule.ModuleName);
            keyboardHandle = User32.SetWindowsHookEx(User32.WindowsHookType.WH_KEYBOARD_LL, keyboardHook, moduleHandle, 0);

            if (keyboardHandle.IsInvalid)
            { throw new InvalidOperationException("SetWindowsHookEx ist failed."); }
        }
    }
    #endregion

    #region KeyboardHook
    private const int WM_KeyDown = 0x100;
    private const int WM_KeyUp = 0x101;
    private const int WM_SysKeyDown = 0x104;
    private const int WM_SysKeyUp = 0x105;
    private readonly List<Keys> modifierKeys = new();
    private int KeyboardHook(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            var keyParam = Marshal.PtrToStructure<KeyParam>(lParam);
            Keys key = (Keys)keyParam.vkCode;

            if (wParam == WM_KeyDown || wParam == WM_SysKeyDown)
            {
                if (IsModifierKeys(key) && !modifierKeys.Contains(key))
                {
                    modifierKeys.Add(key);
                }
                OnKeyDownEvent?.Invoke(this, new KeyEventArgs(GetKeysWithModifier(key, modifierKeys)));
            }

            if (wParam == WM_KeyDown)
            {
                byte[] keyState = new byte[256];
                if (User32Ext.GetKeyboardState(keyState))
                {
                    byte[] inBuffer = new byte[2];
                    if (User32Ext.ToAscii(keyParam.vkCode, keyParam.scanCode, keyState, inBuffer, keyParam.flags) == 1)
                    {
                        OnKeyPressEvent?.Invoke(this, new KeyPressEventArgs((char)inBuffer[0]));
                    }
                }
            }

            if (wParam == WM_KeyUp || wParam == WM_SysKeyUp)
            {
                if (IsModifierKeys(key))
                {
                    modifierKeys.RemoveAll(x => x == key);
                }

                OnKeyUpEvent?.Invoke(this, new KeyEventArgs(GetKeysWithModifier(key, modifierKeys)));
            }
        }
        return User32.CallNextHookEx((nint)User32.WindowsHookType.WH_KEYBOARD_LL, nCode, wParam, lParam);
    }

    private static Keys GetKeysWithModifier(Keys key, List<Keys> modifierKeys)
    {
        Keys rtnKey = Keys.None;
        foreach (Keys modifierKey in modifierKeys)
        {
            switch (modifierKey)
            {
                case Keys.LeftControl:
                case Keys.RightControl:
                    rtnKey |= Keys.ControlModifier;
                    break;
                case Keys.LeftMenu:
                case Keys.RightMenu:
                    rtnKey |= Keys.AltModifier;
                    break;
                case Keys.LeftShift:
                case Keys.RightShift:
                    rtnKey |= Keys.ShiftModifier;
                    break;
                default:
                    break;
            }
        }
        rtnKey |= key;
        return rtnKey;
    }

    private static bool IsModifierKeys(Keys key) => key switch
    {
        Keys.LeftControl or Keys.RightControl or Keys.LeftMenu or Keys.RightMenu or Keys.LeftShift or Keys.RightShift => true,
        _ => false,
    };
    #endregion

    #region IDisposable
    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                if (!keyboardHandle.IsInvalid)
                {
                    keyboardHandle.DangerousRelease();
                    keyboardHandle = User32.SafeHookHandle.Null;
                }
            }
            disposedValue = true;
        }
    }

    ~KeyBoardHook()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}

public record KeyEventArgs(Keys Key);
public delegate void KeyEventHandler(object sender, KeyEventArgs args);
public record KeyPressEventArgs(char Key);
public delegate void KeyPressEventHandler(object sender, KeyPressEventArgs args);


[StructLayout(LayoutKind.Sequential)]
internal struct KeyParam
{
    public int vkCode;
    public int scanCode;
    public int flags;
    public int time;
    public int dwExtraInfo;
}
