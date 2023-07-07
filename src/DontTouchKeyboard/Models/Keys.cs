namespace DontTouchKeyboard.Models;

public enum Keys
{
    /// <summary>
    /// The bitmask to extract modifiers from a key value.
    /// </summary>
    Modifiers = -65536,
    /// <summary>
    /// The bitmask to extract a key code from a key value.
    /// </summary>
    KeyCode = 65535,
    /// <summary>
    /// The SHIFT modifier key.
    /// </summary>
    ShiftModifier = 65536,
    /// <summary>
    /// The CTRL modifier key.
    /// </summary>
    ControlModifier = 131072,
    /// <summary>
    /// The ALT modifier key.
    /// </summary>
    AltModifier = 262144,

    /// <summary>
    /// No virtual key value.
    /// </summary>
    None = 0,
    /// <summary>
    /// The left mouse button.
    /// </summary>
    LeftButton = 1,
    /// <summary>
    /// The right mouse button.
    /// </summary>
    RightButton = 2,
    /// <summary>
    /// The cancel key or button
    /// </summary>
    Cancel = 3,
    /// <summary>
    /// The middle mouse button.
    /// </summary>
    MiddleButton = 4,
    /// <summary>
    /// An additional "extended" device key or button (for example, an additional mouse
    /// button).
    /// </summary>
    XButton1 = 5,
    /// <summary>
    /// An additional "extended" device key or button (for example, an additional mouse
    /// button).
    /// </summary>
    XButton2 = 6,
    /// <summary>
    /// The virtual back key or button.
    /// </summary>
    Back = 8,
    /// <summary>
    /// The Tab key.
    /// </summary>
    Tab = 9,
    /// <summary>
    /// The LINEFEED key.
    /// </summary>
    LineFeed = 10,
    /// <summary>
    /// The Clear key or button.
    /// </summary>
    Clear = 12,
    /// <summary>
    /// The Enter key.
    /// </summary>
    Enter = 13,
    /// <summary>
    /// The Shift key. This is the general Shift case, applicable to key layouts with
    /// only one Shift key or that do not need to differentiate between left Shift and
    /// right Shift keystrokes.
    /// </summary>
    Shift = 0x10,
    /// <summary>
    /// The Ctrl key. This is the general Ctrl case, applicable to key layouts with only
    /// one Ctrl key or that do not need to differentiate between left Ctrl and right
    /// Ctrl keystrokes.
    /// </summary>
    Control = 17,
    /// <summary>
    /// The menu key or button.
    /// </summary>
    Menu = 18,
    /// <summary>
    /// The Pause key or button.
    /// </summary>
    Pause = 19,
    /// <summary>
    /// The Caps Lock key or button.
    /// </summary>
    CapitalLock = 20,
    /// <summary>
    /// The Kana symbol key-shift button
    /// </summary>
    Kana = 21,
    ImeOn = 22,
    /// <summary>
    /// The Junja symbol key-shift button.
    /// </summary>
    Junja = 23,
    /// <summary>
    /// The Final symbol key-shift button.
    /// </summary>
    Final = 24,
    /// <summary>
    /// The Hanja symbol key shift button.
    /// </summary>
    Hanja = 25,
    ImeOff = 26,
    /// <summary>
    /// The Esc key.
    /// </summary>
    Escape = 27,
    /// <summary>
    /// The convert button or key.
    /// </summary>
    Convert = 28,
    /// <summary>
    /// The nonconvert button or key.
    /// </summary>
    NonConvert = 29,
    /// <summary>
    /// The accept button or key.
    /// </summary>
    Accept = 30,
    /// <summary>
    /// The mode change key.
    /// </summary>
    ModeChange = 0x1F,
    /// <summary>
    /// The Spacebar key or button.
    /// </summary>
    Space = 0x20,
    /// <summary>
    /// The Page Up key.
    /// </summary>
    PageUp = 33,
    /// <summary>
    /// The Page Down key.
    /// </summary>
    PageDown = 34,
    /// <summary>
    /// The End key.
    /// </summary>
    End = 35,
    /// <summary>
    /// The Home key.
    /// </summary>
    Home = 36,
    /// <summary>
    /// The Left Arrow key.
    /// </summary>
    Left = 37,
    /// <summary>
    /// The Up Arrow key.
    /// </summary>
    Up = 38,
    /// <summary>
    /// The Right Arrow key.
    /// </summary>
    Right = 39,
    /// <summary>
    /// The Down Arrow key.
    /// </summary>
    Down = 40,
    /// <summary>
    /// The Select key or button.
    /// </summary>
    Select = 41,
    /// <summary>
    /// The Print key or button.
    /// </summary>
    Print = 42,
    /// <summary>
    /// The execute key or button.
    /// </summary>
    Execute = 43,
    /// <summary>
    /// The snapshot key or button.
    /// </summary>
    Snapshot = 44,
    /// <summary>
    /// The Insert key.
    /// </summary>
    Insert = 45,
    /// <summary>
    /// The Delete key.
    /// </summary>
    Delete = 46,
    /// <summary>
    /// The Help key or button.
    /// </summary>
    Help = 47,
    /// <summary>
    /// The number "0" key.
    /// </summary>
    Number0 = 48,
    /// <summary>
    /// The number "1" key.
    /// </summary>
    Number1 = 49,
    /// <summary>
    /// The number "2" key.
    /// </summary>
    Number2 = 50,
    /// <summary>
    /// The number "3" key.
    /// </summary>
    Number3 = 51,
    /// <summary>
    /// The number "4" key.
    /// </summary>
    Number4 = 52,
    /// <summary>
    /// The number "5" key.
    /// </summary>
    Number5 = 53,
    /// <summary>
    /// The number "6" key.
    /// </summary>
    Number6 = 54,
    /// <summary>
    /// The number "7" key.
    /// </summary>
    Number7 = 55,
    /// <summary>
    /// The number "8" key.
    /// </summary>
    Number8 = 56,
    /// <summary>
    /// The number "9" key.
    /// </summary>
    Number9 = 57,
    /// <summary>
    /// The letter "A" key.
    /// </summary>
    A = 65,
    /// <summary>
    /// The letter "B" key.
    /// </summary>
    B = 66,
    /// <summary>
    /// The letter "C" key.
    /// </summary>
    C = 67,
    /// <summary>
    /// The letter "D" key.
    /// </summary>
    D = 68,
    /// <summary>
    /// The letter "E" key.
    /// </summary>
    E = 69,
    /// <summary>
    /// The letter "F" key.
    /// </summary>
    F = 70,
    /// <summary>
    /// The letter "G" key.
    /// </summary>
    G = 71,
    /// <summary>
    /// The letter "H" key.
    /// </summary>
    H = 72,
    /// <summary>
    /// The letter "I" key.
    /// </summary>
    I = 73,
    /// <summary>
    /// The letter "J" key.
    /// </summary>
    J = 74,
    /// <summary>
    /// The letter "K" key.
    /// </summary>
    K = 75,
    /// <summary>
    /// The letter "L" key.
    /// </summary>
    L = 76,
    /// <summary>
    /// The letter "M" key.
    /// </summary>
    M = 77,
    /// <summary>
    /// The letter "N" key.
    /// </summary>
    N = 78,
    /// <summary>
    /// The letter "O" key.
    /// </summary>
    O = 79,
    /// <summary>
    /// The letter "P" key.
    /// </summary>
    P = 80,
    /// <summary>
    /// The letter "Q" key.
    /// </summary>
    Q = 81,
    /// <summary>
    /// The letter "R" key.
    /// </summary>
    R = 82,
    /// <summary>
    /// The letter "S" key.
    /// </summary>
    S = 83,
    /// <summary>
    /// The letter "T" key.
    /// </summary>
    T = 84,
    /// <summary>
    /// The letter "U" key.
    /// </summary>
    U = 85,
    /// <summary>
    /// The letter "V" key.
    /// </summary>
    V = 86,
    /// <summary>
    /// The letter "W" key.
    /// </summary>
    W = 87,
    /// <summary>
    /// The letter "X" key.
    /// </summary>
    X = 88,
    /// <summary>
    /// The letter "Y" key.
    /// </summary>
    Y = 89,
    /// <summary>
    /// The letter "Z" key.
    /// </summary>
    Z = 90,
    /// <summary>
    /// The left Windows key.
    /// </summary>
    LeftWindows = 91,
    /// <summary>
    /// The right Windows key.
    /// </summary>
    RightWindows = 92,
    /// <summary>
    /// The application key or button.
    /// </summary>
    Application = 93,
    /// <summary>
    /// The sleep key or button.
    /// </summary>
    Sleep = 95,
    /// <summary>
    /// The number "0" key as located on a numeric pad.
    /// </summary>
    NumberPad0 = 96,
    /// <summary>
    /// The number "1" key as located on a numeric pad.
    /// </summary>
    NumberPad1 = 97,
    /// <summary>
    /// The number "2" key as located on a numeric pad.
    /// </summary>
    NumberPad2 = 98,
    /// <summary>
    /// The number "3" key as located on a numeric pad.
    /// </summary>
    NumberPad3 = 99,
    /// <summary>
    /// The number "4" key as located on a numeric pad.
    /// </summary>
    NumberPad4 = 100,
    /// <summary>
    /// The number "5" key as located on a numeric pad.
    /// </summary>
    NumberPad5 = 101,
    /// <summary>
    /// The number "6" key as located on a numeric pad.
    /// </summary>
    NumberPad6 = 102,
    /// <summary>
    /// The number "7" key as located on a numeric pad.
    /// </summary>
    NumberPad7 = 103,
    /// <summary>
    /// The number "8" key as located on a numeric pad.
    /// </summary>
    NumberPad8 = 104,
    /// <summary>
    /// The number "9" key as located on a numeric pad.
    /// </summary>
    NumberPad9 = 105,
    /// <summary>
    /// The multiply (*) operation key as located on a numeric pad.
    /// </summary>
    Multiply = 106,
    /// <summary>
    /// The add (+) operation key as located on a numeric pad.
    /// </summary>
    Add = 107,
    /// <summary>
    /// The separator key as located on a numeric pad.
    /// </summary>
    Separator = 108,
    /// <summary>
    /// The subtract (-) operation key as located on a numeric pad.
    /// </summary>
    Subtract = 109,
    /// <summary>
    /// The decimal (.) key as located on a numeric pad.
    /// </summary>
    Decimal = 110,
    /// <summary>
    /// The divide (/) operation key as located on a numeric pad.
    /// </summary>
    Divide = 111,
    /// <summary>
    /// The F1 function key.
    /// </summary>
    F1 = 112,
    /// <summary>
    /// The F2 function key.
    /// </summary>
    F2 = 113,
    /// <summary>
    /// The F3 function key.
    /// </summary>
    F3 = 114,
    /// <summary>
    /// The F4 function key.
    /// </summary>
    F4 = 115,
    /// <summary>
    /// The F5 function key.
    /// </summary>
    F5 = 116,
    /// <summary>
    /// The F6 function key.
    /// </summary>
    F6 = 117,
    /// <summary>
    /// The F7 function key.
    /// </summary>
    F7 = 118,
    /// <summary>
    /// The F8 function key.
    /// </summary>
    F8 = 119,
    /// <summary>
    /// The F9 function key.
    /// </summary>
    F9 = 120,
    /// <summary>
    /// The F10 function key.
    /// </summary>
    F10 = 121,
    /// <summary>
    /// The F11 function key.
    /// </summary>
    F11 = 122,
    /// <summary>
    /// The F12 function key.
    /// </summary>
    F12 = 123,
    /// <summary>
    /// The F13 function key.
    /// </summary>
    F13 = 124,
    /// <summary>
    /// The F14 function key.
    /// </summary>
    F14 = 125,
    /// <summary>
    /// The F15 function key.
    /// </summary>
    F15 = 126,
    /// <summary>
    /// The F16 function key.
    /// </summary>
    F16 = 0x7F,
    /// <summary>
    /// The F17 function key.
    /// </summary>
    F17 = 0x80,
    /// <summary>
    /// The F18 function key.
    /// </summary>
    F18 = 129,
    /// <summary>
    /// The F19 function key.
    /// </summary>
    F19 = 130,
    /// <summary>
    /// The F20 function key.
    /// </summary>
    F20 = 131,
    /// <summary>
    /// The F21 function key.
    /// </summary>
    F21 = 132,
    /// <summary>
    /// The F22 function key.
    /// </summary>
    F22 = 133,
    /// <summary>
    /// The F23 function key.
    /// </summary>
    F23 = 134,
    /// <summary>
    /// The F24 function key.
    /// </summary>
    F24 = 135,
    /// <summary>
    /// The navigation up button.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    NavigationView = 136,
    /// <summary>
    /// The navigation menu button.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    NavigationMenu = 137,
    /// <summary>
    /// The navigation up button.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    NavigationUp = 138,
    /// <summary>
    /// The navigation down button.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    NavigationDown = 139,
    /// <summary>
    /// The navigation left button.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    NavigationLeft = 140,
    /// <summary>
    /// The navigation right button.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    NavigationRight = 141,
    /// <summary>
    /// The navigation accept button.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    NavigationAccept = 142,
    /// <summary>
    /// The navigation cancel button.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    NavigationCancel = 143,
    /// <summary>
    /// The Num Lock key.
    /// </summary>
    NumberKeyLock = 144,
    /// <summary>
    /// The Scroll Lock (ScrLk) key.
    /// </summary>
    Scroll = 145,
    /// <summary>
    /// The left Shift key.
    /// </summary>
    LeftShift = 160,
    /// <summary>
    /// The right Shift key.
    /// </summary>
    RightShift = 161,
    /// <summary>
    /// The left Ctrl key.
    /// </summary>
    LeftControl = 162,
    /// <summary>
    /// The right Ctrl key.
    /// </summary>
    RightControl = 163,
    /// <summary>
    /// The left menu key.
    /// </summary>
    LeftMenu = 164,
    /// <summary>
    /// The right menu key.
    /// </summary>
    RightMenu = 165,
    /// <summary>
    /// The go back key.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GoBack = 166,
    /// <summary>
    /// The go forward key.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GoForward = 167,
    /// <summary>
    /// The refresh key.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    Refresh = 168,
    /// <summary>
    /// The stop key.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    Stop = 169,
    /// <summary>
    /// The search key.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    Search = 170,
    /// <summary>
    /// The favorites key.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    Favorites = 171,
    /// <summary>
    /// The go home key.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GoHome = 172,

    /// <summary>
    /// The volume mute key.
    /// </summary>
    VolumeMute = 173,
    /// <summary>
    /// The volume down key.
    /// </summary>
    VolumeDown = 174,
    /// <summary>
    /// The volume up key.
    /// </summary>
    VolumeUp = 175,
    /// <summary>
    /// The media next track key.
    /// </summary>
    MediaNextTrack = 176,
    /// <summary>
    /// The media previous track key.
    /// </summary>
    MediaPreviousTrack = 177,
    /// <summary>
    /// The media Stop key.
    /// </summary>
    MediaStop = 178,
    /// <summary>
    /// The media play pause key.
    /// </summary>
    MediaPlayPause = 179,
    /// <summary>
    /// The launch mail key.
    /// </summary>
    LaunchMail = 180,
    /// <summary>
    /// The select media key.
    /// </summary>
    SelectMedia = 181,
    /// <summary>
    /// The start application one key.
    /// </summary>
    LaunchApplication1 = 182,
    /// <summary>
    /// The start application two key.
    /// </summary>
    LaunchApplication2 = 183,
    /// <summary>
    /// The OEM Semicolon key on a US standard keyboard.
    /// </summary>
    OemSemicolon = 186,
    /// <summary>
    /// The OEM plus key on any country/region keyboard.
    /// </summary>
    OemPlus = 187,
    /// <summary>
    /// The OEM comma key on any country/region keyboard.
    /// </summary>
    OemComma = 188,
    /// <summary>
    /// The OEM minus key on any country/region keyboard.
    /// </summary>
    OemMinus = 189,
    /// <summary>
    /// The OEM period key on any country/region keyboard.
    /// </summary>
    OemPeriod = 190,
    /// <summary>
    /// The OEM question mark key on a US standard keyboard.
    /// </summary>
    OemQuestion = 191,
    /// <summary>
    /// The OEM tilde key on a US standard keyboard.
    /// </summary>
    OemTilde = 192,
    /// <summary>
    /// The gamepad A button.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadA = 195,
    /// <summary>
    /// The gamepad B button.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadB = 196,
    /// <summary>
    /// The gamepad X button.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadX = 197,
    /// <summary>
    /// The gamepad Y button.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadY = 198,
    /// <summary>
    /// The gamepad right shoulder.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadRightShoulder = 199,
    /// <summary>
    /// The gamepad left shoulder.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadLeftShoulder = 200,
    /// <summary>
    /// The gamepad left trigger.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadLeftTrigger = 201,
    /// <summary>
    /// The gamepad right trigger.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadRightTrigger = 202,
    /// <summary>
    /// The gamepad d-pad up.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadDPadUp = 203,
    /// <summary>
    /// The gamepad d-pad down.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadDPadDown = 204,
    /// <summary>
    /// The gamepad d-pad left.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadDPadLeft = 205,
    /// <summary>
    /// The gamepad d-pad right.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadDPadRight = 206,
    /// <summary>
    /// The gamepad menu button.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadMenu = 207,
    /// <summary>
    /// The gamepad view button.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadView = 208,
    /// <summary>
    /// The gamepad left thumbstick button.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadLeftThumbstickButton = 209,
    /// <summary>
    /// The gamepad right thumbstick button.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadRightThumbstickButton = 210,
    /// <summary>
    /// The gamepad left thumbstick up.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadLeftThumbstickUp = 211,
    /// <summary>
    /// The gamepad left thumbstick down.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadLeftThumbstickDown = 212,
    /// <summary>
    /// The gamepad left thumbstick right.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadLeftThumbstickRight = 213,
    /// <summary>
    /// The gamepad left thumbstick left.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadLeftThumbstickLeft = 214,
    /// <summary>
    /// The gamepad right thumbstick up.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadRightThumbstickUp = 215,
    /// <summary>
    /// The gamepad right thumbstick down.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadRightThumbstickDown = 216,
    /// <summary>
    /// The gamepad right thumbstick right.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadRightThumbstickRight = 217,
    /// <summary>
    /// The gamepad right thumbstick left.
    [SupportedOSPlatform("Windows10.0.10240.0")]
    GamepadRightThumbstickLeft = 218,
    /// <summary>
    /// The OEM open bracket key on a US standard keyboard.
    /// </summary>
    OemOpenBrackets = 219,
    /// <summary>
    /// The OEM pipe key on a US standard keyboard.
    /// </summary>
    OemPipe = 220,
    /// <summary>
    /// The OEM close bracket key on a US standard keyboard.
    /// </summary>
    OemCloseBrackets = 221,
    /// <summary>
    /// The OEM singled/double quote key on a US standard keyboard.
    /// </summary>
    OemQuotes = 222,
    /// <summary>
    /// The OEM 8 key.
    /// </summary>
    Oem8 = 223,
    /// <summary>
    /// The OEM angle bracket or backslash key on the RT 102 key keyboard.
    /// </summary>
    OemBackslash = 226,
    /// <summary>
    /// The PROCESS KEY key.
    /// </summary>
    ProcessKey = 229,
    /// <summary>
    /// Used to pass Unicode characters as if they were keystrokes. The Packet key value
    /// is the low word of a 32-bit virtual-key value used for non-keyboard input methods.
    /// </summary>
    Packet = 231,
    /// <summary>
    /// The ATTN key.
    /// </summary>
    Attn = 246,
    /// <summary>
    /// The CRSEL key.
    /// </summary>
    Crsel = 247,
    /// <summary>
    /// The EXSEL key.
    /// </summary>
    Exsel = 248,
    /// <summary>
    /// The ERASE EOF key.
    /// </summary>
    EraseEof = 249,
    /// <summary>
    /// The PLAY key.
    /// </summary>
    Play = 250,
    /// <summary>
    /// The ZOOM key.
    /// </summary>
    Zoom = 251,
    /// <summary>
    /// A constant reserved for future use.
    /// </summary>
    NoName = 252,
    /// <summary>
    /// The PA1 key.
    /// </summary>
    Pa1 = 253,
    /// <summary>
    /// The CLEAR key.
    /// </summary>
    OemClear = 254,

}


public static class KeysExtension
{
    public static string? ToVisual(this Keys key, KeyStates states) => key switch
    {
        Keys.None => null,
        Keys.LeftButton => null,
        Keys.RightButton => null,
        Keys.Cancel => null,
        Keys.MiddleButton => null,
        Keys.XButton1 => null,
        Keys.XButton2 => null,
        Keys.Clear => null,
        Keys.Back => null,
        Keys.Tab => "Tab",
        Keys.Enter => "Enter",
        Keys.Shift => "Shift",
        Keys.Control => "Ctrl",
        Keys.Menu => "Alt",
        Keys.Pause => "Pause",
        Keys.CapitalLock => "Caps Lock",
        Keys.Kana => null,
        Keys.Junja => null,
        Keys.Final => null,
        Keys.Hanja => null,
        Keys.Escape => "Esc",
        Keys.Convert => null,
        Keys.NonConvert => null,
        Keys.Accept => null,
        Keys.ModeChange => null,
        Keys.Space => null,
        Keys.PageUp => "Page\r\nUp",
        Keys.PageDown => "Page\r\nDown",
        Keys.End => "End",
        Keys.Home => "Home",
        Keys.Left => null,
        Keys.Up => null,
        Keys.Right => null,
        Keys.Down => null,
        Keys.Select => null,
        Keys.Print => "PtrSc",
        Keys.Execute => null,
        Keys.Snapshot => null,
        Keys.Insert => "Insert",
        Keys.Delete => "Delete",
        Keys.Help => null,
        Keys.Number1 => states.IsShiftDown() ? "!" : "1",
        Keys.Number2 => states.IsShiftDown() ? "@" : "2",
        Keys.Number3 => states.IsShiftDown() ? "#" : "3",
        Keys.Number4 => states.IsShiftDown() ? "$" : "4",
        Keys.Number5 => states.IsShiftDown() ? "%" : "5",
        Keys.Number6 => states.IsShiftDown() ? "^" : "6",
        Keys.Number7 => states.IsShiftDown() ? "&" : "7",
        Keys.Number8 => states.IsShiftDown() ? "*" : "8",
        Keys.Number9 => states.IsShiftDown() ? "(" : "9",
        Keys.Number0 => states.IsShiftDown() ? ")" : "0",
        Keys.A => states.IsUpper() ? "A" : "a",
        Keys.B => states.IsUpper() ? "B" : "b",
        Keys.C => states.IsUpper() ? "C" : "c",
        Keys.D => states.IsUpper() ? "D" : "d",
        Keys.E => states.IsUpper() ? "E" : "e",
        Keys.F => states.IsUpper() ? "F" : "f",
        Keys.G => states.IsUpper() ? "G" : "g",
        Keys.H => states.IsUpper() ? "H" : "h",
        Keys.I => states.IsUpper() ? "I" : "i",
        Keys.J => states.IsUpper() ? "J" : "j",
        Keys.K => states.IsUpper() ? "K" : "k",
        Keys.L => states.IsUpper() ? "L" : "l",
        Keys.M => states.IsUpper() ? "M" : "m",
        Keys.N => states.IsUpper() ? "N" : "n",
        Keys.O => states.IsUpper() ? "O" : "o",
        Keys.P => states.IsUpper() ? "P" : "p",
        Keys.Q => states.IsUpper() ? "Q" : "q",
        Keys.R => states.IsUpper() ? "R" : "r",
        Keys.S => states.IsUpper() ? "S" : "s",
        Keys.T => states.IsUpper() ? "T" : "t",
        Keys.U => states.IsUpper() ? "U" : "u",
        Keys.V => states.IsUpper() ? "V" : "v",
        Keys.W => states.IsUpper() ? "W" : "w",
        Keys.X => states.IsUpper() ? "X" : "x",
        Keys.Y => states.IsUpper() ? "Y" : "y",
        Keys.Z => states.IsUpper() ? "Z" : "z",
        Keys.LeftWindows => null,
        Keys.RightWindows => null,
        Keys.Application => null,
        Keys.Sleep => null,
        Keys.NumberPad0 => "0",
        Keys.NumberPad1 => "1",
        Keys.NumberPad2 => "2",
        Keys.NumberPad3 => "3",
        Keys.NumberPad4 => "4",
        Keys.NumberPad5 => "5",
        Keys.NumberPad6 => "6",
        Keys.NumberPad7 => "7",
        Keys.NumberPad8 => "8",
        Keys.NumberPad9 => "9",
        Keys.Multiply => "*",
        Keys.Add => "+",
        Keys.Separator => null,
        Keys.Subtract => "-",
        Keys.Decimal => ".",
        Keys.Divide => "/",
        Keys.F1 => "F1",
        Keys.F2 => "F2",
        Keys.F3 => "F3",
        Keys.F4 => "F4",
        Keys.F5 => "F5",
        Keys.F6 => "F6",
        Keys.F7 => "F7",
        Keys.F8 => "F8",
        Keys.F9 => "F9",
        Keys.F10 => "F10",
        Keys.F11 => "F11",
        Keys.F12 => "F12",
        Keys.F13 => "F13",
        Keys.F14 => "F14",
        Keys.F15 => "F15",
        Keys.F16 => "F16",
        Keys.F17 => "F17",
        Keys.F18 => "F18",
        Keys.F19 => "F19",
        Keys.F20 => "F20",
        Keys.F21 => "F21",
        Keys.F22 => "F22",
        Keys.F23 => "F23",
        Keys.F24 => "F24",
        Keys.NumberKeyLock => "Num\r\nLock",
        Keys.Scroll => "Scroll",
        Keys.LeftShift => "Shift",
        Keys.RightShift => "Shift",
        Keys.LeftControl => "Ctrl",
        Keys.RightControl => "Ctrl",
        Keys.LeftMenu => "Alt",
        Keys.RightMenu => "Alt",
        Keys.OemSemicolon => states.IsShiftDown() ? ":" : ";",
        Keys.OemPlus => states.IsShiftDown() ? "+" : "=",
        Keys.OemComma => states.IsShiftDown() ? "<" : ",",
        Keys.OemMinus => states.IsShiftDown() ? "_" : "-",
        Keys.OemPeriod => states.IsShiftDown() ? ">" : ".",
        Keys.OemQuestion => states.IsShiftDown() ? "?" : "/",
        Keys.OemTilde => states.IsShiftDown() ? "~" : "`",
        Keys.OemOpenBrackets => states.IsShiftDown() ? "{" : "[",
        Keys.OemPipe => states.IsShiftDown() ? "|" : "\\",
        Keys.OemCloseBrackets => states.IsShiftDown() ? "}" : "]",
        Keys.OemQuotes => states.IsShiftDown() ? "\"" : "'",
        _ => null,
    };
}

