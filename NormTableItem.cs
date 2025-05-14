using CommunityToolkit.Mvvm.ComponentModel;

namespace DüsseldorferSchülerinventar.Models
{
    public partial class NormTableItem : ObservableObject
    {
        [ObservableProperty]
        private int _id;
        
        [ObservableProperty]
        private string _name;
        
        [ObservableProperty]
        private double _p1;
        
        [ObservableProperty]
        private double _p2;
        
        [ObservableProperty]
        private double _p3;
        
        [ObservableProperty]
        private double _p4;
        
        [ObservableProperty]
        private double _p5;
        
        public string PercentileInfo => $"P1: {P1:F1} | P2: {P2:F1} | P3: {P3:F1} | P4: {P4:F1} | P5: {P5:F1}";
    }
}