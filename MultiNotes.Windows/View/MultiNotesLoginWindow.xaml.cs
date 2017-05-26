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
    /// Interaction logic for MultiNotesLoginWindow.xaml
    /// </summary>
    public partial class MultiNotesLoginWindow : Window
    {
        public MultiNotesLoginWindow()
        {
            InitializeComponent();

            // Manually alter window height and width
            this.SizeToContent = SizeToContent.Manual;

            // Automatically resize height relative to content
            this.SizeToContent = SizeToContent.Height;

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }


        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            MultiNotesMainWindow mainWindow = new MultiNotesMainWindow();
            this.Close();
            mainWindow.Show();
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            MultiNotesRegisterWindow registerWindow = new MultiNotesRegisterWindow();
            this.Close();
            registerWindow.Show();
        }
    }
}
