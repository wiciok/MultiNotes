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
        UserMethod methods;

        public async void GetAllNotes()
        {
            getToken();
            MessageBox.Show(token);
            user = await methods.GetUserInfo(token, _authenticationRecord.Email);
            var noteApi = new NoteApi(_authenticationRecord, user.Id);
            try
            {
                IEnumerable<Note> tempNotes = await noteApi.GetAllNotesAsync();
                notes = new ObservableCollection<Note>(tempNotes);
                for(int i = 0; i < notes.Count; i++)
                {
                    MessageBox.Show(notes[i].Content + "\n");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An error has occured. {0}", e.Message);
                return;
            }
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
