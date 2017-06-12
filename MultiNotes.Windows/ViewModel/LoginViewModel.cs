using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using MultiNotes.Core;
using MultiNotes.Windows.Services;
using MultiNotes.Windows.View;

namespace MultiNotes.Windows.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
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

        public event PropertyChangedEventHandler PropertyChanged;

        public LoginViewModel(Action closeAction)
        {
            LogInCmd = new CommandHandler(LogIn);
            SignUpCmd = new CommandHandler(x => Signup());
            _closeAction = closeAction;
        }

        public async void Login(string email, string password, bool isPasswordHashed = false)
        {
            var methods = new UserMethod(ConnectionApi.HttpClient);

            try
            {
                await methods.Login(email, password, isPasswordHashed);

                var mainWindow = new MultiNotesMainWindow(true);
                mainWindow.Show();
                _closeAction.Invoke();
            }
            catch (Exception e)
            {
                //todo: tak nie moze zostac, tu musi byc sensowna obsluga wyjatkow roznych typow
                MessageBox.Show("Login Exception");
            }
        }
        public void Signup()
        {
            var registerWindow = new MultiNotesRegisterWindow();
            registerWindow.Show();
            _closeAction.Invoke();
        }


        private void LogIn(object parameter)
        {
            string passwordInVm = null;
            var passwordContainer = parameter as IHavePassword;
            if (passwordContainer != null)
            {
                var secureString = passwordContainer.Password;
                passwordInVm = PasswordService.ConvertToUnsecureString(secureString);
            }
            Login(Email, passwordInVm);
        }

        protected virtual void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
