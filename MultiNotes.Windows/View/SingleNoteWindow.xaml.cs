using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MultiNotes.Model;
using MultiNotes.Windows.Util;
using MultiNotes.Windows.ViewModel;

namespace MultiNotes.Windows.View
{
    /// <summary>
    /// Interaction logic for SingleNoteWindow.xaml
    /// </summary>
    public partial class SingleNoteWindow
    {
        public SingleNoteWindow(Note note)
        {
            InitializeComponent();
            DataContext = new SingleNoteWindowViewModel(x => Close(), note);
            this.ShowInTaskbar = false;

            // Manually alter window height and width
            SizeToContent = SizeToContent.Manual;

            // Automatically resize height relative to content
            SizeToContent = SizeToContent.Height;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if(menuItem!=null && NoteColors.colorsDictionary[menuItem.Name]!=null)
                this.MainGrid.Background = NoteColors.colorsDictionary[menuItem.Name];
        }
    }
}
