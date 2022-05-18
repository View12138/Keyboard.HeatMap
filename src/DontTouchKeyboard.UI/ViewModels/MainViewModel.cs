using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using UnhandledExceptionEventArgs = Microsoft.UI.Xaml.UnhandledExceptionEventArgs;

namespace DontTouchKeyboard.UI.ViewModels
{
    internal class MainViewModel : ObservableObject
    {
        private ObservableCollection<UnhandledExceptionEventArgs> exceptions;

        public ObservableCollection<UnhandledExceptionEventArgs> Exceptions { get => exceptions; set => SetProperty(ref exceptions, value); }
    }
}
