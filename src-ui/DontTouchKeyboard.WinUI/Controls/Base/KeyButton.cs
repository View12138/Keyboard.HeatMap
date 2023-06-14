using DontTouchKeyboard.Core;
using DontTouchKeyboard.Core.Internals;
using DontTouchKeyboard.Core.Models;

namespace DontTouchKeyboard.UI.Controls.Base;

[ContentProperty(Name = nameof(Content))]
public class KeyButton : ContentControl, IKeyButton
{
    public KeyButton()
    {
        DefaultStyleKey = typeof(KeyButton);
        Style = GetSizeStyle(Size);
        App.StatusChanged += (s, e) =>
        {
            KeyChanged(this, Key);
        };
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
    }

    public int KeyCode { get => (int)GetValue(KeyProperty); set => SetValue(KeyProperty, (Keys)value); }
    public Keys Key { get => (Keys)GetValue(KeyProperty); set => SetValue(KeyProperty, value); }
    public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(nameof(Key), typeof(Keys), typeof(KeyButton), new PropertyMetadata(Keys.None, (s, e) => KeyChanged(s, (Keys)e.NewValue)));

    public VisibleType LockVisibility { get => (VisibleType)GetValue(LockVisibilityProperty); set => SetValue(LockVisibilityProperty, value); }
    public static readonly DependencyProperty LockVisibilityProperty = DependencyProperty.Register(nameof(LockVisibility), typeof(VisibleType), typeof(KeyButton), new PropertyMetadata(VisibleType.Collapsed));

    public VisibleType LocationVisibility { get => (VisibleType)GetValue(LocationVisibilityProperty); set => SetValue(LocationVisibilityProperty, value); }
    public static readonly DependencyProperty LocationVisibilityProperty = DependencyProperty.Register(nameof(LocationVisibility), typeof(VisibleType), typeof(KeyButton), new PropertyMetadata(VisibleType.Collapsed));

    public SizeType Size { get => (SizeType)GetValue(SizeProperty); set => SetValue(SizeProperty, value); }
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(nameof(Size), typeof(SizeType), typeof(KeyButton), new PropertyMetadata(SizeType.Default, (s, e) => s.SetValue(StyleProperty, GetSizeStyle((SizeType)e.NewValue))));

    private static Style GetSizeStyle(SizeType size = SizeType.Default) => App.GetRequiredResource<Style>($"{size}KeyStyle");

    private static void KeyChanged(DependencyObject s, Keys currentKey)
    {
        string? visual = currentKey.ToVisual(KeyStatesFactory.KeyStates);
        if (visual != null)
        {
            s.SetValue(ContentProperty, visual);
        }
    }

    public override string ToString()
    {
        return $"{Key}:{Key.ToVisual(KeyStatesFactory.KeyStates)}";
    }

}
