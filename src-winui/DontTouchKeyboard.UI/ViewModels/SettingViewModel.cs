#pragma warning disable CA1822 // Mark members as static
namespace DontTouchKeyboard.UI.ViewModels;

internal partial class SettingViewModel : ObservableObject
{
    // Properties

    [ObservableProperty] private int themeIndex = 2;
    public ElementTheme GetTheme() => themeIndex switch
    {
        0 => ElementTheme.Dark,
        1 => ElementTheme.Light,
        _ => ElementTheme.Default,
    };
    partial void OnThemeIndexChanged(int value) => App.TrySetWindowTheme(GetTheme());

    [ObservableProperty] private int backdropIndex = 0;
    public Backdrop GetBackdrop() => backdropIndex switch
    {
        0 => Backdrop.Mica,
        1 => Backdrop.Acrylic,
        _ => Backdrop.Custom,
    };
    partial void OnBackdropIndexChanged(int value) => App.TrySetSystemBackdrop(GetBackdrop());

    [ObservableProperty] private Visibility micaSupported = MicaController.IsSupported() ? Visibility.Collapsed : Visibility.Visible;

    [ObservableProperty] private Visibility acrylicSupported = DesktopAcrylicController.IsSupported() ? Visibility.Collapsed : Visibility.Visible;


    [ObservableProperty] private bool isAdminEnabled = false;

    [ObservableProperty] private bool isElevated = false;

    [ObservableProperty] private bool runElevated = false;

    partial void OnRunElevatedChanged(bool value)
    {

    }

    [ObservableProperty] private StorageFile? backgroundPath;

    // Commands

    [RelayCommand] public void OpenColorsSettings() => StartProcessHelper.Start(OtherApp.Settings_Colors);
    [RelayCommand] public void OpenBackgroundsSettings() => StartProcessHelper.Start(OtherApp.Settings_Backgrounds);

    [RelayCommand] public void Evaluate() => StartProcessHelper.Start(OtherApp.Store_Review);
    [RelayCommand] public void CheckUpdate() => StartProcessHelper.Start(OtherApp.Store_Update);


    [RelayCommand] public async void SelectBackgroundImage() => BackgroundPath = await App.PickSingleFileDialog();
    [RelayCommand] public void RestartElevated() { }


}
