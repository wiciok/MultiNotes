using MultiNotes.Windows.ViewModel;
using System.Windows;
using MultiNotes.Core;

namespace MultiNotes.Windows.View
{
    /// <summary>
    /// Interaction logic for MultiNotesLoginWindow.xaml
    /// </summary>
    public partial class MultiNotesLoginWindow
    {
        public MultiNotesLoginWindow()
        {
            InitializeComponent();
            ConnectionApi.Configure();


            // Manually alter window height and width
            SizeToContent = SizeToContent.Manual;

            // Automatically resize height relative to content
            SizeToContent = SizeToContent.Height;

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            var loginViewModel = new LoginViewModel(this);
            var email = EmailTextBox.Text;
            var password = PassBox.Password;
            loginViewModel.Login(email, password);
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            MultiNotesRegisterWindow registerWindow = new MultiNotesRegisterWindow();
            Close();
            registerWindow.Show();
        }
    }
}
