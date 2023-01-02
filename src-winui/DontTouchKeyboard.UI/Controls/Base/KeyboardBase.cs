namespace DontTouchKeyboard.UI.Controls.Base;

public class KeyboardBase : UserControl
{
    public KeyboardBase()
    {
        UpdateStates();

        App.StatusChanged += (s, e) => UpdateStates();
    }

    private void UpdateStates()
    {
        CapitalLockState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.CapitalLock);
        NumberKeyLockState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.NumberKeyLock);
        InsertState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Insert);
        ScrollState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Scroll);
        ShiftState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Shift);
        ControlState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control);
    }


    /// <summary>
    /// 获取或设置 CapsLock 键的状态
    /// </summary>
    public CoreVirtualKeyStates CapitalLockState
    {
        get { return (CoreVirtualKeyStates)GetValue(CapitalLockStateProperty); }
        set { SetValue(CapitalLockStateProperty, value); }
    }
    // Using a DependencyProperty as the backing store for CapsLock.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CapitalLockStateProperty =
        DependencyProperty.Register(nameof(CapitalLockState), typeof(CoreVirtualKeyStates), typeof(KeyboardBase), new PropertyMetadata(CoreVirtualKeyStates.None));

    /// <summary>
    /// 获取或设置 NumLock 键的状态
    /// </summary>
    public CoreVirtualKeyStates NumberKeyLockState
    {
        get { return (CoreVirtualKeyStates)GetValue(NumberKeyLockStateProperty); }
        set { SetValue(NumberKeyLockStateProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CapsLock.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty NumberKeyLockStateProperty =
        DependencyProperty.Register(nameof(NumberKeyLockState), typeof(CoreVirtualKeyStates), typeof(KeyboardBase), new PropertyMetadata(CoreVirtualKeyStates.None));

    /// <summary>
    /// 获取或设置 Insert 键的状态
    /// </summary>
    public CoreVirtualKeyStates InsertState
    {
        get { return (CoreVirtualKeyStates)GetValue(InsertStateProperty); }
        set { SetValue(InsertStateProperty, value); }
    }
    // Using a DependencyProperty as the backing store for CapsLock.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty InsertStateProperty =
        DependencyProperty.Register(nameof(InsertState), typeof(CoreVirtualKeyStates), typeof(KeyboardBase), new PropertyMetadata(CoreVirtualKeyStates.None));

    /// <summary>
    /// 获取或设置 Scroll 键的状态
    /// </summary>
    public CoreVirtualKeyStates ScrollState
    {
        get { return (CoreVirtualKeyStates)GetValue(ScrollStateProperty); }
        set { SetValue(ScrollStateProperty, value); }
    }
    // Using a DependencyProperty as the backing store for CapsLock.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ScrollStateProperty =
        DependencyProperty.Register(nameof(ScrollState), typeof(CoreVirtualKeyStates), typeof(KeyboardBase), new PropertyMetadata(CoreVirtualKeyStates.None));

    /// <summary>
    /// 获取或设置 Shift 键的状态
    /// </summary>
    public CoreVirtualKeyStates ShiftState
    {
        get { return (CoreVirtualKeyStates)GetValue(ShiftStateProperty); }
        set { SetValue(ShiftStateProperty, value); }
    }
    // Using a DependencyProperty as the backing store for Shift.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ShiftStateProperty =
        DependencyProperty.Register(nameof(ShiftState), typeof(CoreVirtualKeyStates), typeof(KeyboardBase), new PropertyMetadata(CoreVirtualKeyStates.None));

    /// <summary>
    /// 获取或设置 Control 键的状态
    /// </summary>
    public CoreVirtualKeyStates ControlState
    {
        get { return (CoreVirtualKeyStates)GetValue(ControlStateProperty); }
        set { SetValue(ControlStateProperty, value); }
    }
    // Using a DependencyProperty as the backing store for Shift.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ControlStateProperty =
        DependencyProperty.Register(nameof(ControlState), typeof(CoreVirtualKeyStates), typeof(KeyboardBase), new PropertyMetadata(CoreVirtualKeyStates.None));


    private IEnumerable<KeyButton> keys;
    /// <summary>
    /// 获取当前键盘上的所有按键
    /// </summary>
    public IEnumerable<KeyButton> Keys => keys ??= GetKeys(Content);

    private IEnumerable<KeyButton> GetKeys(UIElement element)
    {
        if (element is Panel panel)
        {
            foreach (var child in panel.Children)
            {
                foreach (var item in GetKeys(child))
                {
                    yield return item;
                }
            }
        }
        else if (element is KeyButton button)
        {
            yield return button;
        }
    }

}
