using DüsseldorferSchülerinventar.Models;
using DüsseldorferSchülerinventar.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DüsseldorferSchülerinventar.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly ApiService _apiService;
        
        [ObservableProperty]
        private string _username;
        
        [ObservableProperty]
        private string _password;
        
        [ObservableProperty]
        private bool _isBusy;
        
        [ObservableProperty]
        private string _errorMessage;
        
        public bool IsNotBusy => !IsBusy;
        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        public LoginViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [RelayCommand]
        private async Task Login()
        {
            if (IsBusy) return;
            
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;
                
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Bitte Benutzername und Passwort eingeben";
                    return;
                }

                var user = await _apiService.Login(Username, Password);
                
                if (user != null)
                {
                    // Erfolgreiche Anmeldung
                    await Shell.Current.GoToAsync($"//{nameof(MainMenuView)}");
                }
                else
                {
                    ErrorMessage = "Anmeldung fehlgeschlagen";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fehler: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task NavigateToRegister()
        {
            await Shell.Current.GoToAsync(nameof(RegisterView));
        }

        public void ResetState()
        {
            Username = string.Empty;
            Password = string.Empty;
            ErrorMessage = string.Empty;
            IsBusy = false;
        }
    }
}