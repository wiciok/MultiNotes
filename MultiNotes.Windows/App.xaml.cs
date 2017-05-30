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

            //todo: usunąć to potem, teraz jest tylko w celach testowych
            string loremIpsum =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ultricies diam id tellus ullamcorper, ac pulvinar mi venenatis. Quisque sed gravida purus. Etiam id tortor eros. Vivamus vel pretium mauris, id tincidunt lectus. Nullam a bibendum dolor. Phasellus iaculis arcu massa, nec egestas nibh efficitur eu. Pellentesque ornare ullamcorper sem sed sagittis. ";
            SingleNoteWindow singleNoteWindow=new SingleNoteWindow(new Note {Content = loremIpsum, CreateTimestamp = DateTime.Now});
            singleNoteWindow.Show();
        }
    }
}
