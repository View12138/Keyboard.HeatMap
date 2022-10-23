using System;
using System.Text;

namespace DontTouchKeyboard.UI.Extensions
{
    /// <summary>
    /// ExceptionExtensions
    /// </summary>
    internal static class ExceptionExtensions
    {
        /// <summary>
        /// 获取所有错误
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="isProduction">是否是生产环境<para>生产环境仅包含 Message，而不包含 调用栈</para></param>
        /// <param name="hasInner">是否包括所有内部错误</param>
        /// <param name="title"></param>
        /// <param name="innerTitle"></param>
        /// <returns></returns>
        public static string ToMessage(this Exception ex, bool isProduction = false, bool hasInner = true)
        {
            var message = new StringBuilder();
            message.AppendLine(ex.Message);
            if (!isProduction && !string.IsNullOrEmpty(ex.StackTrace))
            { message.AppendLine(ex.StackTrace); }

            Exception inner = ex;
            var space = "   ";
            int index = 1;
            while (hasInner && (inner = inner.InnerException) != null)
            {
                message.AppendLine($"{space}{index}. : {inner.Message}");
                if (!isProduction && !string.IsNullOrEmpty(inner.StackTrace))
                { message.AppendLine($"{space}{inner.StackTrace}"); }
                index += 1;
            }
            return message.ToString();
        }
    }
}
