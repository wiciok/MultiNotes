using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MultiNotes.Core;
using MultiNotes.Model;
using MultiNotes.Windows.Services;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace MultiNotes.Windows.ViewModel
{
    public sealed class SingleNoteWindowViewModel : INotifyPropertyChanged
    {
        private User _user;
        private NoteApi _noteApi;
        private readonly UserMethod _methods;
        private string _token;
        private readonly AuthenticationRecord _authenticationRecord;
        public string DisplayedDate { get; }

        #region Commands
        public ICommand EditNoteCmd { get; }
        public ICommand SaveNoteCmd { get; }
        public ICommand ChangeNoteColorCmd { get; }
        #endregion

        public Note Note { get; set; }
        public Visibility IsSaveButtonVisible { get; private set; }
        public Visibility IsEditButtonVisible { get; private set; }
        public bool IsReadOnly { get; private set; }
        public System.Windows.Media.Brush NoteColor { get; private set; }
        public double NoteWidth { get; set; } = 100;
        public double NoteHeight { get; set; } = 250;
        public double NotePositionX { get;  set; } = 100;
        public double NotePositionY { get;  set; } = 100;

        public event PropertyChangedEventHandler PropertyChanged;

        public SingleNoteWindowViewModel(Note note)
        {
            _methods = new UserMethod(ConnectionApi.HttpClient);
            _methods.PreparedAuthenticationRecord();
            _authenticationRecord = _methods.Record;

            Note = note;
            DisplayedDate = Note.CreateTimestamp.ToShortDateString();

            NoteWindowPreferences notePreferences = NotesDisplayPreferences.Get(Note.Id);

            if (notePreferences != null)
            {
                NoteWidth = notePreferences.WindowWidth;
                NoteHeight = notePreferences.WindowHeight;
                NotePositionX = notePreferences.WindowPositionX;
                NotePositionY = notePreferences.WindowPositionY;
                NoteColor = notePreferences.WindowColor;
            }

            OnPropertyChanged(nameof(NoteWidth));
            OnPropertyChanged(nameof(NoteHeight));

            EditNoteCmd = new CommandHandler(EditNote);
            SaveNoteCmd = new CommandHandler(SaveNote);
            ChangeNoteColorCmd = new CommandHandler(ChangeNoteColor);
            IsSaveButtonVisible = Visibility.Collapsed;
            IsEditButtonVisible = Visibility.Visible;
            IsReadOnly = true;
            OnPropertyChanged(nameof(IsReadOnly));
            OnPropertyChanged(nameof(IsSaveButtonVisible));
            OnPropertyChanged(nameof(IsEditButtonVisible));
        }

        private void ChangeNoteColor(object obj)
        {
            MenuItem item = obj as MenuItem;

            Rectangle rectangle = item.Header as Rectangle;
            NoteColor = rectangle.Fill;

            SaveNotePreferences();
        }

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void EditNote(object obj)
        {
            IsReadOnly = false;
            IsSaveButtonVisible = Visibility.Visible;
            IsEditButtonVisible = Visibility.Hidden;
            OnPropertyChanged(nameof(IsSaveButtonVisible));
            OnPropertyChanged(nameof(IsEditButtonVisible));
            OnPropertyChanged(nameof(IsReadOnly));
        }

        private async void SaveNote(object obj)
        {
            IsReadOnly = true;
            IsSaveButtonVisible = Visibility.Collapsed;
            IsEditButtonVisible = Visibility.Visible;

            var authToken = new AuthenticationToken(ConnectionApi.HttpClient);
            _token = await authToken.PostAuthRecordAsync(_authenticationRecord);
            _user = await _methods.GetUserInfo(_token, _authenticationRecord.Email);
            _noteApi = new NoteApi(_authenticationRecord, _user.Id);

            await _noteApi.UpdateNoteAsync(Note.Id, Note);

            OnPropertyChanged(nameof(IsSaveButtonVisible));
            OnPropertyChanged(nameof(IsEditButtonVisible));
            OnPropertyChanged(nameof(IsReadOnly));

            SaveNotePreferences();
        }

        private void SaveNotePreferences()
        {
            var windowPref = new NoteWindowPreferences()
            {
                WindowColor = NoteColor,
                IsDisplayed = true,     //na razie, dopóki nie ma zaimplementowanego wl/wyl
                WindowHeight = NoteHeight,
                WindowWidth = NoteWidth,
                WindowPositionX = NotePositionX,
                WindowPositionY = NotePositionY
            };

            NotesDisplayPreferences.Add(Note.Id, windowPref);
            NotesDisplayPreferences.SaveToDisc();
        }
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            SaveNotePreferences();
        }
    }
}
