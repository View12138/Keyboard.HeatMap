﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DontTouchKeyboard.Plugs
{
    public class Class1
    {
    }

    public interface IPlugs
    {
        public void OnStart();

        public void OnStop();

        public void OnPause();

        public void OnResume();

        public void OnDestroy();

        public void OnApplicationQuit();
    }
}
