// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DontTouchKeyboard.UI.Core;

/// <summary>
/// 按键尺寸
/// </summary>
public enum SizeType
{
    /// <summary>
    /// 0.5 Default
    /// </summary>
    Mini,
    /// <summary>
    /// 1 Default
    /// </summary>
    Default,
    /// <summary>
    /// 1.5 Default
    /// </summary>
    Medium,
    /// <summary>
    /// 2 Default
    /// </summary>
    Large,
    /// <summary>
    /// 2.5 Default
    /// </summary>
    XLarge,
    /// <summary>
    /// 3 Default
    /// </summary>
    XXLarge,
    /// <summary>
    /// 5 Default
    /// </summary>
    XXXLarge,
    /// <summary>
    /// 1 Default &amp; Height
    /// </summary>
    Function,
    /// <summary>
    /// 1.5 Default &amp; 0.8 Height
    /// </summary>
    FunctionMedium,
    /// <summary>
    /// 1 Default &amp; 2 Height
    /// </summary>
    VerticalLarge,
    CapsLock,
}
