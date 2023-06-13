namespace System;

public static class IntExtension
{
    /// <summary>
    /// 从16进制整形数中获取颜色
    /// </summary>
    /// <typeparam name="TColor"></typeparam>
    /// <param name="color"></param>
    /// <param name="parseColor"></param>
    /// <returns></returns>

    public static TColor HexToColor<TColor>(this int color, Func<int, int, int, TColor> parseColor)
        => parseColor((color & 0xff0000) >> 0x10, (color & 0x00ff00) >> 0x08, (color & 0x0000ff) >> 0x00);
}
