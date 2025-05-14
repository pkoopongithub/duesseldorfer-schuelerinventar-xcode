using DüsseldorferSchülerinventar.Models;
using DüsseldorferSchülerinventar.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace DüsseldorferSchülerinventar.ViewModels
{
    public partial class QuestionnaireViewModel : ObservableObject
    {
        private readonly AssessmentService _assessmentService;
        
        [ObservableProperty]
        private ObservableCollection<QuestionItem> _questions;

        [ObservableProperty]
        private QuestionItem _currentQuestion;

        [ObservableProperty]
        private int _currentQuestionIndex;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _nextButtonText = "Weiter";

        public bool IsNotFirstQuestion => CurrentQuestionIndex > 0;

        public QuestionnaireViewModel(AssessmentService assessmentService)
        {
            _assessmentService = assessmentService;
            Questions = new ObservableCollection<QuestionItem>();
        }

        public void Initialize()
        {
            LoadQuestions();
            UpdateButtonText();
        }

        private void LoadQuestions()
        {
            IsBusy = true;
            
            try
            {
                Questions.Clear();
                var allQuestions = _assessmentService.GetAllQuestions();
                
                foreach (var question in allQuestions)
                {
                    Questions.Add(new QuestionItem
                    {
                        Id = question.Id,
                        Text = question.Text,
                        // Antworten aus gespeichertem Profil laden falls vorhanden
                        SelectedAnswer = _assessmentService.CurrentProfile?.Answers[question.Id - 1] ?? 0
                    });
                }
                
                CurrentQuestion = Questions.FirstOrDefault();
                CurrentQuestionIndex = 0;
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private void PreviousQuestion()
        {
            if (CurrentQuestionIndex > 0)
            {
                CurrentQuestionIndex--;
                CurrentQuestion = Questions[CurrentQuestionIndex];
                UpdateButtonText();
            }
        }

        [RelayCommand]
        private void NextQuestion()
        {
            if (CurrentQuestionIndex < Questions.Count - 1)
            {
                CurrentQuestionIndex++;
                CurrentQuestion = Questions[CurrentQuestionIndex];
                UpdateButtonText();
            }
            else
            {
                FinishQuestionnaire();
            }
        }

        private void UpdateButtonText()
        {
            NextButtonText = CurrentQuestionIndex == Questions.Count - 1 ? "Abschließen" : "Weiter";
        }

        private async void FinishQuestionnaire()
        {
            IsBusy = true;
            try
            {
                await _assessmentService.CompleteAssessment();
                await Shell.Current.GoToAsync($"//{nameof(ProfileView)}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void SaveProgress()
        {
            if (_assessmentService.CurrentProfile != null)
            {
                foreach (var question in Questions)
                {
                    _assessmentService.CurrentProfile.Answers[question.Id - 1] = question.SelectedAnswer;
                }
                _assessmentService.SaveProfileProgress();
            }
        }
    }
}