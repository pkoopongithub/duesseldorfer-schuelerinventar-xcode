using DüsseldorferSchülerinventar.ViewModels;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

namespace DüsseldorferSchülerinventar.Views
{
    public partial class ProfileView : ContentPage
    {
        public ProfileView()
        {
            InitializeComponent();
            On<iOS>().SetUseSafeArea(true);
            
            // iOS-spezifische Anpassungen
#if IOS
            var scrollView = this.FindByName<ScrollView>("MainScrollView");
            if (scrollView != null)
            {
                scrollView.On<iOS>().SetShouldDelayContentTouches(false);
            }
#endif
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is ProfileViewModel viewModel)
            {
                viewModel.LoadProfileData();
            }
        }
    }
}