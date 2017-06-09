using MultiNotes.Core;
using MultiNotes.Model;
using MultiNotes.Windows.View;
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
            singleNoteWindows = new List<SingleNoteWindow>();
            singleNotes = new List<Note>();
            GetAllNotes(true);
        }

        private void ShowSingleNotes()
        {
            foreach(var note in Notes)
            {
                var window = new SingleNoteWindow(note);
                window.Show();
                window.SetBottom();
                singleNoteWindows.Add(window);
            }
        }

        private void CloseSingleNotes()
        {
            foreach (var window in singleNoteWindows)
            {
                window.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Action _closeAction;
        private readonly AuthenticationRecord _authenticationRecord;
        private string token;
        public User LoggedUser { get; private set; }
        private NoteApi noteApi;

        private List<Note> singleNotes;
        private List<SingleNoteWindow> singleNoteWindows;

        UserMethod methods;
        public ICommand AddNoteCmd { get; }
        public ICommand DeleteNoteCmd { get; }
        public ICommand RefreshNotesCmd { get; }

        private static object lockObject = new object();
        private static bool ok;
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

        public async void GetAllNotes(bool showSingleNotes)
        {
            getToken();
            LoggedUser = await methods.GetUserInfo(token, _authenticationRecord.Email);
            noteApi = new NoteApi(_authenticationRecord, LoggedUser.Id);
            try
            {
                IEnumerable<Note> tempNotes = await noteApi.GetAllNotesAsync();
                IEnumerable<Note> tempSortedNotes = tempNotes.OrderByDescending<Note, DateTime>(o => o.CreateTimestamp).ToList();

                Notes.Clear();
                singleNotes.Clear();
                foreach (var note in tempSortedNotes)
                {
                    Notes.Add(note);
                    singleNotes.Add(note);
                }

                foreach (var note in Notes)
                {
                    note.CreateTimestamp = note.CreateTimestamp.ToLocalTime();
                    note.LastChangeTimestamp = note.LastChangeTimestamp.ToLocalTime();
                    if(showSingleNotes == true)
                    {
                        var window = new SingleNoteWindow(note);
                        window.Show();
                        singleNoteWindows.Add(window);
                    }
                }              
            }
            catch (Exception e)
            {
                MessageBox.Show("An error has occured." + e.Message);
                return;
            }
        }

        private void NewNote(object parameter)
        {
            AddNote(Note);
        }

        private void RefNotes(object parameter)
        {
            GetAllNotes(false);
            CloseSingleNotes();
            ShowSingleNotes();
        }

        private async void DelNote(object parameter)
        {
            var id = parameter as string;
            await noteApi.DeleteNoteByIdAsync(id);

            var note = Notes.FirstOrDefault(s => s.Id == id);

            if (note != null)
            {
                Notes.Remove(note);
            }

            //MessageBox.Show("Note deleted successfully. Refresh application.");
        }

        public async void AddNote(string note)
        {
            Note newNote = new Note();  
            newNote.Id = await UniqueId.GetUniqueBsonId(ConnectionApi.HttpClient);
            newNote.OwnerId = LoggedUser.Id;
            newNote.Content = note;
            newNote.CreateTimestamp = DateTime.UtcNow;
            newNote.LastChangeTimestamp = DateTime.UtcNow;
            ok = false;
            //lock (newNote)
            //{
            //}
            if (ok == false)
            {
                await noteApi.AddNoteAsync(newNote);
                ok = true;
            }


            Notes.Insert(0, newNote);
           // MessageBox.Show("Note added successfully!");
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
