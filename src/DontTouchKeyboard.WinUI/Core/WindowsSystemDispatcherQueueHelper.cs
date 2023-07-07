// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DontTouchKeyboard.WinUI.Core;
internal class WindowsSystemDispatcherQueueHelper
{

    object? m_dispatcherQueueController = null;
    public void EnsureWindowsSystemDispatcherQueueController()
    {
        if (DispatcherQueue.GetForCurrentThread() != null)
        {
            // one already exists, so we'll just use it.
            return;
        }

        if (m_dispatcherQueueController == null)
        {
            DispatcherQueueOptions options = new()
            {
                dwSize = Marshal.SizeOf(typeof(DispatcherQueueOptions)),
                threadType = 2,    // DQTYPE_THREAD_CURRENT
                apartmentType = 2 // DQTAT_COM_STA
            };

            _ = CoreMessaging.CreateDispatcherQueueController(options, ref m_dispatcherQueueController);
        }
    }
}