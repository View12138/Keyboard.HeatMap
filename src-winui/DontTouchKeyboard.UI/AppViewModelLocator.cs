#pragma warning disable CA1822 // Mark members as static
namespace DontTouchKeyboard.UI;

internal class AppViewModelLocator
{
    public ShellViewModel Shell => Ioc.Default.GetRequiredService<ShellViewModel>();

    public SystemInfoViewModel Error => Ioc.Default.GetRequiredService<SystemInfoViewModel>();

    public SettingViewModel Setting => Ioc.Default.GetRequiredService<SettingViewModel>();
}
