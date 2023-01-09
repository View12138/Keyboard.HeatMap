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


internal class IndexToVisibility : DependencyObject, IValueConverter
{

    /// <summary>
    /// 获取或设置转换为 <see cref="Visibility.Visible"/> 的值
    /// </summary>
    public int VisibleValue
    {
        get { return (int)GetValue(VisibiltyValueProperty); }
        set { SetValue(VisibiltyValueProperty, value); }
    }
    // Using a DependencyProperty as the backing store for VisibiltyValue.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty VisibiltyValueProperty =
        DependencyProperty.Register(nameof(VisibleValue), typeof(int), typeof(IndexToVisibility), new PropertyMetadata(0));

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is int index)
        {
            if (index == VisibleValue)
            {
                return Visibility.Visible;
            }
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

internal class IndexToBool : DependencyObject, IValueConverter
{

    /// <summary>
    /// 获取或设置转换为 <see cref="true"/> 的值
    /// </summary>
    public int TrueValue
    {
        get { return (int)GetValue(TrueValueProperty); }
        set { SetValue(TrueValueProperty, value); }
    }
    // Using a DependencyProperty as the backing store for TrueValue.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TrueValueProperty =
        DependencyProperty.Register(nameof(TrueValue), typeof(int), typeof(IndexToBool), new PropertyMetadata(0));


    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is int index)
        {
            if (index == TrueValue)
            {
                return true;
            }
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}