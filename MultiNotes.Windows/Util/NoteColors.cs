using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MultiNotes.Windows.Util
{
    public static class NoteColors
    {
        public static Dictionary<string, System.Windows.Media.SolidColorBrush> colorsDictionary;

        static NoteColors()
        {
            colorsDictionary = new Dictionary<string, SolidColorBrush>(10);
            colorsDictionary.Add("Red", Brushes.IndianRed);
            colorsDictionary.Add("Blue", Brushes.CornflowerBlue);
        }
    }
}
