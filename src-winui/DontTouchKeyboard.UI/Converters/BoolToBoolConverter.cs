namespace DontTouchKeyboard.UI.Converters;

internal class BoolToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool val)
        {
            return !val;
        }
        throw new ArgumentException("不是可转换的类型", nameof(value));
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is bool val)
        {
            return !val;
        }
        throw new ArgumentException("不是可转换的类型", nameof(value));
    }
}

internal class SystemInfoToMessage : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool isDetail)
        {
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
