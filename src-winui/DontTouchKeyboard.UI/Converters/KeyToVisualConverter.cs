namespace DontTouchKeyboard.UI.Converters;

public class KeyToVisualConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var status = parameter as KeyStates;
        if (value is VirtualKey key)
        {
            return key.ToVisual(status);
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException("不支持的转换");
    }


}
