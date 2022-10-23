using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DontTouchKeyboard.UI.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Windows.System;
using Windows.UI.Core;
using DontTouchKeyboard.UI.Extensions;

namespace DontTouchKeyboard.UI.Converters
{
    public class KeyToVisualConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var status = parameter as KeyStates;
            if (value is VirtualKey key)
            {
                return key.ToVisual(status);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException("不支持的转换");
        }


    }
}
