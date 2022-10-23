
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DontTouchKeyboard.UI.Controls.Base;

[ContentProperty(Name = nameof(Content))]
[TemplateVisualState(Name = nameof(KeyType.Single), GroupName = "NormalState")]
[TemplateVisualState(Name = nameof(KeyType.Double), GroupName = "NormalState")]
public class KeyButton : ContentControl
{
    public KeyButton()
    {
        this.DefaultStyleKey = typeof(KeyButton);
        this.Style = GetSizeStyle(Size);
        AppHost.StatusChanged += (s, e) => KeyChanged(this, Key);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
    }


    /// <summary>
    /// 获取或设置按键码
    /// <list type="bullet">
    /// <item><see cref="Key"/> 的 <see cref="int"/> 类型包装</item>
    /// <item>当不是键码预定义的 <see cref="VirtualKey"/> 时，使用 <see cref="KeyCode"/> 代替 <see cref="Key"/></item>
    /// </list>
    /// </summary>
    public int KeyCode { get => (int)GetValue(KeyProperty); set => SetValue(KeyProperty, (VirtualKey)value); }
    /// <summary>
    /// 获取或设置虚拟键码
    /// </summary>
    public VirtualKey Key { get => (VirtualKey)GetValue(KeyProperty); set => SetValue(KeyProperty, value); }
    // Using a DependencyProperty as the backing store for Key.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty KeyProperty =
        DependencyProperty.Register(nameof(Key), typeof(VirtualKey), typeof(KeyButton), new PropertyMetadata(VirtualKey.None,
            (s, e) => KeyChanged(s, (VirtualKey)e.NewValue)));

    /// <summary>
    /// 获取或设置锁定状态的可见性
    /// </summary>
    public Visibility LockVisibility { get => (Visibility)GetValue(LockVisibilityProperty); set => SetValue(LockVisibilityProperty, value); }
    // Using a DependencyProperty as the backing store for IsLocked.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty LockVisibilityProperty =
        DependencyProperty.Register(nameof(LockVisibility), typeof(Visibility), typeof(KeyButton), new PropertyMetadata(Visibility.Collapsed));

    /// <summary>
    /// 获取或设置定位键标识的可见性
    /// </summary>
    public Visibility LocationVisibility { get => (Visibility)GetValue(LocationVisibilityProperty); set => SetValue(LocationVisibilityProperty, value); }
    // Using a DependencyProperty as the backing store for IsLocked.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty LocationVisibilityProperty =
        DependencyProperty.Register(nameof(LocationVisibility), typeof(Visibility), typeof(KeyButton), new PropertyMetadata(Visibility.Collapsed));

    /// <summary>
    /// 按键尺寸
    /// </summary>
    public SizeType Size { get => (SizeType)GetValue(SizeProperty); set => SetValue(SizeProperty, value); }
    // Using a DependencyProperty as the backing store for Size.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SizeProperty =
        DependencyProperty.Register(nameof(Size), typeof(SizeType), typeof(KeyButton), new PropertyMetadata(SizeType.Default,
            (s, e) => s.SetValue(StyleProperty, GetSizeStyle((SizeType)e.NewValue))));


    private static Style GetSizeStyle(SizeType size = SizeType.Default)
        => Application.Current.Resources[$"{size}KeyStyle"] as Style;

    private static void KeyChanged(DependencyObject s, VirtualKey key)
    {
        var visual = key.ToVisual(new KeyStates());
        if (visual != null)
        {
            s.SetValue(ContentProperty, visual);
        }
    }

    public override string ToString()
    {
        return $"{Key}:{Key.ToVisual(KeyStates.Instance)}";
    }

}
