using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using MultiNotes.Core;
using MultiNotes.Windows.View;

namespace MultiNotes.Windows.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public LoginViewModel(Action closeAction)
        {
            LogInCmd = new RelayCommand(LogIn);
            SignUpCmd = new RelayCommand(pars => Signup());
            _closeAction = closeAction;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly Action _closeAction;
        public ICommand LogInCmd { get; }
        public ICommand SignUpCmd { get; }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        //Metod zczytująca hasło jako parametr i tranformjąca je z Secure stringa na tekst
        private void LogIn(object parameter)
        {
            string passwordInVm = null;
            var passwordContainer = parameter as IHavePassword;
            if (passwordContainer != null)
            {
                var secureString = passwordContainer.Password;
                passwordInVm = RelayCommand.ConvertToUnsecureString(secureString);
            }
            Login(Email, passwordInVm);
        }


        protected virtual void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public void MakeLoginTask(string email, string password)
        {
            Login(email, password);
        }

        public async void Login(string email, string password)
        {
            var methods = new UserMethod(ConnectionApi.HttpClient);

            try
            {
                await methods.Login(email, password);
            }
            catch (Exception e)
            {
                //todo: tak nie moze zostac, tu musi byc sensowna obsluga wyjatkow roznych typow
                MessageBox.Show("Wrong username or password");
                return;
            }

            var mainWindow = new MultiNotesMainWindow();
            mainWindow.Show();
            _closeAction.Invoke();
        }
        public void Signup()
        {
            var registerWindow = new MultiNotesRegisterWindow();
            Application.Current.MainWindow.Close();
            registerWindow.Show();
        }
    }
}
