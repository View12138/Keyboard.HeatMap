using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Keyboard.HeatMap.Common;
using Keyboard.HeatMap.Owner;

namespace Keyboard.HeatMap.Controls
{
    /// <summary>
    /// TaskMenu.xaml 的交互逻辑
    /// </summary>
    public partial class TaskMenu : Window
    {
        DoubleAnimation showMenu;

        public event EventHandler StartR;
        public event EventHandler StopR;
        public event EventHandler ShowH;
        public event EventHandler CloseH;

        public TaskMenu()
        {
            InitializeComponent();
            showMenu = new DoubleAnimation(1, TimeSpan.FromMilliseconds(200));
            Deactivated += (s, e) =>
            {
                Visibility = Visibility.Collapsed;
            };
        }

        /// <summary>
        /// 显示菜单
        /// </summary>
        public void ShowMenu()
        {
            Visibility = Visibility.Visible;
            var mousePosition = System.Windows.Forms.Control.MousePosition;
            float dpiX, dpiY;
            IntPtr dc = Win32.GetDC(IntPtr.Zero);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromHdc(dc))
            {
                dpiX = g.DpiX;
                dpiY = g.DpiY;
            }
            Left = (mousePosition.X) / (dpiY / 96);
            if (Left < 0) { Left = 0; }
            if (Left + Width > System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width) { Left = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - Width; }
            Top = (mousePosition.Y - ActualHeight * (dpiY / 96) - 5) / (dpiY / 96);
            if (Top < 0) { Top = 0; }
            if (Top + Width > System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height) { Top = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - Height; }
            BeginAnimation(OpacityProperty, showMenu);
            Activate();
        }

        public void Show(KeyState state)
        {
            Show();
            SetState(state);
        }

        /// <summary>
        /// 设置当前状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="hideMenu">是否隐藏菜单</param>
        public void SetState(KeyState state, bool hideMenu = false)
        {
            if (this.Dispatcher.Thread != Thread.CurrentThread)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (state == KeyState.Open)
                    {
                        StopKeyReadGrid.Visibility = Visibility.Visible;
                        StartKeyReadGrid.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        StopKeyReadGrid.Visibility = Visibility.Collapsed;
                        StartKeyReadGrid.Visibility = Visibility.Visible;
                    }
                    if (hideMenu)
                    { Visibility = Visibility.Collapsed; }
                }));
            }
            else
            {
                if (state == KeyState.Open)
                {
                    StopKeyReadGrid.Visibility = Visibility.Visible;
                    StartKeyReadGrid.Visibility = Visibility.Collapsed;
                }
                else
                {
                    StopKeyReadGrid.Visibility = Visibility.Collapsed;
                    StartKeyReadGrid.Visibility = Visibility.Visible;
                }
                if (hideMenu)
                { Visibility = Visibility.Collapsed; }
            }
        }

        private void CloseGrid_MouseUp(object sender, MouseButtonEventArgs e) => Handle(CloseH);

        private void ShowHomeGrid_MouseUp(object sender, MouseButtonEventArgs e) => Handle(ShowH);

        private void StopKeyReadGrid_MouseUp(object sender, MouseButtonEventArgs e) => Handle(StopR, false);

        private void StartKeyReadGrid_MouseUp(object sender, MouseButtonEventArgs e) => Handle(StartR, false);

        private void Handle(EventHandler @event, bool hideMenu = true)
        {
            @event?.Invoke(this, new EventArgs());
            if (hideMenu)
            { Visibility = Visibility.Collapsed; }
        }
    }
}
