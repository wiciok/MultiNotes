using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MultiNotes.Windows.View
{
    /// <summary>
    /// Interaction logic for MultiNotesMainWindow.xaml
    /// </summary>
    public partial class MultiNotesMainWindow : Window
    {
        public MultiNotesMainWindow()
        {
            InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if(this.WindowState == WindowState.Minimized)
            {
                SingleNoteWindow note = new SingleNoteWindow();
                note.Show();
            }
        }
    }
}
