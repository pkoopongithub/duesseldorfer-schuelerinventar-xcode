using CommunityToolkit.Mvvm.ComponentModel;

namespace DüsseldorferSchülerinventar.Models
{
    public partial class QuestionItem : ObservableObject
    {
        public int Id { get; set; }
        public string Text { get; set; }
        
        [ObservableProperty]
        private int _selectedAnswer;
        
        public bool IsAnswer1Selected
        {
            get => SelectedAnswer == 4;
            set { if (value) SelectedAnswer = 4; }
        }
        
        public bool IsAnswer2Selected
        {
            get => SelectedAnswer == 3;
            set { if (value) SelectedAnswer = 3; }
        }
        
        public bool IsAnswer3Selected
        {
            get => SelectedAnswer == 2;
            set { if (value) SelectedAnswer = 2; }
        }
        
        public bool IsAnswer4Selected
        {
            get => SelectedAnswer == 1;
            set { if (value) SelectedAnswer = 1; }
        }
    }
}