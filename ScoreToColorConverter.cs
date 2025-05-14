using DüsseldorferSchülerinventar.Models;
using Microsoft.Maui.Graphics;

namespace DüsseldorferSchülerinventar.Converters
{
    public class ScoreToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int score)
            {
                return score switch
                {
                    1 => Color.FromArgb("#FF5252"),  // Rot
                    2 => Color.FromArgb("#FFAB40"),  // Orange
                    3 => Color.FromArgb("#FFD740"),  // Gelb
                    4 => Color.FromArgb("#69F0AE"),  // Grün
                    5 => Color.FromArgb("#448AFF"),  // Blau
                    _ => Colors.Gray
                };
            }
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}