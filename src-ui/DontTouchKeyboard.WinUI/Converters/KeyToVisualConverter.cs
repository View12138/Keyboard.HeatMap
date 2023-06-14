using DontTouchKeyboard.Core.Internals;

namespace DontTouchKeyboard.UI.Converters;

public class KeyToVisualConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is Keys key)
        {
            return key.ToVisual(KeyStatesFactory.KeyStates);
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return ThrowHelper.ThrowNotSupportedException<object>("不支持的转换");
    }


}
