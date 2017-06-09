using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using MultiNotes.Core;
using MultiNotes.Model;

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

        public ICommand EditNoteCmd { get; }
        public ICommand SaveNoteCmd { get; }
        public Note Note { get; set; }
        public Visibility IsSaveButtonVisible { get; private set; }
        public Visibility IsEditButtonVisible { get; private set; }
        public bool IsReadOnly { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public SingleNoteWindowViewModel(Note note)
        {
            _methods = new UserMethod(ConnectionApi.HttpClient);
            _methods.PreparedAuthenticationRecord();
            _authenticationRecord = _methods.Record;

            Note = note;
            DisplayedDate = Note.CreateTimestamp.ToShortDateString();

            EditNoteCmd = new CommandHandler(EditNote);
            SaveNoteCmd = new CommandHandler(SaveNote);
            IsSaveButtonVisible = Visibility.Hidden;
            IsEditButtonVisible = Visibility.Visible;
            IsReadOnly = true;
            OnPropertyChanged(nameof(IsReadOnly));
            OnPropertyChanged(nameof(IsSaveButtonVisible));
            OnPropertyChanged(nameof(IsEditButtonVisible));
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
            OnPropertyChanged(nameof(IsSaveButtonVisible));
            OnPropertyChanged(nameof(IsReadOnly));
        }

        private async void SaveNote(object obj)
        {
            IsReadOnly = true;
            IsSaveButtonVisible = Visibility.Hidden;
            IsEditButtonVisible = Visibility.Visible;

            var authToken = new AuthenticationToken(ConnectionApi.HttpClient);
            _token = await authToken.PostAuthRecordAsync(_authenticationRecord);
            _user = await _methods.GetUserInfo(_token, _authenticationRecord.Email);
            _noteApi = new NoteApi(_authenticationRecord, _user.Id);

            await _noteApi.UpdateNoteAsync(Note.Id, Note);

            OnPropertyChanged(nameof(IsSaveButtonVisible));
            OnPropertyChanged(nameof(IsSaveButtonVisible));
            OnPropertyChanged(nameof(IsReadOnly));
        }
    }
}
