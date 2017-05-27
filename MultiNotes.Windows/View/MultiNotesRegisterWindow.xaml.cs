using MultiNotes.Windows.ViewModel;
using System.Windows;
using MultiNotes.Core;

namespace MultiNotes.Windows.View
{
    /// <summary>
    /// Interaction logic for MultiNotesRegisterWindow.xaml
    /// </summary>
    public partial class MultiNotesRegisterWindow: Window, IHavePassword
    {
        public MultiNotesRegisterWindow()
        {
            InitializeComponent();
            ConnectionApi.Configure();

            this.DataContext = new RegisterViewModel();

            // Manually alter window height and width
            SizeToContent = SizeToContent.Manual;

            // Automatically resize height relative to content
            SizeToContent = SizeToContent.Height;

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public System.Security.SecureString Password
        {
            get
            {
                return PassBox.SecurePassword;
            }
        }


        //private void registerBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    var loginWindow = new MultiNotesLoginWindow();
        //    var registerViewModel = new RegisterViewModel();

        //    if (EmailTextBox.Text == RepeatEmailTextBox.Text)
        //    {
        //        if(PassBox.Password == RepeatPassBox.Password)
        //        {
        //            registerViewModel.MakeRegisterTask(EmailTextBox.Text, PassBox.Password);
        //        }
        //    }
            
        //    Close();
        //    loginWindow.Show();
        //}
    }
}
