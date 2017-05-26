using MultiNotes.Windows.ViewModel;
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
    /// Interaction logic for MultiNotesRegisterWindow.xaml
    /// </summary>
    public partial class MultiNotesRegisterWindow : Window
    {
        public MultiNotesRegisterWindow()
        {
            InitializeComponent();
            // Manually alter window height and width
            this.SizeToContent = SizeToContent.Manual;

            // Automatically resize height relative to content
            this.SizeToContent = SizeToContent.Height;

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            MultiNotesLoginWindow loginWindow = new MultiNotesLoginWindow();
            RegisterViewModel registerViewModel = new RegisterViewModel();

            if (emailTextBox.Text == repeatEmailTextBox.Text)
            {
                if(passBox.Password == repeatPassBox.Password)
                {
                    registerViewModel.MakeRegisterTask(emailTextBox.Text, passBox.Password);
                }
            }
            
            this.Close();
            loginWindow.Show();
        }
    }
}
