using System;
using MultiNotes.Windows.View;
using System.Windows;
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
            MultiNotesLoginWindow loginWindow = new MultiNotesLoginWindow();
            loginWindow.Show();
        }
    }
}
