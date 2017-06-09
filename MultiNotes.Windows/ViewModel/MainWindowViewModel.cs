using MultiNotes.Core;
using MultiNotes.Model;
using MultiNotes.Windows.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MultiNotes.Windows.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly AuthenticationRecord _authenticationRecord;
        private readonly UserMethod _methods;
        private readonly List<Note> _singleNotes;
        private readonly List<SingleNoteWindow> _singleNoteWindows;
        private string _token;
        private NoteApi _noteApi;
        private static bool _ok;

        public User LoggedUser { get; private set; }
        public ICommand AddNoteCmd { get; }
        public ICommand DeleteNoteCmd { get; }
        public ICommand RefreshNotesCmd { get; }
        public ObservableCollection<Note> Notes { get; set; }

        private string _note;
        public string Note
        {
            get => _note;
            set
            {
                _note = value;
                OnPropertyChanged(nameof(Note));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public MainWindowViewModel()
        {
            AddNoteCmd = new CommandHandler(NewNote);
            DeleteNoteCmd = new CommandHandler(DelNote);
            RefreshNotesCmd = new CommandHandler(RefNotes);
            _methods = new UserMethod(ConnectionApi.HttpClient);
            _methods.PreparedAuthenticationRecord();
            _authenticationRecord = _methods.Record;
            Notes = new ObservableCollection<Note>();
            _singleNoteWindows = new List<SingleNoteWindow>();
            _singleNotes = new List<Note>();

            GetAllNotes(true);
        }

        private void ShowSingleNotes()
        {
            foreach (var note in Notes)
            {
                var window = new SingleNoteWindow(note);
                window.Show();
                window.SetBottom();
                _singleNoteWindows.Add(window);
            }
        }

        private void CloseSingleNotes()
        {
            foreach (var window in _singleNoteWindows)
            {
                window.Close();
            }
        }

        public async void AddNote(string note)
        {
            var newNote = new Note
            {
                Id = await UniqueId.GetUniqueBsonId(ConnectionApi.HttpClient),
                OwnerId = LoggedUser.Id,
                Content = note,
                CreateTimestamp = DateTime.UtcNow,
                LastChangeTimestamp = DateTime.UtcNow
            };

            _ok = false;

            if (_ok == false)
            {
                await _noteApi.AddNoteAsync(newNote);
                _ok = true;
            }

            Notes.Insert(0, newNote);
            // MessageBox.Show("Note added successfully!");
        }

        public async void GetAllNotes(bool showSingleNotes)
        {
            GetToken();
            LoggedUser = await _methods.GetUserInfo(_token, _authenticationRecord.Email);
            _noteApi = new NoteApi(_authenticationRecord, LoggedUser.Id);

            try
            {
                var tempNotes = await _noteApi.GetAllNotesAsync();
                var tempSortedNotes = tempNotes.OrderByDescending(o => o.CreateTimestamp).ToList();

                Notes.Clear();
                _singleNotes.Clear();

                foreach (var note in tempSortedNotes)
                {
                    Notes.Add(note);
                    _singleNotes.Add(note);
                }

                foreach (var note in Notes)
                {
                    note.CreateTimestamp = note.CreateTimestamp.ToLocalTime();
                    note.LastChangeTimestamp = note.LastChangeTimestamp.ToLocalTime();
                    if (showSingleNotes == true)
                    {
                        var window = new SingleNoteWindow(note);
                        window.Show();
                        _singleNoteWindows.Add(window);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An error has occured." + e.Message);
            }
        }

        protected virtual void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
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
            await _noteApi.DeleteNoteByIdAsync(id);

            var note = Notes.FirstOrDefault(s => s.Id == id);

            if (note != null)
            {
                Notes.Remove(note);
            }
            //MessageBox.Show("Note deleted successfully. Refresh application.");
        }

        private async void GetToken()
        {
            var authToken = new AuthenticationToken(ConnectionApi.HttpClient);
            _token = await authToken.PostAuthRecordAsync(_authenticationRecord);
        }
    }
}
