using System.Diagnostics;

namespace DontTouchKeyboard.WinUI.Core;

public static class StartProcessHelper
{
    public static void Start(OtherApp otherApp)
    {
        Process.Start(new ProcessStartInfo(otherApp.Process) { UseShellExecute = true });
    }
}


public sealed class OtherApp
{
    private OtherApp() => ThrowHelper.ThrowInvalidOperationException();
    private OtherApp(string process)
    {
        Process = process;
    }

    public static OtherApp Settings_Colors => new("ms-settings:colors");
    public static OtherApp Settings_Backgrounds => new("ms-settings:personalization-background");

    public static OtherApp Store_Review => new($"ms-windows-store://review/?ProductId={App.ProductId}&mode=mini");
    public static OtherApp Store_Update => new($"ms-windows-store://pdp/?ProductId={App.ProductId}&mode=mini");

    public string Process { get; }
}