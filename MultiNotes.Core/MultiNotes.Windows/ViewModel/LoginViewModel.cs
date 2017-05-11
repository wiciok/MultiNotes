using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiNotes.Core;
using System.Web.Http;
using System.Windows;

namespace MultiNotes.Windows.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task Login()
        {
            ConnectionApi.configure();
            UserMethod methods = new UserMethod(ConnectionApi.httpClient);

            try
            {
                await methods.login("nowiutki", "nowiutki");
            }
            catch(HttpResponseException e)
            {
                if(e.Response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    MessageBox.Show("Error: NotFound");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
