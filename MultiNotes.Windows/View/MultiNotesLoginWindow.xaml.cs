using MultiNotes.Windows.ViewModel;
using System.Windows;
using MultiNotes.Core;

namespace MultiNotes.Windows.View
{
    /// <summary>
    /// Interaction logic for MultiNotesLoginWindow.xaml
    /// </summary>
    public partial class MultiNotesLoginWindow: Window,IHavePassword
    {
        public MultiNotesLoginWindow()
        {
            InitializeComponent();
            ConnectionApi.Configure();
            LoginViewModel vm = new LoginViewModel();
            this.DataContext = vm;
            if (vm.CloseAction == null)
                vm.CloseAction = new System.Action(this.Close);

            this.DataContext = new LoginViewModel();

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
        //    MultiNotesRegisterWindow registerWindow = new MultiNotesRegisterWindow();
        //    Close();
        //    registerWindow.Show();
        //}
    }
}
