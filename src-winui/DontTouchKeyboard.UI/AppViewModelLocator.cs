namespace DontTouchKeyboard.UI;

internal class AppViewModelLocator
{
    public ShellViewModel Shell => Ioc.Default.GetService<ShellViewModel>();

    public SystemInfoViewModel Error => Ioc.Default.GetService<SystemInfoViewModel>();
}
