namespace PInvoke;

public static class CoreMessaging
{
    [DllImport("CoreMessaging.dll", EntryPoint = "CreateDispatcherQueueController")]
    public static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object? dispatcherQueueController);
}


[StructLayout(LayoutKind.Sequential)]
public struct DispatcherQueueOptions
{
    public int dwSize;
    public int threadType;
    public int apartmentType;
}
