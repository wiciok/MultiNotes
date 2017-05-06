using System.Windows;
using System.Windows.Input;

namespace MultiNotes.Windows.View
{
    /// <summary>
    /// Interaction logic for SingleNoteWindow.xaml
    /// </summary>
    public partial class SingleNoteWindow
    {
        public SingleNoteWindow()
        {
            InitializeComponent();

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
    }
}
