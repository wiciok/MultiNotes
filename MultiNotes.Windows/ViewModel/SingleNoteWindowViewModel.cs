using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MultiNotes.Core;
using MultiNotes.Model;
using MultiNotes.Windows.Annotations;

namespace MultiNotes.Windows.ViewModel
{
    //todo: po zmianie notatki trzeba wywolac update z noteapi!!!

    public class SingleNoteWindowViewModel : INotifyPropertyChanged
    {
        public ICommand EditNoteCmd { get; }
        public ICommand SaveNoteCmd { get; }
        public Note Note { get; set; }
        public string DisplayedDate => Note.CreateTimestamp.ToShortDateString(); //todo: mozna to zmienic czy cos

        public Visibility IsSaveButtonVisible { get; private set; }
        public Visibility IsEditButtonVisible { get; private set; }

        public bool IsReadOnly { get; private set; }

        private User user;
        private NoteApi noteApi;
        UserMethod methods;
        private string token;
        private readonly AuthenticationRecord _authenticationRecord;

        public SingleNoteWindowViewModel(Action<object> closeAction, Note note)
        {
            methods = new UserMethod(ConnectionApi.HttpClient);

            methods.PreparedAuthenticationRecord();
            _authenticationRecord = methods.Record;

            Note = note;
            EditNoteCmd = new CommandHandler(EditNote);
            SaveNoteCmd = new CommandHandler(SaveNote);
            IsSaveButtonVisible = Visibility.Hidden;
            IsEditButtonVisible = Visibility.Visible;
            IsReadOnly = true;
            OnPropertyChanged(nameof(IsReadOnly));
            OnPropertyChanged(nameof(IsSaveButtonVisible));
            OnPropertyChanged(nameof(IsEditButtonVisible));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propName)
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

            AuthenticationToken authToken = new AuthenticationToken(ConnectionApi.HttpClient);
            token = await authToken.PostAuthRecordAsync(_authenticationRecord);
            user = await methods.GetUserInfo(token, _authenticationRecord.Email);
            noteApi = new NoteApi(_authenticationRecord, user.Id);

            await noteApi.UpdateNoteAsync(Note.Id, Note);

            OnPropertyChanged(nameof(IsSaveButtonVisible));
            OnPropertyChanged(nameof(IsSaveButtonVisible));
            OnPropertyChanged(nameof(IsReadOnly));
        }
    }
}
