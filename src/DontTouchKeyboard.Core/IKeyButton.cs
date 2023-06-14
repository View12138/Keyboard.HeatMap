using DontTouchKeyboard.Core.Internals;
using DontTouchKeyboard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontTouchKeyboard.Core;

public interface IKeyButton
{
    /// <summary>
    /// 获取或设置按键码
    /// <list type="bullet">
    /// <item><see cref="Key"/> 的 <see cref="int"/> 类型包装</item>
    /// <item>当不是键码预定义的 <see cref="Keys"/> 时，使用 <see cref="KeyCode"/> 代替 <see cref="Key"/></item>
    /// </list>
    /// </summary>
    public int KeyCode { get; set; }

    /// <summary>
    /// 获取或设置虚拟键码
    /// </summary>
    public Keys Key { get; set; }

    /// <summary>
    /// 获取或设置锁定状态的可见性
    /// </summary>
    public VisibleType LockVisibility { get; set; }

    /// <summary>
    /// 获取或设置定位键标识的可见性
    /// </summary>
    public VisibleType LocationVisibility { get; set; }

    /// <summary>
    /// 按键尺寸
    /// </summary>
    public SizeType Size { get; set; }
} 
