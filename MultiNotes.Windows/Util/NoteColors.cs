using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

//todo: najlepiej byłoby zmienić to na słownik i wczytywać go z pliku xaml

namespace MultiNotes.Windows.Util
{
    public static class NoteColors
    {
        public static List<SolidColorBrush> ColorList;

        static NoteColors()
        {
            ColorList = new List<SolidColorBrush>
            {
                Brushes.PaleVioletRed,
                Brushes.LightSteelBlue,
                Brushes.Cornsilk, 
                Brushes.RosyBrown,
                Brushes.LightSalmon,
                Brushes.Azure,
                Brushes.LightPink,
                Brushes.LightGreen,
                Brushes.LightGoldenrodYellow,
                Brushes.HotPink,
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF8C1A"))
            };
        }
    }
}
