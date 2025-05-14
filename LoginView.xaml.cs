using DüsseldorferSchülerinventar.ViewModels;
using Microsoft.Maui.Controls;

namespace DüsseldorferSchülerinventar.Views
{
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();
            
            // iOS-spezifische Anpassungen
#if IOS
            // Keyboard-Anpassungen für iOS
            Entry usernameEntry = this.FindByName<Entry>("usernameEntry");
            Entry passwordEntry = this.FindByName<Entry>("passwordEntry");
            
            if (usernameEntry != null && passwordEntry != null)
            {
                usernameEntry.Completed += (s, e) => passwordEntry.Focus();
                passwordEntry.Completed += (s, e) => passwordEntry.Unfocus();
            }
#endif
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            // ViewModel zurücksetzen bei erneutem Aufruf
            if (BindingContext is LoginViewModel viewModel)
            {
                viewModel.ResetState();
            }
        }
    }
}