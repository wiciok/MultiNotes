using System;
using System.Windows;

namespace MultiNotes.Windows.View
{
    /// <summary>
    /// Interaction logic for MultiNotesMainWindow.xaml
    /// </summary>
    public partial class MultiNotesMainWindow
    {
        public MultiNotesMainWindow()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            /*if(WindowState == WindowState.Minimized)
            {
                SingleNoteWindow note = new SingleNoteWindow();
                note.Show();
            }*/
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
