using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics;

namespace DontTouchKeyboard.UI.Core;

public interface ICustomTitleBar
{
    public FrameworkElement GetAppTitleBar();
    public List<RectInt32> GetDragRects(Window window);
}
