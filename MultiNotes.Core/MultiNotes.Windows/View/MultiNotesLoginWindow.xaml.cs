using MultiNotes.Windows.ViewModel;
using System.Windows;
using MultiNotes.Core;
using MultiNotes.Windows.Services;

namespace MultiNotes.Windows.View
{
    /// <summary>
    /// Interaction logic for MultiNotesLoginWindow.xaml
    /// </summary>
    public partial class MultiNotesLoginWindow : IHavePassword
    {
        public MultiNotesLoginWindow()
        {
            InitializeComponent();
            ConnectionApi.Configure();
            var vm = new LoginViewModel(Close);
            DataContext = vm;

            // Manually alter window height and width
            SizeToContent = SizeToContent.Manual;
            // Automatically resize height relative to content
            SizeToContent = SizeToContent.Height;

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        public System.Security.SecureString Password => PassBox.SecurePassword;
    }
}
