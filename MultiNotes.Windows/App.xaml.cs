using System;
using MultiNotes.Windows.View;
using System.Windows;
using MultiNotes.Core;
using MultiNotes.Core.Util;

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
                var methods = new UserMethod(ConnectionApi.HttpClient);
                if (methods.PrepareAuthenticationRecord())
                {
                    MultiNotesMainWindow mainWindow;
                    if (await InternetConnection.IsInternetConnectionAvailable()) //online
                    {
                        await methods.Login(methods.Record.Email, methods.Record.PasswordHash, true);
                        mainWindow = new MultiNotesMainWindow(true);
                    }
                    else //offline
                        mainWindow = new MultiNotesMainWindow(false);    
                    
                    mainWindow.Show();
                }
                else    //no saved user data in file, or incorrect email address
                {
                    var loginWindow = new MultiNotesLoginWindow();
                    loginWindow.Show();
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Login failed!");
                var loginWindow = new MultiNotesLoginWindow();
                loginWindow.Show();
            }
        }
    }
}
