namespace DontTouchKeyboard.WinUI.Converters;

public class VisibleTypeToVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is VisibleType visible)
        {
            return visible == VisibleType.Visible ? Visibility.Visible : Visibility.Collapsed;
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return ThrowHelper.ThrowNotSupportedException<object>("不支持的转换");
    }
}