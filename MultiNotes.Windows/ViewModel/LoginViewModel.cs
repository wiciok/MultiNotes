using System;
using System.ComponentModel;
using MultiNotes.Core;
using System.Windows;
using System.Windows.Input;
using MultiNotes.Windows.View;
using System.Security;
using System.Runtime.InteropServices;

namespace MultiNotes.Windows.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public LoginViewModel()
        {
            LogInCmd = new RelayCommand(this.LogIn);
            SignUpCmd = new RelayCommand(pars => Signup());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public MultiNotesLoginWindow LoginWindow;
        public Action CloseAction { get; set; }

        public ICommand LogInCmd { get; set; }
        public ICommand SignUpCmd { get; set; }

        private string _Email;
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
                OnPropertyChanged("Email");
            }
        }


        //Metod zczytująca hasło jako parametr i tranformjąca je z Secure stringa na tekst

        private void LogIn(object parameter)
        {
            string PasswordInVM=null;
            var passwordContainer = parameter as IHavePassword;
            if (passwordContainer != null)
            {
                var secureString = passwordContainer.Password;
                PasswordInVM = RelayCommand.ConvertToUnsecureString(secureString);
            }

            Login(Email, PasswordInVM);
        }


        


        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
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
            catch(Exception exc)
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
        public void Signup()
        {
            MultiNotesRegisterWindow registerWindow = new MultiNotesRegisterWindow();
            Application.Current.MainWindow.Close();
            registerWindow.Show();
        }
    }
}
