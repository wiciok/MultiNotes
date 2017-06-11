using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using MultiNotes.Core;
using MultiNotes.Model;
using MultiNotes.Windows.Services;
using MultiNotes.Windows.Util;
using MultiNotes.Windows.View;

namespace MultiNotes.Windows.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly List<SingleNoteWindow> _singleNoteWindows;
        private NoteApi _noteApi;

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
            AddNoteCmd = new CommandHandler(AddNote);
            DeleteNoteCmd = new CommandHandler(DeleteNote);
            RefreshNotesCmd = new CommandHandler(RefreshNotes);
            var methods = new UserMethod(ConnectionApi.HttpClient);
            Notes = new ObservableCollection<Note>();
            _singleNoteWindows = new List<SingleNoteWindow>();
            var authenticationRecord = methods.Record;

            methods.PreparedAuthenticationRecord();

#pragma warning disable 4014 //await warning disable - it's ok as it is now, don't change it!
            async void PrepareNotes()
            {
                var authToken = new AuthenticationToken(ConnectionApi.HttpClient);
                var token = await authToken.PostAuthRecordAsync(authenticationRecord);

                LoggedUser = await methods.GetUserInfo(token, authenticationRecord.Email);
                _noteApi = new NoteApi(authenticationRecord, LoggedUser.Id);
                await GetAllNotes();
                ShowAllNotes();
            }
            PrepareNotes();
#pragma warning restore 4014
        }

        private void ShowAllNotes()
        {
            foreach (var note in Notes)
            {
                note.CreateTimestamp = note.CreateTimestamp.ToLocalTime();
                note.LastChangeTimestamp = note.LastChangeTimestamp.ToLocalTime();

                var windowPref = NotesDisplayPreferences.Get(note.Id);

                SingleNoteWindow window;
                if (windowPref == null)
                {
                    window = new SingleNoteWindow(note)
                    {
                        MainGrid = { Background = NoteColors.ColorList[0] }
                    }; window.Show();
                    window.SetBottom();
                    _singleNoteWindows.Add(window);
                }
                else if (windowPref != null && windowPref.IsDisplayed == true)
                {
                    window = new SingleNoteWindow(note)
                    {
                        MainGrid = { Background = windowPref.WindowColor }
                    };
                    window.Show();
                    window.SetBottom();
                    _singleNoteWindows.Add(window);
                }
            }
        }

        private void CloseAllNotes()
        {
            foreach (var window in _singleNoteWindows)
            {
                window.Close();
            }
            _singleNoteWindows.Clear();
        }

        public async Task GetAllNotes()
        {
            try
            {
                var tempNotes = await _noteApi.GetAllNotesAsync();
                tempNotes = tempNotes.OrderByDescending(o => o.CreateTimestamp).ToList();

                Notes.Clear();

                foreach (var note in tempNotes)
                {
                    Notes.Add(note);
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

        private async void AddNote(object notUsed)
        {
            await AddNote(Note);
        }

        public async Task AddNote(string note)
        {
            var newNote = new Note
            {
                Id = await UniqueId.GetUniqueBsonId(ConnectionApi.HttpClient),
                OwnerId = LoggedUser.Id,
                Content = note,
                CreateTimestamp = DateTime.UtcNow,
                LastChangeTimestamp = DateTime.UtcNow
            };

            await _noteApi.AddNoteAsync(newNote);
            Notes.Insert(0, newNote);

            RefreshNotes(new object());
        }

        private async void RefreshNotes(object notUsed)
        {
            await RefreshNotes();
        }

        private async Task RefreshNotes()
        {
            await GetAllNotes();
            CloseAllNotes();
            ShowAllNotes();
        }

        private async void DeleteNote(object parameter)
        {
            await DeleteNote(parameter, 0);
            await RefreshNotes();
        }

        private async Task DeleteNote(object parameter, int notUsed)
        {
            var id = parameter as string;
            await _noteApi.DeleteNoteByIdAsync(id);

            var note = Notes.FirstOrDefault(s => s.Id == id);

            if (note != null)
                Notes.Remove(note);
        }
    }
}
