using MultiNotes.Core;
using MultiNotes.Windows.ViewModel;
using System.Windows;
using MultiNotes.Windows.Util;

namespace MultiNotes.Windows.View
{
    /// <summary>
    /// Interaction logic for MultiNotesMainWindow.xaml
    /// </summary>
    public partial class MultiNotesMainWindow
    {
        private readonly MainWindowViewModel _vm;
        public MultiNotesMainWindow()
        {
            InitializeComponent();
            ConnectionApi.Configure();
            _vm = new MainWindowViewModel(Close);
            DataContext = _vm;

            MinimizeToTray.Enable(this);

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_vm.CloseFromLogoutFlag)
                _vm.CloseFromLogoutFlag = false;

            else
            {
                const string sMessageBoxText = "Are you sure you want to close MultiNotes?";
                const string sCaption = "Close";

                const MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                const MessageBoxImage icnMessageBox = MessageBoxImage.Question;

                var rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                switch (rsltMessageBox)
                {
                    case MessageBoxResult.Yes:
                        Application.Current.Shutdown();
                        break;

                    case MessageBoxResult.No:
                        e.Cancel = true;
                        break;
                }
            }            
        }
    }
}
