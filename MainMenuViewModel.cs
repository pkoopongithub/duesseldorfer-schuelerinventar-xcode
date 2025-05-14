using DüsseldorferSchülerinventar.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DüsseldorferSchülerinventar.ViewModels
{
    public partial class MainMenuViewModel : ObservableObject
    {
        private readonly AuthService _authService;
        
        [ObservableProperty]
        private bool _isAdmin;

        public MainMenuViewModel(AuthService authService)
        {
            _authService = authService;
            CheckAdminStatus();
        }

        public void CheckAdminStatus()
        {
            IsAdmin = _authService.CurrentUser?.IsAdmin ?? false;
        }

        [RelayCommand]
        private async Task NavigateToQuestionnaire()
        {
            await Shell.Current.GoToAsync(nameof(QuestionnaireView));
        }

        [RelayCommand]
        private async Task NavigateToProfileList()
        {
            await Shell.Current.GoToAsync(nameof(ProfileListView));
        }

        [RelayCommand]
        private async Task NavigateToAdmin()
        {
            await Shell.Current.GoToAsync(nameof(AdminView));
        }

        [RelayCommand]
        private async Task Logout()
        {
            _authService.Logout();
            await Shell.Current.GoToAsync($"//{nameof(LoginView)}");
        }
    }
}