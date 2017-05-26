using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiNotes.Core;
using System.Web.Http;
using System.Windows;
using MultiNotes.Windows.View;

namespace MultiNotes.Windows.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public MultiNotesLoginWindow loginWindow;

        public LoginViewModel(MultiNotesLoginWindow loginWindow)
        {
            this.loginWindow = loginWindow;
        }
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void MakeLoginTask(string email, string password)
        {
            Login(email, password);
        }

        public async void Login(string email, string password)
        {
            
            UserMethod methods = new UserMethod(ConnectionApi.httpClient);

            try
            {
                    await methods.login(email, password);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Wrong username or password");
                return;
            }

            Application.Current.Dispatcher.Invoke(delegate
            {
                MultiNotesMainWindow mainWindow = new MultiNotesMainWindow();
                loginWindow.Close();
                mainWindow.Show();
            });
        }
    }
}
