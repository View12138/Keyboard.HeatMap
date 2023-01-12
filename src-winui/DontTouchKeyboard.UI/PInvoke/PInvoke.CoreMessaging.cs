namespace PInvoke;

internal static class CoreMessaging
{
    [DllImport("CoreMessaging.dll")]
    public static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object? dispatcherQueueController);
}


[StructLayout(LayoutKind.Sequential)]
internal struct DispatcherQueueOptions
{
    internal int dwSize;
    internal int threadType;
    internal int apartmentType;
}
