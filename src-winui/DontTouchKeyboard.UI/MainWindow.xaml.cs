// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Composition.SystemBackdrops;
using WinRT; // required to support Window.As<ICompositionSupportsSystemBackdrop>()
using Microsoft.UI.Composition;
using PIncoke;
using Windows.UI;
using Microsoft.Extensions.Configuration;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DontTouchKeyboard.UI;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        ThemeHelpers.TrySetSystemBackdrop(this, Backdrop.Auto);

        ThemeHelpers.TrySetCustomTitleBar(this, ShellPage);

        ThemeHelpers.OnThemeChange(this);
    }

    private void Grid_PreviewKeyUp(object sender, KeyRoutedEventArgs e)
    {
        App.OnStatusChanged(sender, e.Key);
    }

    private void Grid_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
    {
        App.OnStatusChanged(sender, e.Key);
    }
}
