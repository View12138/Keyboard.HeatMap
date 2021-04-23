using Keyboard.HeatMap.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Keyboard.HeatMap
{
    /// <summary>
    /// MigrationWindows.xaml 的交互逻辑
    /// </summary>
    public partial class MigrationWindows : Window
    {
        public MigrationWindows()
        {
            InitializeComponent();
            Loaded += MigrationWindows_Loaded;
        }

        private async void MigrationWindows_Loaded(object sender, RoutedEventArgs e)
        {
            await DbHandle.ChangeDb();
            DialogResult = true;
        }
    }
}
