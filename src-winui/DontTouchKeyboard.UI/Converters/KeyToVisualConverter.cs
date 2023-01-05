using DontTouchKeyboard.UI.Core.Extensions;

namespace DontTouchKeyboard.UI.Converters;

public class KeyToVisualConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is VirtualKey key)
        {
            return key.ToVisual();
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException("不支持的转换");
    }


}
