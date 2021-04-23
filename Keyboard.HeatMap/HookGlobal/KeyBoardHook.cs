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
    public class KeyBoardHook
    {
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_SYSKEYUP = 0x105;

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
        static int hKeyboardHook = 0;

        /// <summary>
        /// 鼠标常量
        /// </summary>
        public const int WH_KEYBOARD_LL = 13; //keyboard hook constant

        /// <summary>
        /// 声明键盘钩子事件类型
        /// </summary>
        Win32.HookProc KeyboardHookProcedure;

        /// <summary>
        /// 先前按下的键
        /// </summary>
        public List<Keys> preKeys = new List<Keys>();

        /// <summary>
        /// 墨认的构造函数构造当前类的实例并自动的运行起来.
        /// </summary>
        public KeyBoardHook()
        {
            Start();
        }

        //析构函数.
        ~KeyBoardHook()
        {
            Stop();
        }

        /// <summary>
        /// 安装键盘钩子
        /// </summary>
        private void Start()
        {
            if (hKeyboardHook == 0)
            { // 安装键盘钩子
                KeyboardHookProcedure = new Win32.HookProc(KeyboardHookProc);
                ProcessModule curModule = Process.GetCurrentProcess().MainModule;
                IntPtr moduleHandle = Win32.GetModuleHandle(curModule.ModuleName);
                hKeyboardHook = Win32.SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, moduleHandle, 0);

                if (hKeyboardHook == 0)
                { throw new Exception("SetWindowsHookEx ist failed."); }
            }
        }
        /// <summary>
        /// 卸载键盘钩子
        /// </summary>
        private void Stop()
        {
            if (hKeyboardHook != 0)
            { // 卸载键盘钩子
                if (!Win32.UnhookWindowsHookEx(hKeyboardHook))
                { throw new Exception("UnhookWindowsHookEx failed."); }
                else
                { hKeyboardHook = 0; }
            }
        }

        private int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                //当有OnKeyDownEvent 或 OnKeyPressEvent不为null时,ctrl alt shift keyup时 preKeys
                //中的对应的键增加                  
                if (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN)
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                    if (IsCtrlAltShiftKeys(keyData) && preKeys.IndexOf(keyData) == -1)
                    {
                        preKeys.Add(keyData);
                    }
                }
                //引发OnKeyDownEvent
                if (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN)
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                    KeyEventArgs e = new KeyEventArgs(GetDownKeys(keyData));

                    OnKeyDownEvent?.Invoke(this, e);
                }

                //引发OnKeyPressEvent
                if (wParam == WM_KEYDOWN)
                {
                    byte[] keyState = new byte[256];
                    Win32.GetKeyboardState(keyState);

                    byte[] inBuffer = new byte[2];
                    if (Win32.ToAscii(MyKeyboardHookStruct.vkCode,
                    MyKeyboardHookStruct.scanCode,
                    keyState,
                    inBuffer,
                    MyKeyboardHookStruct.flags) == 1)
                    {
                        KeyPressEventArgs e = new KeyPressEventArgs((char)inBuffer[0]);
                        OnKeyPressEvent?.Invoke(this, e);
                    }
                }

                //当有OnKeyDownEvent 或 OnKeyPressEvent不为null时,ctrl alt shift keyup时 preKeys
                //中的对应的键删除
                if (wParam == WM_KEYUP || wParam == WM_SYSKEYUP)
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
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
                if (wParam == WM_KEYUP || wParam == WM_SYSKEYUP)
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                    KeyEventArgs e = new KeyEventArgs(GetDownKeys(keyData));
                    OnKeyUpEvent?.Invoke(this, e);
                }
            }
            return Win32.CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
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