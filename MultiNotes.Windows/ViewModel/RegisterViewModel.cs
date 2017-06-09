using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using MultiNotes.Core;
using MultiNotes.Windows.Services;
using MultiNotes.Windows.View;

namespace MultiNotes.Windows.ViewModel
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        public RegisterViewModel(Action closeAction)
        {
            _closeAction = closeAction;
            SignUpCmd = new CommandHandler(SignUp);
        }

        private readonly Action _closeAction;
        public ICommand SignUpCmd { get; }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        private string _repeatEmail;
        public string RepeatEmail
        {
            get { return _repeatEmail; }
            set
            {
                _repeatEmail = value;
                OnPropertyChanged(nameof(RepeatEmail));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void SignUp(object parameter)
        {
            string passwordInVm = null;
            var passwordContainer = parameter as IHavePassword;
            if (passwordContainer != null)
            {
                var secureString = passwordContainer.Password;
                passwordInVm = PasswordService.ConvertToUnsecureString(secureString);
            }

            if (Email == RepeatEmail)
                MakeRegisterTask(Email, passwordInVm);
            else
            {
                MessageBox.Show("Adresy e-mail nie są takie same");
            }
        }

        public void MakeRegisterTask(string email, string password)
        {
            Register(email, password);
        }

        public async void Register(string email, string password)
        {
            ConnectionApi.Configure();
            var methods = new UserMethod(ConnectionApi.HttpClient);

            try
            {
                await methods.Register(email, password);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            var loginWindow = new MultiNotesLoginWindow();
            loginWindow.Show();
            _closeAction.Invoke();
        }
    }
}
