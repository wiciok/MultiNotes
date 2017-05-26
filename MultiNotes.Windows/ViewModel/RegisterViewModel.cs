using MultiNotes.Core;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace MultiNotes.Windows.ViewModel
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void MakeRegisterTask(string email, string password)
        {
            Task testTask = new Task(() => Register(email, password).Wait());
            testTask.Start();
        }

        public async Task Register(string email, string password)
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
        }
    }
}
