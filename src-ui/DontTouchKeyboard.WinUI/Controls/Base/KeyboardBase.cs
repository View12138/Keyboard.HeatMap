using DontTouchKeyboard.Core;
using DontTouchKeyboard.Core.Models;
using System.Runtime.Versioning;

namespace DontTouchKeyboard.UI.Controls.Base;

public class KeyboardBase : UserControl, IKeyboard
{
    public KeyboardBase()
    {
        UpdateStates();

        App.StatusChanged += (s, e) => UpdateStates();
    }

    private void UpdateStates()
    {
        CapitalLockState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.CapitalLock).ToKeyState();
        NumberKeyLockState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.NumberKeyLock).ToKeyState();
        InsertState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Insert).ToKeyState();
        ScrollState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Scroll).ToKeyState();
        ShiftState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Shift).ToKeyState();
        ControlState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control).ToKeyState();
    }


    public KeyState CapitalLockState { get => (KeyState)GetValue(CapitalLockStateProperty); set => SetValue(CapitalLockStateProperty, value); }
    // Using a DependencyProperty as the backing store for CapsLock.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CapitalLockStateProperty =
        DependencyProperty.Register(nameof(CapitalLockState), typeof(KeyState), typeof(KeyboardBase), new PropertyMetadata(KeyState.None));

    public KeyState NumberKeyLockState { get => (KeyState)GetValue(NumberKeyLockStateProperty); set => SetValue(NumberKeyLockStateProperty, value); }

    // Using a DependencyProperty as the backing store for CapsLock.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty NumberKeyLockStateProperty =
        DependencyProperty.Register(nameof(NumberKeyLockState), typeof(KeyState), typeof(KeyboardBase), new PropertyMetadata(KeyState.None));

    public KeyState InsertState { get => (KeyState)GetValue(InsertStateProperty); set => SetValue(InsertStateProperty, value); }
    // Using a DependencyProperty as the backing store for CapsLock.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty InsertStateProperty =
        DependencyProperty.Register(nameof(InsertState), typeof(KeyState), typeof(KeyboardBase), new PropertyMetadata(KeyState.None));

    public KeyState ScrollState { get => (KeyState)GetValue(ScrollStateProperty); set => SetValue(ScrollStateProperty, value); }
    // Using a DependencyProperty as the backing store for CapsLock.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ScrollStateProperty =
        DependencyProperty.Register(nameof(ScrollState), typeof(KeyState), typeof(KeyboardBase), new PropertyMetadata(KeyState.None));

    public KeyState ShiftState { get => (KeyState)GetValue(ShiftStateProperty); set => SetValue(ShiftStateProperty, value); }
    // Using a DependencyProperty as the backing store for Shift.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ShiftStateProperty =
        DependencyProperty.Register(nameof(ShiftState), typeof(KeyState), typeof(KeyboardBase), new PropertyMetadata(KeyState.None));

    public KeyState ControlState { get => (KeyState)GetValue(ControlStateProperty); set => SetValue(ControlStateProperty, value); }
    // Using a DependencyProperty as the backing store for Shift.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ControlStateProperty =
        DependencyProperty.Register(nameof(ControlState), typeof(KeyState), typeof(KeyboardBase), new PropertyMetadata(KeyState.None));


    private IEnumerable<IKeyButton>? keys;
    public IEnumerable<IKeyButton> Keys => keys ??= GetKeys(Content);

    private IEnumerable<IKeyButton> GetKeys(UIElement element)
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
        else if (element is IKeyButton button)
        {
            yield return button;
        }
    }

}
