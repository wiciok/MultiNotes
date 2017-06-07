using System;
using MultiNotes.Windows.View;
using System.Windows;
using MultiNotes.Core;
using MultiNotes.Model;

namespace MultiNotes.Windows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ConnectionApi.Configure();

            MakeLoginTask();
        }

        public async void MakeLoginTask()
        {
            try
            {
                UserMethod methods = new UserMethod(ConnectionApi.HttpClient);
                methods.PreparedAuthenticationRecord();

                await methods.Login(methods.Record.Email, methods.Record.PasswordHash, true);

                MultiNotesMainWindow mainWindow = new MultiNotesMainWindow();
                mainWindow.Show();
            }
            catch (Exception e)
            {
                MultiNotesLoginWindow loginWindow = new MultiNotesLoginWindow();
                loginWindow.Show();
            }
        }
    }
}
