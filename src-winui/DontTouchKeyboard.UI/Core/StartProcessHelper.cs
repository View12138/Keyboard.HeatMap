using System.Diagnostics;

namespace DontTouchKeyboard.UI.Core;

public static class StartProcessHelper
{
    public static void Start(OtherApp otherApp)
    {
        Process.Start(new ProcessStartInfo(otherApp.Process) { UseShellExecute = true });
    }
}


public class OtherApp
{
    private OtherApp() => ThrowHelper.ThrowInvalidOperationException();
    private OtherApp(string process)
    {
        Process = process;
    }

    public static OtherApp Settings_Colors => new("ms-settings:colors");

    public static OtherApp Settings_Backgrounds => new("ms-settings:personalization-background");

    public string Process { get; }
}