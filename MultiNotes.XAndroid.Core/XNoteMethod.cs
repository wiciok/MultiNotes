using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using MultiNotes.Model;

namespace MultiNotes.XAndroid.Core
{
    public class XNoteMethod
    {
        public List<Note> GetAllLocalNotes()
        {
            return new LocalNoteRepository().GetAllNotes();
        }


        public void AddLocalNote(Note note)
        {
            new LocalNoteRepository().AddNote(note);
        }


        public void UpdateLocalNoteById(string noteId, Note note)
        {
            
        }


        public void UpdateLocalNote(Note note)
        {
            new LocalNoteRepository().UpdateNote(note);
        }


        public void DeleteLocalNote(Note note)
        {
            new LocalNoteRepository().DeleteNote(note);
        }


        public List<Note> FetchAllLocalNotes()
        {
            return new LocalNoteRepository().FetchAllNotes();
        }


        /// <exception cref="WebApiClientException"></exception>
        /// <exception cref="UserNotSignedException"></exception>
        public List<Note> GetAllRemoteNotes(string token)
        {
            return new RemoteNoteRepository().GetAllNotes(token);
        }


        /// <exception cref="WebApiClientException"></exception>
        /// <exception cref="UserNotSignedException"></exception>
        public void AddRemoteNote(Note note, string token)
        {
            new RemoteNoteRepository().AddNote(note, token);
        }


        /// <exception cref="WebApiClientException"></exception>
        /// <exception cref="UserNotSignedException"></exception>
        public void UpdateRemoteNote(Note note, string token)
        {
            new RemoteNoteRepository().UpdateNote(note, token);
        }


        /// <exception cref="WebApiClientException"></exception>
        /// <exception cref="UserNotSignedException"></exception>
        public void DeleteRemoteNote(Note note, string token)
        {
            new RemoteNoteRepository().DeleteNote(note, token);
        }
    }
}
