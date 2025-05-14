using DüsseldorferSchülerinventar.ViewModels;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

namespace DüsseldorferSchülerinventar.Views
{
    public partial class QuestionnaireView : ContentPage
    {
        public QuestionnaireView()
        {
            InitializeComponent();
            On<iOS>().SetUseSafeArea(true);
            
            // iOS-spezifische Anpassungen für bessere Performance
#if IOS
            var carousel = this.FindByName<CarouselView>("QuestionCarousel");
            if (carousel != null)
            {
                carousel.On<iOS>().SetBounces(false);
            }
#endif
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is QuestionnaireViewModel viewModel)
            {
                viewModel.Initialize();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (BindingContext is QuestionnaireViewModel viewModel)
            {
                viewModel.SaveProgress();
            }
        }
    }
}