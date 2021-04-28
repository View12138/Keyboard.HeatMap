using System;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;
using Keyboard.HeatMap.Common;

namespace Keyboard.HeatMap.HookGlobal
{
    /// <summary>
    /// 这个类可以让你得到一个在运行中程序的所有键盘事件
    /// 并且引发一个带KeyEventArgs和MouseEventArgs参数的.NET事件以便你很容易使用这些信息
    /// </summary>
    /// <remarks>
    /// 修改:lihx
    /// 修改时间:04.11.8
    /// </remarks>
    public class KeyBoardHook : IDisposable
    {
        private const int WM_KeyDown = 0x100;
        private const int WM_KeyUp = 0x101;
        private const int WM_SysKeyDown = 0x104;
        private const int WM_SysKeyUp = 0x105;

        //全局的事件
        /// <summary>
        /// keyDown
        /// </summary>
        public event KeyEventHandler OnKeyDownEvent;
        /// <summary>
        /// KeyUp
        /// </summary>
        public event KeyEventHandler OnKeyUpEvent;
        /// <summary>
        /// KeyPress
        /// </summary>
        public event KeyPressEventHandler OnKeyPressEvent;

        /// <summary>
        /// 键盘钩子句柄
        /// </summary>
        private static int hKeyboardHook = 0;

        /// <summary>
        /// 先前按下的键
        /// </summary>
        private List<Keys> preKeys = new List<Keys>();

        private HookProc KeyboardHookProc;
        private bool disposed = false;

        /// <summary>
        /// 墨认的构造函数构造当前类的实例并自动的运行起来.
        /// </summary>
        public KeyBoardHook()
        {
            if (hKeyboardHook == 0)
            { // 安装键盘钩子
                KeyboardHookProc = KeyboardHookMethod;
                GC.KeepAlive(KeyboardHookProc);
                ProcessModule curModule = Process.GetCurrentProcess().MainModule;
                IntPtr moduleHandle = Win32.GetModuleHandle(curModule.ModuleName);
                hKeyboardHook = Win32.SetWindowsHookKeyboard(IdHook.KeyboardLL, KeyboardHookProc, moduleHandle);

                if (hKeyboardHook == 0)
                { throw new Exception("SetWindowsHookEx ist failed."); }
            }
        }

        ~KeyBoardHook() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    try
                    {
                        if (hKeyboardHook != 0)
                        { // 卸载键盘钩子
                            if (Win32.UnhookWindowsHookEx(hKeyboardHook))
                            { hKeyboardHook = 0; }
                            else
                            { throw new Exception("UnhookWindowsHookEx failed."); }
                        }
                    }
                    finally
                    {

                    }
                }
                disposed = true;
            }
        }

        private int KeyboardHookMethod(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var param = Marshal.PtrToStructure<KeyboardLL>(lParam);
                //当有OnKeyDownEvent 或 OnKeyPressEvent 不为null时,ctrl alt shift keyup时 preKeys
                //中的对应的键增加                  
                if (wParam == WM_KeyDown || wParam == WM_SysKeyDown)
                {
                    Keys keyData = (Keys)param.vkCode;
                    if (IsCtrlAltShiftKeys(keyData) && preKeys.IndexOf(keyData) == -1)
                    {
                        preKeys.Add(keyData);
                    }
                }
                //引发OnKeyDownEvent
                if (wParam == WM_KeyDown || wParam == WM_SysKeyDown)
                {
                    Keys keyData = (Keys)param.vkCode;
                    KeyEventArgs e = new KeyEventArgs(GetDownKeys(keyData));

                    OnKeyDownEvent?.Invoke(this, e);
                }

                //引发OnKeyPressEvent
                if (wParam == WM_KeyDown)
                {
                    byte[] keyState = new byte[256];
                    Win32.GetKeyboardState(keyState);

                    byte[] inBuffer = new byte[2];
                    if (Win32.ToAscii(param.vkCode,
                    param.scanCode,
                    keyState,
                    inBuffer,
                    param.flags) == 1)
                    {
                        KeyPressEventArgs e = new KeyPressEventArgs((char)inBuffer[0]);
                        OnKeyPressEvent?.Invoke(this, e);
                    }
                }

                //当有OnKeyDownEvent 或 OnKeyPressEvent不为null时,ctrl alt shift keyup时 preKeys
                //中的对应的键删除
                if (wParam == WM_KeyUp || wParam == WM_SysKeyUp)
                {
                    Keys keyData = (Keys)param.vkCode;
                    if (IsCtrlAltShiftKeys(keyData))
                    {
                        for (int i = preKeys.Count - 1; i >= 0; i--)
                        {
                            if (preKeys[i] == keyData)
                            {
                                preKeys.RemoveAt(i);
                            }
                        }
                    }
                }
                //引发OnKeyUpEvent
                if (wParam == WM_KeyUp || wParam == WM_SysKeyUp)
                {
                    Keys keyData = (Keys)param.vkCode;
                    KeyEventArgs e = new KeyEventArgs(GetDownKeys(keyData));
                    OnKeyUpEvent?.Invoke(this, e);
                }
            }
            return Win32.CallNextHookEx(IdHook.KeyboardLL, nCode, wParam, lParam);
        }

        private Keys GetDownKeys(Keys key)
        {
            Keys rtnKey = Keys.None;
            foreach (Keys keyTemp in preKeys)
            {
                switch (keyTemp)
                {
                    case Keys.LControlKey:
                    case Keys.RControlKey:
                        rtnKey |= Keys.Control;
                        break;
                    case Keys.LMenu:
                    case Keys.RMenu:
                        rtnKey |= Keys.Alt;
                        break;
                    case Keys.LShiftKey:
                    case Keys.RShiftKey:
                        rtnKey |= Keys.Shift;
                        break;
                    default:
                        break;
                }
            }
            rtnKey |= key;
            return rtnKey;
        }

        private bool IsCtrlAltShiftKeys(Keys key)
        {
            switch (key)
            {
                case Keys.LControlKey:
                case Keys.RControlKey:
                case Keys.LMenu:
                case Keys.RMenu:
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                    return true;
                default:
                    return false;
            }
        }
    }
}