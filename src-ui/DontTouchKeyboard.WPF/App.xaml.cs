using System;
using System.Windows;

namespace Keyboard.HeatMap
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 开始统计按键数的时间
        /// </summary>
        public static DateTime StartQueryTime;
        System.Threading.Mutex mutex;
        public App()
        {
            StartQueryTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
            this.Startup += new StartupEventHandler(App_Startup);
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            bool ret;
            mutex = new System.Threading.Mutex(true, "ElectronicNeedleTherapySystem", out ret);
            if (!ret)
            { Environment.Exit(0); }
            if (e.Args.Length == 1)
            {
                switch (e.Args[0])
                {
                    case "-now": StartQueryTime = DateTime.Now; break;
                    case "-today": StartQueryTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0); break;
                    case "-yesterday": StartQueryTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1, 8, 0, 0); break;
                    case "-week": StartQueryTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 7, 8, 0, 0); break;
                    case "-month": StartQueryTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day, 8, 0, 0); break;
                    case "-year": StartQueryTime = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0); break;
                    default:
                        break;
                }
            }

        }
    }
}
