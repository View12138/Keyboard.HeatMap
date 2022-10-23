using Microsoft.UI.Xaml;

namespace DontTouchKeyboard.UI.Extensions
{
    internal static class DependencyObjectExtension
    {
        public static T GetValue<T>(this DependencyObject d, DependencyProperty dp)
        {
            return (T)d.GetValue(dp);
        }
    }
}
