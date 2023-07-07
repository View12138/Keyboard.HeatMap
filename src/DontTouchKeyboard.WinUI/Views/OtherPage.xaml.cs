// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DontTouchKeyboard.WinUI.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OtherPage : Page
    {
        public OtherPage()
        {
            this.InitializeComponent();
        }

        private bool isOpen = false;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is bool isOpen)
            {
                this.isOpen = isOpen;
            }
            base.OnNavigatedTo(e);
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem navigationViewItem)
            {
                FrameNavigationOptions navOptions = new FrameNavigationOptions();
                navOptions.TransitionInfoOverride = isOpen ? new DrillInNavigationTransitionInfo() : args.RecommendedNavigationTransitionInfo;
                navOptions.IsNavigationStackEnabled = sender.PaneDisplayMode != NavigationViewPaneDisplayMode.Top;

                var pageType = NavHelper.GetNavigateTo(navigationViewItem);

                OtherFrame.NavigateToType(pageType, null, navOptions);
            }
        }
    }
}
