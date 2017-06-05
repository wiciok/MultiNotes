using MultiNotes.Core;
using MultiNotes.Windows.ViewModel;
using System;
using System.Windows;
using System.Windows.Media;
using MultiNotes.Windows.Util;

namespace MultiNotes.Windows.View
{
    /// <summary>
    /// Interaction logic for MultiNotesMainWindow.xaml
    /// </summary>
    public partial class MultiNotesMainWindow
    {
        public MultiNotesMainWindow()
        {
            InitializeComponent();
            ConnectionApi.Configure();
            var vm = new MainWindowViewModel(Close);
            DataContext = vm;

            MinimizeToTray.Enable(this);

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            /*if(WindowState == WindowState.Minimized)
            {
                SingleNoteWindow note = new SingleNoteWindow();
                note.Show();
            }*/
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string sMessageBoxText = "Are you sure you want to close MultiNotes?";
            string sCaption = "Close";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Question;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

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
