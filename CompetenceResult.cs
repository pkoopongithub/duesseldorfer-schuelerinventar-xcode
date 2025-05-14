using CommunityToolkit.Mvvm.ComponentModel;

namespace DüsseldorferSchülerinventar.Models
{
    public class CompetenceResult : ObservableObject
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public string Description { get; set; }
        public double Percent => Score * 20; // 1-5 to 20-100%
    }
}