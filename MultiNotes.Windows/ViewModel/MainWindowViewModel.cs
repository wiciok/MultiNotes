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
using System.Windows.Input;

namespace MultiNotes.Windows.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel(Action closeAction)
        {

            AddNoteCmd = new CommandHandler(NewNote);

            _closeAction = closeAction;
            methods = new UserMethod(ConnectionApi.HttpClient);

            methods.PreparedAuthenticationRecord();
            _authenticationRecord = methods.Record;
            MessageBox.Show(_authenticationRecord.Email + " " + _authenticationRecord.PasswordHash);
            GetAllNotes();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Action _closeAction;
        private readonly AuthenticationRecord _authenticationRecord;
        private string token;
        private ObservableCollection<Note> notes;
        private User user;
        private NoteApi noteApi;
        UserMethod methods;
        public ICommand AddNoteCmd { get; }

        private string _note;
        public string Note
        {
            get
            {
                return _note;
            }
            set
            {
                _note = value;
                OnPropertyChanged(nameof(Note));
            }
        }

        public ObservableCollection<Note> Notes
        {
            get
            {
                return notes;
            }
        }

        public async void GetAllNotes()
        {
            getToken();
            MessageBox.Show(token);
            user = await methods.GetUserInfo(token, _authenticationRecord.Email);
            noteApi = new NoteApi(_authenticationRecord, user.Id);
            try
            {
                IEnumerable<Note> tempNotes = await noteApi.GetAllNotesAsync();
                notes = new ObservableCollection<Note>(tempNotes);
                for (int i = 0; i < notes.Count; i++)
                    MessageBox.Show(notes[i].Content);
            }
            catch (Exception e)
            {
                MessageBox.Show("An error has occured. {0}", e.Message);
                return;
            }
        }

        private void NewNote(object parameter)
        {
            AddNote(Note);
        }

        public async void AddNote(string note)
        {
            Note newNote = new Note();  
            newNote.Id = await UniqueId.GetUniqueBsonId(ConnectionApi.HttpClient);
            newNote.OwnerId = user.Id;
            newNote.Content = note;
            newNote.CreateTimestamp = DateTime.Now;
            newNote.LastChangeTimestamp = DateTime.Now;

            await noteApi.AddNoteAsync(newNote);
        }

        private async void getToken()
        {
            AuthenticationToken authToken = new AuthenticationToken(ConnectionApi.HttpClient);
            token = await authToken.PostAuthRecordAsync(_authenticationRecord);
        }

        protected virtual void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }      
}
