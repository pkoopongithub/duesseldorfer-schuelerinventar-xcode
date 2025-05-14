using DüsseldorferSchülerinventar.Models;
using DüsseldorferSchülerinventar.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace DüsseldorferSchülerinventar.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly AssessmentService _assessmentService;
        
        [ObservableProperty]
        private string _profileName;

        [ObservableProperty]
        private DateTime _testDate;

        [ObservableProperty]
        private string _testDuration;

        [ObservableProperty]
        private ObservableCollection<CompetenceResult> _competenceResults;

        [ObservableProperty]
        private bool _isRefreshing;

        public ProfileViewModel(AssessmentService assessmentService)
        {
            _assessmentService = assessmentService;
            CompetenceResults = new ObservableCollection<CompetenceResult>();
        }

        [RelayCommand]
        private async Task LoadProfileData()
        {
            IsRefreshing = true;
            
            try
            {
                var profile = _assessmentService.CurrentProfile;
                if (profile == null) return;
                
                ProfileName = profile.Name ?? "Profil";
                TestDate = profile.CompletedDate;
                TestDuration = CalculateDuration(profile);
                
                CompetenceResults.Clear();
                
                var results = _assessmentService.CalculateResults();
                foreach (var result in results)
                {
                    CompetenceResults.Add(result);
                }
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private string CalculateDuration(Profile profile)
        {
            if (profile.StartedDate == default || profile.CompletedDate == default)
                return "Dauer: unbekannt";
            
            var duration = profile.CompletedDate - profile.StartedDate;
            return $"Dauer: {duration.TotalMinutes:F0} Minuten";
        }

        [RelayCommand]
        private async Task Edit()
        {
            await Shell.Current.GoToAsync($"{nameof(QuestionnaireView)}?editMode=true");
        }

        [RelayCommand]
        private async Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task Refresh()
        {
            await LoadProfileData();
        }
    }
}