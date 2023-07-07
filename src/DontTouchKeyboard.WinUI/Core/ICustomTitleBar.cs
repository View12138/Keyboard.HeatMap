namespace DontTouchKeyboard.WinUI.Core;

/// <summary>
/// 自定义标题栏
/// </summary>
public interface ICustomTitleBar
{
    /// <summary>
    /// 获取自定义标题栏
    /// </summary>
    /// <returns>自定义标题栏元素</returns>
    public FrameworkElement GetAppTitleBar();

    /// <summary>
    /// 获取可以拖动区域
    /// </summary>
    /// <param name="window">设置自定义标题栏的窗口</param>
    /// <param name="scaleAdjustment">缩放比例尺</param>
    /// <returns></returns>
    public List<RectInt32> GetDragRects(Window window, double scaleAdjustment);
}
