﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Windows.UI.Core;

namespace DontTouchKeyboard.UI.Extensions
{
    internal static class CoreVirtualKeyStatesExtension
    {

        public static bool IsLock(this CoreVirtualKeyStates states)
        {
            return (states & CoreVirtualKeyStates.Locked) == CoreVirtualKeyStates.Locked;
        }

        public static bool IsDown(this CoreVirtualKeyStates states)
        {
            return (states & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
        }

        public static bool IsDownOrLocked(this CoreVirtualKeyStates states)
        {
            return states.IsDown() || states.IsLock();
        }

        public static Visibility ToLockedVisibility(this CoreVirtualKeyStates states)
        {
            return states.IsLock() ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}