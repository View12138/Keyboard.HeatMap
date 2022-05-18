using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.Management.Infrastructure;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Management;
using Windows.System;
using WinRT.Interop;
using UnhandledExceptionEventArgs = Microsoft.UI.Xaml.UnhandledExceptionEventArgs;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DontTouchKeyboard.UI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {

        private AppWindow appWindow;
        public MainWindow()
        {
            this.InitializeComponent();
            var hWnd = WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            appWindow = AppWindow.GetFromWindowId(windowId);
            //appWindow.SetPresenter(AppWindowPresenterKind.Overlapped);
            appWindow.Title = "别敲键盘";

            //var topWindow = AppWindow.Create(CompactOverlayPresenter.Create());
            //topWindow.Show();

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            //Task.Run(() =>
            //{
            //    CimSession cimSession = CimSession.Create("localhost");
            //    IEnumerable<CimInstance> enumeratedInstances = cimSession.EnumerateInstances(@"root\cimv2", "Win32_Keyboard");
            //    foreach (CimInstance instance in enumeratedInstances)
            //    {
            //        var p = instance.CimInstanceProperties;
            //    }
            //});
        }

        private void TextBlock_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            keys.Text = $"Key:{e.Key} ({(int)e.Key}) - OriginalKey:{e.OriginalKey} ({(int)e.OriginalKey})";
        }

        private void Grid_PreviewKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key is VirtualKey.CapitalLock
                 or VirtualKey.NumberKeyLock
                 or VirtualKey.Insert
                 or VirtualKey.Scroll
                 or VirtualKey.Shift
                 or VirtualKey.LeftShift
                 or VirtualKey.RightShift
                 or VirtualKey.Control
                 or VirtualKey.LeftControl
                 or VirtualKey.RightControl
                 )
            {
                App.OnStatusChanged();
            }
        }

        private void Grid_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key is VirtualKey.CapitalLock
                 or VirtualKey.NumberKeyLock
                 or VirtualKey.Insert
                 or VirtualKey.Scroll
                 or VirtualKey.Shift
                 or VirtualKey.LeftShift
                 or VirtualKey.RightShift
                 or VirtualKey.Control
                 or VirtualKey.LeftControl
                 or VirtualKey.RightControl)
            {
                App.OnStatusChanged();
            }
            else
            {
                throw new Exception("一个非常难以处理的错误, 并且不知道怎么发生的");
            }
        }

        public void Exception_Throwing(object sender, UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            ErrorInfoBar.Tag = e;
            ErrorInfoBar.Title = "错误";
            ErrorInfoBar.Message = e.Message;
            ErrorInfoBar.IsOpen = true;
        }

        private void ErrorInfoBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (ErrorInfoBar.Tag is UnhandledExceptionEventArgs ex)
            {
                if (ErrorActionButton.Tag.ToString() == "false")
                {
                    ErrorActionButton.Tag = "true";
                    ErrorActionButton.Content = "摘要";

                    ErrorInfoBar.Title = ex.Message;
                    ErrorInfoBar.Message = ex.Exception.ToString() + Environment.NewLine;
                    ErrorInfoBar.IsOpen = true;
                }
                else
                {
                    ErrorActionButton.Tag = "false";
                    ErrorActionButton.Content = "详细";

                    ErrorInfoBar.Title = "错误";
                    ErrorInfoBar.Message = ex.Message;
                    ErrorInfoBar.IsOpen = true;
                }

            }
        }
    }
}
