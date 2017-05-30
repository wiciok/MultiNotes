using MultiNotes.Windows.ViewModel;
using System.Windows;
using MultiNotes.Core;

namespace MultiNotes.Windows.View
{
    /// <summary>
    /// Interaction logic for MultiNotesRegisterWindow.xaml
    /// </summary>
    public partial class MultiNotesRegisterWindow: IHavePassword
    {
        public MultiNotesRegisterWindow()
        {
            InitializeComponent();
            ConnectionApi.Configure();

            DataContext = new RegisterViewModel(Close);


            // Manually alter window height and width
            SizeToContent = SizeToContent.Manual;
            // Automatically resize height relative to content
            SizeToContent = SizeToContent.Height;

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public System.Security.SecureString Password => PassBox.SecurePassword;
    }
}
