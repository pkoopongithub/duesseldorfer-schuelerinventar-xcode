using DüsseldorferSchülerinventar.ViewModels;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

namespace DüsseldorferSchülerinventar.Views
{
    public partial class AdminView : ContentPage
    {
        public AdminView()
        {
            InitializeComponent();
            On<iOS>().SetUseSafeArea(true);
            
            // iOS Performance Optimization
#if IOS
            var collectionView = this.FindByName<CollectionView>("NormTablesCollection");
            if (collectionView != null)
            {
                collectionView.On<iOS>().SetPrefersItemAnimations(false);
            }
#endif
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is AdminViewModel viewModel)
            {
                viewModel.LoadNormTablesCommand.Execute(null);
            }
        }
    }
}