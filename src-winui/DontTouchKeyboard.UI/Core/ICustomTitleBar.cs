using Windows.Graphics;

namespace DontTouchKeyboard.UI.Core;

public interface ICustomTitleBar
{
    public FrameworkElement GetAppTitleBar();
    public List<RectInt32> GetDragRects(Window window, double scaleAdjustment);
}
