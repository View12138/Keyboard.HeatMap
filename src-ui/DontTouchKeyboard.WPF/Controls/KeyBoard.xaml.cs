using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Keyboard.HeatMap.Owner;
using Keys = System.Windows.Forms.Keys;

namespace Keyboard.HeatMap.Controls
{
    /// <summary>
    /// KeyBoard.xaml 的交互逻辑
    /// </summary>
    public partial class KeyBoard : UserControl
    {
        /// <summary>
        /// 可控制按键集合
        /// <para>key 为按键码</para>
        /// </summary>
        private Dictionary<int, Button> Buttons;
        public KeyBoard()
        {
            DataContext = this;
            InitializeComponent();
            Buttons = new Dictionary<int, Button>();
            foreach (StackPanel stack in PrimaryBoardBox.Children)
            {
                foreach (var buttonItem in stack.Children)
                {
                    if (buttonItem is Button button)
                    {
                        if (int.TryParse(button.Tag?.ToString(), out int index))
                        {
                            if (index != 0)
                            { Buttons.Add(index, (Button)buttonItem); }
                        }
                    }
                }
            }
            foreach (var buttonNumpad in NumpadBoardBox.Children)
            {
                if (buttonNumpad is Button button)
                {
                    if (int.TryParse(button.Tag?.ToString(), out int index))
                    {
                        if (index != 0)
                        { Buttons.Add(index, (Button)buttonNumpad); }
                    }
                }
            }
            SetNumpadBoardBoxVisiblity(NumKeyVisibility);
        }

        // dependency property
        /// <summary>
        /// 小键盘可见性
        /// </summary>
        public Visibility NumKeyVisibility
        {
            get { return (Visibility)GetValue(NumKeyVisibilityProperty); }
            set
            { SetValue(NumKeyVisibilityProperty, value); }
        }
        public static readonly DependencyProperty NumKeyVisibilityProperty =
            DependencyProperty.Register(nameof(NumKeyVisibility), typeof(Visibility), typeof(KeyBoard), new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// 第二功能键前景色
        /// </summary>
        public Brush SecondaryForeground
        {
            get { return (Brush)GetValue(SecondForegroundProperty); }
            set { SetValue(SecondForegroundProperty, value); }
        }
        public static readonly DependencyProperty SecondForegroundProperty =
            DependencyProperty.Register(nameof(SecondaryForeground), typeof(Brush), typeof(KeyBoard), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(101, 101, 101))));

        /// <summary>
        /// 主功能键前景色
        /// </summary>
        public Brush PrimaryForeground
        {
            get { return (Brush)GetValue(PrimaryForegroundProperty); }
            set { SetValue(PrimaryForegroundProperty, value); }
        }
        public static readonly DependencyProperty PrimaryForegroundProperty =
            DependencyProperty.Register(nameof(PrimaryForeground), typeof(Brush), typeof(KeyBoard), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        /// <summary>
        /// 设置 - 随 Windows 启动
        /// </summary>
        public string StartWithWindowsTip
        {
            get { return (string)GetValue(StartWithWindowsTipProperty); }
            set { SetValue(StartWithWindowsTipProperty, value); }
        }
        public static readonly DependencyProperty StartWithWindowsTipProperty =
            DependencyProperty.Register(nameof(StartWithWindowsTip), typeof(string), typeof(KeyBoard), new PropertyMetadata("随 Windows 启动"));

        /// <summary>
        /// 设置 - 启动后后台运行
        /// </summary>
        public string BackgroundRunTip
        {
            get { return (string)GetValue(StartBackRunProperty); }
            set { SetValue(StartBackRunProperty, value); }
        }
        public static readonly DependencyProperty StartBackRunProperty =
            DependencyProperty.Register(nameof(BackgroundRunTip), typeof(string), typeof(KeyBoard), new PropertyMetadata("启动后最小化"));

        /// <summary>
        /// 设置 - 自动开始录制
        /// </summary>
        public string AutoPlayTip
        {
            get { return (string)GetValue(AutoPlayTipProperty); }
            set { SetValue(AutoPlayTipProperty, value); }
        }
        public static readonly DependencyProperty AutoPlayTipProperty =
            DependencyProperty.Register(nameof(AutoPlayTip), typeof(string), typeof(KeyBoard), new PropertyMetadata("启动后开始录制"));

        /// <summary>
        /// 设置 - 以管理员身份启动
        /// </summary>
        public string AsAdminTip
        {
            get { return (string)GetValue(AsAdminTipProperty); }
            set { SetValue(AsAdminTipProperty, value); }
        }
        public static readonly DependencyProperty AsAdminTipProperty =
            DependencyProperty.Register(nameof(AsAdminTip), typeof(string), typeof(KeyBoard), new PropertyMetadata("以管理员身份启动"));




        // handle
        /// <summary>
        /// 展开/关闭小键盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            if (NumKeyVisibility == Visibility.Visible)
            {
                SetNumpadBoardBoxVisiblity(Visibility.Collapsed);
            }
            else
            {
                SetNumpadBoardBoxVisiblity(Visibility.Visible);
            }
        }

        // public
        /// <summary>
        /// 设置按键热力值
        /// </summary>
        /// <param name="hotDatas"></param>
        public void SetKeyListHot(Dictionary<Keys, long> hotDatas)
        {
            if (hotDatas.Count <= 0)
            { return; }
            var max = hotDatas.Values.Max();
            int level = (int)(max / 20);
            if (level > 0)
            {// level 代表每 level 次算一个等级，共20个等级。
                foreach (var hotData in hotDatas)
                {
                    int colorIndex = (int)(hotData.Value / (double)level) == 0 ? 1 : (int)(hotData.Value / (double)level);
                    if (colorIndex > 20)
                    { colorIndex = 20; }
                    if (colorIndex <= 0)
                    { continue; }
                    var button = GetButtonByKeys(hotData.Key);
                    if (button == null)
                    { continue; }
                    button.Background = new SolidColorBrush(HotColors.ColorFromInt(HotColors.Colors[colorIndex]));
                    button.ToolTip = $"{hotData.Value} 次击键";
                }
            }
            else
            {// 1 次算一个等级，等级小于20;
                foreach (var record in hotDatas)
                {
                    if (record.Value <= 0)
                    { continue; }
                    var button = GetButtonByKeys(record.Key);
                    if (button == null)
                    { continue; }
                    button.Background = new SolidColorBrush(HotColors.ColorFromInt(HotColors.Colors[(int)record.Value]));
                    button.ToolTip = $"{record.Value} 次击键";
                }
            }
        }
        /// <summary>
        /// 重置热力值
        /// </summary>
        public void ResetKeyBoardHot()
        {
            foreach (var button in Buttons.Values)
            {
                button.Background = Brushes.Transparent;
                button.ToolTip = "尝试在键盘上按下它";
            }
        }
        /// <summary>
        /// 设置设置按钮的状态
        /// </summary>
        /// <param name="asAdmin"></param>
        /// <param name="startWithWindows"></param>
        /// <param name="backgroundRun"></param>
        /// <param name="autoPlay"></param>
        public void SetSettingState(bool asAdmin, bool startWithWindows, bool backgroundRun, bool autoPlay)
        {
            string settingState = asAdmin ? "开启" : "关闭";
            AsAdminTip = $"以管理员身份启动 : {settingState}";
            AsAdmin.IsChecked = asAdmin;
            settingState = startWithWindows ? "开启" : "关闭";
            StartWithWindowsTip = $"随 Windows 启动 : {settingState}";
            StartWithWindows.IsChecked = startWithWindows;
            settingState = backgroundRun ? "开启" : "关闭";
            BackgroundRunTip = $"启动后最小化 : {settingState}";
            BackgroundRun.IsChecked = backgroundRun;
            settingState = autoPlay ? "开启" : "关闭";
            AutoPlayTip = $"启动后开始录制 : {settingState}";
            AutoPlay.IsChecked = autoPlay;
        }
        /// <summary>
        /// 设置某个按键按下
        /// </summary>
        /// <param name="keys"></param>
        public void SetDown(Keys keys)
        {
            var button = GetButtonByKeys(keys);
            if (button != null)
            {
                ThicknessAnimation showClick = new ThicknessAnimation(new Thickness(5), TimeSpan.FromMilliseconds(50));
                button.BeginAnimation(BorderThicknessProperty, showClick);
            }
        }
        /// <summary>
        /// 设置某个按键释放
        /// </summary>
        /// <param name="keys"></param>
        public void SetUp(Keys keys)
        {
            var button = GetButtonByKeys(keys);
            if (button != null)
            {
                ThicknessAnimation hideClick = new ThicknessAnimation(new Thickness(0), TimeSpan.FromMilliseconds(400));
                button.BeginAnimation(BorderThicknessProperty, hideClick);
            }
        }

        // private
        /// <summary>
        /// 获取指定的按键
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        private Button GetButtonByKeys(Keys keys)
        {
            int keyCode = (int)keys;
            if (Buttons.ContainsKey(keyCode))
            { return Buttons[keyCode]; }
            else
            { return null; }
        }
        /// <summary>
        /// 设置小键盘的可见性
        /// </summary>
        /// <param name="visibility"></param>
        private void SetNumpadBoardBoxVisiblity(Visibility visibility)
        {
            if (visibility == Visibility.Visible)
            {
                ThicknessAnimation showNumpad = new ThicknessAnimation(new Thickness(12, 0, 0, 0), TimeSpan.FromMilliseconds(200));
                showNumpad.EasingFunction = new SineEase() { EasingMode = EasingMode.EaseInOut };
                Numpadbox.BeginAnimation(MarginProperty, showNumpad);
                NumKeyVisibility = Visibility.Visible;
            }
            else
            {
                ThicknessAnimation hideNumpad = new ThicknessAnimation(new Thickness(-350, 0, 0, 0), TimeSpan.FromMilliseconds(100));
                hideNumpad.EasingFunction = new SineEase() { EasingMode = EasingMode.EaseInOut };
                Numpadbox.BeginAnimation(MarginProperty, hideNumpad);
                NumKeyVisibility = Visibility.Collapsed;
            }
        }

        // event
        /// <summary>
        /// 表示将用于处理设置项按钮改变状态的事件的方法。
        /// </summary>
        /// <param name="check">设置项按钮状态</param>
        public delegate void SettingEventHandler(bool? check);

        /// <summary>
        /// 小键盘“后台运行”点击事件
        /// </summary>
        public event System.Windows.Forms.KeyEventHandler CloseKeyClick;
        /// <summary>
        /// 以管理员身份启动
        /// </summary>
        public event SettingEventHandler AsAdminChanged;
        /// <summary>
        ///小键盘“随 Windows 启动”改变事件
        /// </summary>
        public event SettingEventHandler StartWithWindowsChanged;
        /// <summary>
        /// 小键盘“启动时后台运行”改变事件
        /// </summary>
        public event SettingEventHandler BackgroundRunChanged;
        /// <summary>
        /// 小键盘“启动后开始录制”事件
        /// </summary>
        public event SettingEventHandler AutoPlayChanged;
        /// <summary>
        /// 小键盘“重新开始录制”点击事件
        /// </summary>
        public event EventHandler ResetClick;

        // notify event
        private void Close_Click(object sender, RoutedEventArgs e) => CloseKeyClick?.Invoke(this, new System.Windows.Forms.KeyEventArgs(Keys.None));
        private void AsAdmin_Click(object sender, RoutedEventArgs e) => AsAdminChanged?.Invoke(AsAdmin.IsChecked);
        private void StartWithWindows_Click(object sender, RoutedEventArgs e) => StartWithWindowsChanged?.Invoke(StartWithWindows.IsChecked);
        private void BackgroundRun_Click(object sender, RoutedEventArgs e) => BackgroundRunChanged?.Invoke(BackgroundRun.IsChecked);
        private void AutoPlay_Click(object sender, RoutedEventArgs e) => AutoPlayChanged?.Invoke(AutoPlay.IsChecked);
        private void Reset_Click(object sender, RoutedEventArgs e) => ResetClick?.Invoke(this, new EventArgs());

    }
}
