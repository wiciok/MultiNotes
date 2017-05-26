using MultiNotes.Windows.ViewModel;
using System.Windows;
using MultiNotes.Core;

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
            ConnectionApi.configure();


            // Manually alter window height and width
            this.SizeToContent = SizeToContent.Manual;

            // Automatically resize height relative to content
            this.SizeToContent = SizeToContent.Height;

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            LoginViewModel loginViewModel = new LoginViewModel(this);
            string email = this.emailTextBox.Text;
            string password = this.passBox.Password;
            loginViewModel.Login(email, password);
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            MultiNotesRegisterWindow registerWindow = new MultiNotesRegisterWindow();
            this.Close();
            registerWindow.Show();
        }
    }
}
