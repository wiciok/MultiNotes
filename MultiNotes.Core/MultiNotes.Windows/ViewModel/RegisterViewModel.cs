using MultiNotes.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Windows;

namespace MultiNotes.Windows.ViewModel
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task Register()
        {
            ConnectionApi.configure();
            UserMethod methods = new UserMethod(ConnectionApi.httpClient);

            try
            {
                await methods.register("nowiutki", "nowiutki");
            }
            catch(Exception ex)
            {
                    MessageBox.Show(ex.Message);
            }
        }
    }
}
