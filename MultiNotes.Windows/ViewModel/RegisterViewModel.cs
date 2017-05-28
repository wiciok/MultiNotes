using System;
using System.ComponentModel;
using MultiNotes.Core;
using System.Windows;
using System.Windows.Input;
using MultiNotes.Windows.View;
using System.Security;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MultiNotes.Windows.ViewModel
{
    public class RegisterViewModel : INotifyPropertyChanged
    {


        public RegisterViewModel()
        {
            SignUpCmd= new RelayCommand(this.SignUp);

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public MultiNotesRegisterWindow LoginWindow;
        public Action CloseAction { get; set; }

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
        private string _RepeatEmail;
        public string RepeatEmail
        {
            get
            {
                return _RepeatEmail;
            }
            set
            {
                _RepeatEmail = value;
                OnPropertyChanged("RepeatEmail");
            }
        }

        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private void SignUp(object parameter)
        {
            string PasswordInVM = null;
            var passwordContainer = parameter as IHavePassword;
            if (passwordContainer != null)
            {
                var secureString = passwordContainer.Password;
                PasswordInVM = RelayCommand.ConvertToUnsecureString(secureString);
            }

            if (Email == RepeatEmail)
            {
                MakeRegisterTask(Email, PasswordInVM);
            }
            else
            {
                MessageBox.Show("Adresy e-mail nie są takie same");
            }
        }


        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void MakeRegisterTask(string email, string password)
        {
            Register(email, password);
        }

        public async void Register(string email, string password)
        {
            ConnectionApi.Configure();
            UserMethod methods = new UserMethod(ConnectionApi.HttpClient);

            try
            {
                await methods.Register(email, password);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            MultiNotesLoginWindow LoginWindow = new MultiNotesLoginWindow();
            LoginWindow.Show();
            
        }
    }
}
