using DontTouchKeyboard.Core.Models;

namespace DontTouchKeyboard.Core;

public interface IKeyboard
{
    /// <summary>
    /// 获取或设置 CapsLock 键的状态
    /// </summary>
    public KeyState CapitalLockState { get; set; }

    /// <summary>
    /// 获取或设置 NumLock 键的状态
    /// </summary>
    public KeyState NumberKeyLockState { get; set; }

    /// <summary>
    /// 获取或设置 Insert 键的状态
    /// </summary>
    public KeyState InsertState { get; set; }

    /// <summary>
    /// 获取或设置 Scroll 键的状态
    /// </summary>
    public KeyState ScrollState { get; set; }

    /// <summary>
    /// 获取或设置 Shift 键的状态
    /// </summary>
    public KeyState ShiftState { get; set; }

    /// <summary>
    /// 获取或设置 Control 键的状态
    /// </summary>
    public KeyState ControlState { get; set; }

    /// <summary>
    /// 获取当前键盘上的所有按键
    /// </summary>
    public IEnumerable<IKeyButton> Keys { get; }
}