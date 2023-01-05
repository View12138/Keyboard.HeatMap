namespace DontTouchKeyboard.UI.ViewModels;

internal partial class SettingViewModel : ObservableObject
{
    [ObservableProperty]
    private int themeIndex = 2;

    partial void OnThemeIndexChanged(int value)
    {
        App.TrySetWindowTheme(themeIndex switch
        {
            0 => ElementTheme.Dark,
            1 => ElementTheme.Light,
            _ => ElementTheme.Default,
        });
    }

    [ObservableProperty]
    private int backdropIndex = 3;
    partial void OnBackdropIndexChanged(int value)
    {

    }

    [ObservableProperty]
    private string currentVersion = "2.0.0-dev";

    [ObservableProperty]
    private string versionDate = "2022-01-04";

    [ObservableProperty]
    private bool isAdminEnabled = false;

    [ObservableProperty]
    private bool isElevated = false;

    [ObservableProperty]
    private bool runElevated = false;

    partial void OnRunElevatedChanged(bool value)
    {

    }

    [RelayCommand]
    public void CheckUpdate()
    {

    }

    [RelayCommand]
    public void Evaluate()
    {

    }

    [RelayCommand]
    public void RestartElevated()
    {

    }
}
