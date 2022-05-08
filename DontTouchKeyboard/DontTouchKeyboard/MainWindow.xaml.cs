using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Windows.System;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DontTouchKeyboard
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.Title = "别敲键盘";

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
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
        }
    }
}
