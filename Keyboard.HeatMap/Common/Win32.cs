using System;
using System.Runtime.InteropServices;

namespace Keyboard.HeatMap.Common
{
    /// <summary>
    /// win32
    /// </summary>
    public static partial class Win32
    {

        /// <summary>
        /// 装置钩子的函数
        /// </summary>
        /// <param name="idHook">指示欲被安装的挂钩处理过程之类型</param>
        /// <param name="lpfn"></param>
        /// <param name="hmod"></param>
        /// <param name="dwThreadId"></param>
        /// <returns></returns>
        //[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        [DllImport("user32.dll", EntryPoint = "SetWindowsHookEx")]
        static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hmod, int dwThreadId);

        /// <summary>
        /// 卸下钩子的函数
        /// </summary>
        /// <param name="idHook"></param>
        /// <returns></returns>
        //[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        [DllImport("user32.dll", EntryPoint = "UnhookWindowsHookEx")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        /// <summary>
        /// 下一个钩挂的函数
        /// </summary>
        /// <param name="idHook"></param>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        //[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        [DllImport("user32.dll", EntryPoint = "CallNextHookEx")]
        static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

        /// <summary>
        /// 转换为 ACSII 字符
        /// </summary>
        /// <param name="uVirtKey"></param>
        /// <param name="uScanCode"></param>
        /// <param name="lpbKeyState"></param>
        /// <param name="lpwTransKey"></param>
        /// <param name="fuState"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "ToAscii")]
        public static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpKeyState, byte[] lpChar, int uFlags);

        /// <summary>
        /// 获取按键状态
        /// </summary>
        /// <param name="pbKeyState"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "GetKeyboardState")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetKeyboardState(byte[] pbKeyState);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpModuleName"></param>
        /// <returns></returns>
        //[DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        [DllImport("kernel32.dll", EntryPoint = "GetModuleHandle")]
        public static extern IntPtr GetModuleHandle([In()][MarshalAs(UnmanagedType.LPWStr)] string lpModuleName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "GetDC")]
        public static extern IntPtr GetDC(IntPtr hwnd);
    }

    public static partial class Win32
    {
        public static int SetWindowsHookKeyboard(IdHook idHook, HookProc lpfn, IntPtr hInstance, int threadId = 0)
            => SetWindowsHookEx((int)idHook, lpfn, hInstance, threadId);

        public static int CallNextHookEx(IdHook idHook, int nCode, int wParam, IntPtr lParam)
            => CallNextHookEx((int)idHook, nCode, wParam, lParam);
    }

    /// <summary>
    /// 回调函数类型
    /// </summary>
    /// <param name="nCode"></param>
    /// <param name="wParam"></param>
    /// <param name="lParam"></param>
    /// <returns></returns>
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int HookProc(int code, int wParam, IntPtr lParam);

    /// <summary>
    /// 挂钩类型
    /// </summary>
    public enum IdHook : int
    {
        /// <summary>
        /// 安装一个挂钩处理过程，以监视由对话框、消息框、菜单条、或滚动条中的输入事件引发的消息。
        /// <para>详情参见 <see langword="MessageProc"/> 挂钩处理过程</para>
        /// </summary>
        MsgFilter = -1,
        /// <summary>
        /// 安装一个挂钩处理过程，对寄送至系统消息队列的输入消息进行纪录。
        /// <para>详情参见 <see langword="JournalRecordProc"/> 挂钩处理过程</para>
        /// </summary>
        JournalRecord = 0,
        /// <summary>
        /// 安装一个挂钩处理过程，对此前由 WH_JOURNALRECORD 挂钩处理过程纪录的消息进行寄送。
        /// <para>详情参见 <see langword="JournalPlaybackProc"/> 挂钩处理过程</para>
        /// </summary>
        JournalPlayback = 1,
        /// <summary>
        /// 安装一个挂钩处理过程，对击键消息进行监视。
        /// <para>详情参见 <see langword="KeyboardProc"/> 挂钩处理过程</para>
        /// </summary>
        Keyboard = 2,
        /// <summary>
        /// 安装一个挂钩处理过程，对寄送至消息队列的消息进行监视。
        /// <para>详情参见 <see langword="GetMsgProc"/> 挂钩处理过程</para>
        /// </summary>
        GetMessage = 3,
        /// <summary>
        /// 安装一个挂钩处理过程，在系统将消息发送至目标窗口处理过程之前，对该消息进行监视。
        /// <para>详情参见 <see langword="CallWndProc"/> 挂钩处理过程</para>
        /// </summary>
        CallWndProc = 4,
        /// <summary>
        /// 安装一个挂钩处理过程，接受对CBT应用程序有用的消息。
        /// <para>详情参见 <see langword="CBTProc"/> 挂钩处理过程</para>
        /// </summary>
        CBT = 5,
        /// <summary>
        /// 安装一个挂钩处理过程，以监视由对话框、消息框、菜单条、或滚动条中的输入事件引发的消息。这个挂钩处理过程对系统中所有应用程序的这类消息都进行监视。
        /// <para>详情参见 <see langword="SysMsgProc"/> 挂钩处理过程</para>
        /// </summary>
        SysMsgFilter = 6,
        /// <summary>
        /// 安装一个挂钩处理过程，对鼠标消息进行监视。
        /// <para>详情参见 MouseProc 挂钩处理过程</para>
        /// </summary>
        Mouse = 7,
        /// <summary>
        /// 安装一个挂钩处理过程，以便对其他挂钩处理过程进行调试。
        /// <para>详情参见 <see langword="DebugProc"/> 挂钩处理过程</para>
        /// </summary>
        Debug = 9,
        /// <summary>
        /// 安装一个挂钩处理过程，以接受对外壳应用程序有用的通知。
        /// <para>详情参见 <see langword="ShellProc"/> 挂钩处理过程</para>
        /// </summary>
        Shell = 10,
        /// <summary>
        /// 安装一个挂钩处理过程，该挂钩处理过程当应用程序的前台线程即将进入空闲状态时被调用，它有助于在空闲时间内执行低优先级的任务。
        /// <para></para>
        /// </summary>
        ForegroundIDLE = 11,
        /// <summary>
        /// 安装一个挂钩处理过程，它对已被目标窗口处理过程处理过了的消息进行监视。
        /// <para>详情参见 <see langword="CallWndRetProc"/> 挂钩处理过程</para>
        /// </summary>
        CallWndProcRet = 12,
        /// <summary>
        /// 此挂钩只能在 Windows NT 中被安装，用来对底层的键盘输入事件进行监视。
        /// <para>详情参见 <see langword="LowLevelKeyboardProc"/> 挂钩处理过程</para>
        /// </summary>
        KeyboardLL = 13,
        /// <summary>
        /// 此挂钩只能在 Windows NT 中被安装，用来对底层的鼠标输入事件进行监视。
        /// <para>详情参见 <see langword="LowLevelMouseProc"/> 挂钩处理过程</para>
        /// </summary>
        MouseLL = 14,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardLL
    {
        /// DWORD->int
        public int vkCode;

        /// DWORD->int
        public int scanCode;

        /// DWORD->int
        public int flags;

        /// DWORD->int
        public int time;

        /// ULONG_PTR->int
        public int dwExtraInfo;
    }

}