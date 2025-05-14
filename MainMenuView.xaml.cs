using DüsseldorferSchülerinventar.ViewModels;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

namespace DüsseldorferSchülerinventar.Views
{
    public partial class MainMenuView : ContentPage
    {
        public MainMenuView()
        {
            InitializeComponent();
            
            // iOS-spezifische Anpassungen
            On<iOS>().SetUseSafeArea(true);
            
            // Navigationsevents abonnieren
            Shell.Current.Navigated += OnNavigatedTo;
        }

        private void OnNavigatedTo(object sender, ShellNavigatedEventArgs e)
        {
            // ViewModel aktualisieren wenn Seite sichtbar wird
            if (e.Current?.Location.ToString().Contains(nameof(MainMenuView)) ?? false)
            {
                if (BindingContext is MainMenuViewModel viewModel)
                {
                    viewModel.CheckAdminStatus();
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Shell.Current.Navigated -= OnNavigatedTo;
        }
    }
}