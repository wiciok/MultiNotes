using MultiNotes.Windows.ViewModel;
using System.Windows;

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
