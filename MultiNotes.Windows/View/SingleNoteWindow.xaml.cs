using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
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

        //-------------custom grip code--------------

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        //Attach this to the MouseDown event of your drag control to move the window in place of the title bar
        private void WindowDrag(object sender, MouseButtonEventArgs e) // MouseDown
        {
            ReleaseCapture();
            SendMessage(new WindowInteropHelper(this).Handle,
                0xA1, (IntPtr)0x2, (IntPtr)0);
        }

        //Attach this to the PreviewMousLeftButtonDown event of the grip control in the lower right corner of the form to resize the window
        private void WindowResize(object sender, MouseButtonEventArgs e) //PreviewMousLeftButtonDown
        {
            HwndSource hwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
            SendMessage(hwndSource.Handle, 0x112, (IntPtr)61448, IntPtr.Zero);
        }
        //--------------------------------------------
    }
}
