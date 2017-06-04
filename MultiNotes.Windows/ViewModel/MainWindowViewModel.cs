using MultiNotes.Core;
using MultiNotes.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MultiNotes.Windows.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel(Action closeAction)
        {
            _closeAction = closeAction;
        
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Action _closeAction;
        private readonly AuthenticationRecord _authenticationRecord;
        private ObservableCollection<Note> notes;

        public async void GetAllNotes()
        { 
            var noteApi = new NoteApi(_authenticationRecord, "id");
            try
            {
                IEnumerable<Note> tempNotes = await noteApi.GetAllNotesAsync();
                notes = new ObservableCollection<Note>(tempNotes);
            }
            catch (Exception e)
            {
                MessageBox.Show("An error has occured. {0}", e.Message);
                return;
            }
        }

        protected virtual void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }      
}
