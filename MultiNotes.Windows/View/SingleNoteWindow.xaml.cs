using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
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

            //dynamically create color menu elements
            foreach (var el in NoteColors.ColorList)
            {
                var menuItemTmp = new MenuItem()
                {
                    Height = 31,
                    Margin = new Thickness(0, 0, -47.8, 0),
                    Header = new Rectangle()
                    {
                        Fill = el,
                        Height = 25,
                        Width = 150,
                        Stroke = System.Windows.Media.Brushes.Black
                    }
                };
                menuItemTmp.Click += MenuItem_OnClick;

                ColorMenu.Items.Add(menuItemTmp);
            }
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
            if (sender is MenuItem menuItem)
            {
                var rectangle = menuItem.Header as Rectangle;
                if (rectangle != null)
                    MainGrid.Background = rectangle.Fill;
            }
        }
    }
}
