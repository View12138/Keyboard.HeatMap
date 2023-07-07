namespace Keyboard.HeatMap;

/// <summary>
/// MainWindow.xaml 的交互逻辑
/// </summary>
public partial class MainWindow : Window
{
    // 字段
    /// <summary>
    /// 是否是任务栏菜单点击的关闭
    /// </summary>
    private bool MenuClickClose;
    /// <summary>
    /// 快捷方式帮助类
    /// </summary>
    private Shortcut Shortcut;
    /// <summary>
    /// 键盘钩子帮助类
    /// </summary>
    private KeyBoardHandle KeyBoard;
    /// <summary>
    /// 任务栏菜单
    /// <para>初始化后请调用 <c>TaskMenu.Show();</c> 一次</para>
    /// </summary>
    private TaskMenu Menu;
    /// <summary>
    /// 任务栏图标
    /// </summary>
    private WinForms.NotifyIcon notifyIcon;
    /// <summary>
    /// 开始统计时间 到 目前的按键统计数据
    /// </summary>
    private Dictionary<WinForms.Keys, long> TodayKeyUpList;

    /// <summary>
    /// 初始化
    /// </summary>
    public MainWindow()
    {
        DataContext = Shortcut;
        InitializeComponent();
        Hide();
        Closing += (s, e) =>
        {
            if (MenuClickClose)
            {
                notifyIcon.Visible = false;
                Menu.Close();
                KeyBoard.Dispose();
            }
            else
            {
                Hide();
                e.Cancel = true;
            }
        };
        Activated += (s, e) =>
        {
            InputBox.Focus();
        };

        // 初始化字段
        MenuClickClose = false;
        Shortcut = new Shortcut("别敲键盘");
        KeyBoard = new KeyBoardHandle();
        Menu = new TaskMenu();
        notifyIcon = new WinForms.NotifyIcon();
        TodayKeyUpList = new Dictionary<WinForms.Keys, long>();

        KeyBoardControl.CloseKeyClick += (s, e) =>
        {
            Close();
        };
        KeyBoardControl.ResetClick += (s, e) =>
        {
            App.StartQueryTime = DateTime.Now;
            TodayKeyUpList.Clear();
            KeyBoardControl.ResetKeyBoardHot();
            Title = $"{Shortcut.ProductName} - 自 {App.StartQueryTime:M月d日 H点m分} 以来的统计数据";
        };
        KeyBoardControl.StartWithWindowsChanged += (check) =>
        {
            if (check.HasValue)
            {
                DontTouchKeyboard.WPF.Properties.Settings.Default.StartWithWindows = check.Value;
                DontTouchKeyboard.WPF.Properties.Settings.Default.Save();
                if (check.Value)
                {
                    Shortcut.CreateShortcut(ShortcutType.Startup);
                }
                else
                {
                    Shortcut.RemoveShortcut(ShortcutType.Startup);
                }
            }
        };
        KeyBoardControl.BackgroundRunChanged += (check) =>
        {
            if (check.HasValue)
            {
                DontTouchKeyboard.WPF.Properties.Settings.Default.BackgroundRun = check.Value;
                DontTouchKeyboard.WPF.Properties.Settings.Default.Save();
            }
        };
        KeyBoardControl.AutoPlayChanged += (check) =>
        {
            if (check.HasValue)
            {
                DontTouchKeyboard.WPF.Properties.Settings.Default.AutoPlay = check.Value;
                DontTouchKeyboard.WPF.Properties.Settings.Default.Save();
            }
        };

        DateTime time = DateTime.Now;
        KeyBoard.KeyDown += (s, e) =>
        {
            KeyBoardControl.SetDown(e.KeyCode);
            if (KeyBoard.KeyState == KeyState.Closed)
            { return; }
        };
        KeyBoard.KeyUp += (s, e) =>
        {
            KeyBoardControl.SetUp(e.KeyCode);
            if (KeyBoard.KeyState == KeyState.Closed)
            { return; }
            if (TodayKeyUpList.ContainsKey(e.KeyCode))
            {
                int i = (int)e.KeyCode;
                TodayKeyUpList[e.KeyCode] += 1;
            }
            else
            {
                TodayKeyUpList.Add(e.KeyCode, 1);
            }
            double timeSpan = DontTouchKeyboard.WPF.Properties.Settings.Default.RefreshTimeSpan;
            if (timeSpan < 0)
            { return; }
            if (timeSpan == 0)
            {
                KeyBoardControl.SetKeyListHot(TodayKeyUpList);
            }
            else if (DateTime.Now - time > TimeSpan.FromSeconds(timeSpan))
            {
                time = DateTime.Now;
                KeyBoardControl.SetKeyListHot(TodayKeyUpList);
            }
            Title = $"{Shortcut.ProductName} —— 自 {App.StartQueryTime:yy年M月d日 H点m分} 以来共击键 {TodayKeyUpList.Values.Sum()} 次";
        };
        KeyBoard.KeyStateChanged += (s, e) =>
        {
            notifyIcon.Text = $"{Shortcut.ProductName} - {(e == KeyState.Open ? "记录中" : "已暂停")}";
            Menu.SetState(KeyBoard.KeyState);
        };

        Menu.Show(KeyBoard.KeyState);
        Menu.CloseH += (s, e) =>
        {
            MenuClickClose = true;
            Close();
        };
        Menu.ShowH += (s, e) =>
        {
            Show();
            Activate();
        };
        Menu.StartR += (s, e) =>
        {
            KeyBoard.Start();
        };
        Menu.StopR += (s, e) =>
        {
            KeyBoard.Stop();
        };

        notifyIcon.Text = $"{Shortcut.ProductName} - {(KeyBoard.KeyState == KeyState.Open ? "记录中" : "已暂停")}";
        notifyIcon.Icon = Properties.Resources.Logo;
        notifyIcon.Visible = true;
        notifyIcon.MouseClick += (s, e) =>
        {
            if (e.Button == WinForms.MouseButtons.Right)
            {
                Menu.ShowMenu();
            }
        };
        notifyIcon.DoubleClick += (s, e) =>
        {
            Show();
            Activate();
        };

        {
            if (DontTouchKeyboard.WPF.Properties.Settings.Default.Version == 0)
            {
                MigrationWindows migration = new MigrationWindows();
                migration.ShowDialog();
                DontTouchKeyboard.WPF.Properties.Settings.Default.Version = 1;
                DontTouchKeyboard.WPF.Properties.Settings.Default.Save();
            }
            if (DontTouchKeyboard.WPF.Properties.Settings.Default.FirstRun)
            {
                string content = "也许你不知道，你都敲了多少次键盘，也许你也根本不关心。但是现在你可以，为了键盘的健康，让我们一起别敲键盘吧！";
                string title = " —— 《 别敲键盘 》";
                if (MessageBox.Show(content, $"您好{title}", MessageBoxButton.OK) == MessageBoxResult.OK)
                {
                    DontTouchKeyboard.WPF.Properties.Settings.Default.FirstRun = false;

                    if (!Shortcut.GetShortcut(ShortcutType.Desktop))
                    {
                        if (MessageBox.Show("是否创建一个桌面的快捷方式？", $"还有一步{title}", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        { Shortcut.CreateShortcut(ShortcutType.Desktop); }
                    }
                    if (!Shortcut.GetShortcut(ShortcutType.Startup))
                    {
                        if (MessageBox.Show("让我们每天都记录一下键盘的敲击情况，怎么样？\r\n\r\n【这需要让我随 Windows 一起启动，但您仍然可以稍后在程序中设置此选项】", $"马上就好{title}", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            DontTouchKeyboard.WPF.Properties.Settings.Default.StartWithWindows = Shortcut.CreateShortcut(ShortcutType.Startup);
                        }
                        else
                        { DontTouchKeyboard.WPF.Properties.Settings.Default.StartWithWindows = false; }
                    }
                    DontTouchKeyboard.WPF.Properties.Settings.Default.Save();
                    Show();

                    Activate();
                }
                else
                {
                    MenuClickClose = true;
                    Close();
                }
            }
            bool asAdmin = DontTouchKeyboard.WPF.Properties.Settings.Default.AsAdmin;
            bool startWithWindows = DontTouchKeyboard.WPF.Properties.Settings.Default.StartWithWindows;
            bool backgroundRun = DontTouchKeyboard.WPF.Properties.Settings.Default.BackgroundRun;
            bool autoPlay = DontTouchKeyboard.WPF.Properties.Settings.Default.AutoPlay;
            if (startWithWindows)
            {
                if (!Shortcut.GetShortcut(ShortcutType.Startup))
                {
                    Shortcut.CreateShortcut(ShortcutType.Startup);
                }
            }

            if (!backgroundRun)
            {
                Show();
                Activate();
            }

            if (autoPlay)
            {
                KeyBoard.Start();
            }

            TodayKeyUpList = KeyBoard.Count(App.StartQueryTime, DateTime.Now);
            KeyBoardControl.SetKeyListHot(TodayKeyUpList);
            KeyBoardControl.SetSettingState(asAdmin, startWithWindows, backgroundRun, autoPlay);
            Title = $"{Shortcut.ProductName} - 自 {App.StartQueryTime:yy年M月d日 H点m分} 以来的统计数据";
        };
    }
}
