using System.Collections.Generic;
using System.Windows.Media;

namespace Keyboard.HeatMap.Owner
{
    /// <summary>
    /// 20 级热力颜色
    /// </summary>
    public static class HotColors
    {
        /// <summary>
        /// 热力值字典
        /// </summary>
        private static Dictionary<int, int> _colors = new Dictionary<int, int>()
        {
            { 0x01, 0xebe8e7 },
            { 0x02, 0xece0db },
            { 0x03, 0xedd8d0 },
            { 0x04, 0xeecfc4 },
            { 0x05, 0xefc6b8 },
            { 0x06, 0xf0bdac },
            { 0x07, 0xf1b5a0 },
            { 0x08, 0xf2ad95 },
            { 0x09, 0xf3a489 },
            { 0x0a, 0xf49b7d },
            { 0x0b, 0xf59372 },
            { 0x0c, 0xf68a66 },
            { 0x0d, 0xf78259 },
            { 0x0e, 0xf8794d },
            { 0x0f, 0xf97142 },
            { 0x10, 0xfa6836 },
            { 0x11, 0xfb602b },
            { 0x12, 0xfc561d },
            { 0x13, 0xfd4e12 },
            { 0x14, 0xfe4607 },
        };

        /// <summary>
        /// 获取所有热力色，16进制形式
        /// </summary>
        public static Dictionary<int, int> Colors { get => _colors; }

        /// <summary>
        /// 从16进制整形数中获取颜色
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color ColorFromInt(int color)
        {
            byte r = (byte)((color & 0xff0000) >> 0x10);
            byte g = (byte)((color & 0x00ff00) >> 0x08);
            byte b = (byte)((color & 0x0000ff) >> 0x00);
            return Color.FromRgb(r, g, b);
        }
    }
}
