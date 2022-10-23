using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DontTouchKeyboard.Runner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //Task.Run(() =>
            //{
            //    CimSession cimSession = CimSession.Create("localhost");
            //    IEnumerable<CimInstance> enumeratedInstances = cimSession.EnumerateInstances(@"root\cimv2", "Win32_Keyboard");
            //    foreach (CimInstance instance in enumeratedInstances)
            //    {
            //        var p = instance.CimInstanceProperties;
            //    }
            //});
            Process.Start(@"C:\Users\ms_vi\OneDrive\Projects\view\dotnet\Keyboard.HeatMap\DontTouchKeyboard\DontTouchKeyboard (Package)\bin\x64\Debug\DontTouchKeyboard\DontTouchKeyboard.exe");
        }
    }
}
