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
            DeleteNoteCmd = new CommandHandler(DelNote);
            RefreshNotesCmd = new CommandHandler(RefNotes);

            _closeAction = closeAction;
            methods = new UserMethod(ConnectionApi.HttpClient);

            methods.PreparedAuthenticationRecord();
            _authenticationRecord = methods.Record;
            Notes = new ObservableCollection<Model.Note>();
            GetAllNotes();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Action _closeAction;
        private readonly AuthenticationRecord _authenticationRecord;
        private string token;
        private User user;
        private NoteApi noteApi;
        UserMethod methods;
        public ICommand AddNoteCmd { get; }
        public ICommand DeleteNoteCmd { get; }
        public ICommand RefreshNotesCmd { get; }

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

        public ObservableCollection<Note> Notes { get; set; }

        public async void GetAllNotes()
        {
            getToken();
            user = await methods.GetUserInfo(token, _authenticationRecord.Email);
            noteApi = new NoteApi(_authenticationRecord, user.Id);
            try
            {
                IEnumerable<Note> tempNotes = await noteApi.GetAllNotesAsync();
                IEnumerable<Note> tempSortedNotes = tempNotes.OrderByDescending<Note, DateTime>(o => o.CreateTimestamp).ToList();

                Notes.Clear();
                foreach (var item in tempSortedNotes)
                {
                    Notes.Add(item);
                }

                foreach (var item in Notes)
                {
                    item.CreateTimestamp = item.CreateTimestamp.ToLocalTime();
                    item.LastChangeTimestamp = item.LastChangeTimestamp.ToLocalTime();
                }              
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

        private void RefNotes(object parameter)
        {
            GetAllNotes();
        }

        private void DelNote(object parameter)
        {
            var id = parameter as string;
            MessageBox.Show(id);
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
            MessageBox.Show("Note added successfully!");
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
