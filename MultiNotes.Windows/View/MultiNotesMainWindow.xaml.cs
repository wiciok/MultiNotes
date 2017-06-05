using MultiNotes.Core;
using MultiNotes.Windows.ViewModel;
using System;
using System.Windows;
using System.Windows.Input;

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
            ConnectionApi.Configure();
            var vm = new MainWindowViewModel(Close);
            DataContext = vm;

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
    }
}
