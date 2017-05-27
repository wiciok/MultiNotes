using System;
using System.ComponentModel;
using MultiNotes.Core;
using System.Windows;
using MultiNotes.Windows.View;

namespace MultiNotes.Windows.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public MultiNotesLoginWindow LoginWindow;

        public LoginViewModel(MultiNotesLoginWindow loginWindow)
        {
            LoginWindow = loginWindow;
        }
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void MakeLoginTask(string email, string password)
        {
            Login(email, password);
        }

        public async void Login(string email, string password)
        {
            
            UserMethod methods = new UserMethod(ConnectionApi.HttpClient);

            try
            {
                    await methods.Login(email, password);
            }
            catch(Exception)
            {
                MessageBox.Show("Wrong username or password");
                return;
            }

            Application.Current.Dispatcher.Invoke(delegate
            {
                MultiNotesMainWindow mainWindow = new MultiNotesMainWindow();
                LoginWindow.Close();
                mainWindow.Show();
            });
        }
    }
}
