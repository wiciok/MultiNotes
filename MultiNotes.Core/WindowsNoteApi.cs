using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiNotes.Model;
using MultiNotes.Core.Util;

namespace MultiNotes.Core
{
    class WindowsNoteApi : INoteApi
    {
        private string _userId;
        private AuthenticationRecord _authenticationRecord;
        private INoteMethod _windowsNoteMethod;

        public WindowsNoteApi(AuthenticationRecord authenticationRecord, string userId)
        {
            this._authenticationRecord = authenticationRecord;
            this._userId = userId;
            _windowsNoteMethod = new NoteMethod(ConnectionApi.HttpClient);
        }
        public async Task AddNoteAsync(Note note)
        {
            _windowsNoteMethod.AddNoteToFile(note);
            IEnumerable<Note> localNotes = _windowsNoteMethod.GetAllNotesFromFile(_userId);

            if (await InternetConnection.IsInternetConnectionAvailable())
            {
                string token = await new AuthenticationToken(ConnectionApi.HttpClient).PostAuthRecordAsync(_authenticationRecord);
                IEnumerable<Note> remoteNotes = await _windowsNoteMethod.GetAllNotesFromDatabase(token);

                UpdateNoteDatabase(remoteNotes, localNotes, token);
            }
        }

        public async Task<Note> GetNoteByIdAsync(string id)
        {
            IEnumerable<Note> localNotes = _windowsNoteMethod.GetAllNotesFromFile(_userId);

            if (await InternetConnection.IsInternetConnectionAvailable())
            {
                string token = await new AuthenticationToken(ConnectionApi.HttpClient).PostAuthRecordAsync(_authenticationRecord);
                IEnumerable<Note> remoteNotes = await _windowsNoteMethod.GetAllNotesFromDatabase(token);

                UpdateNoteDatabase(remoteNotes, localNotes, token);
            }
            return _windowsNoteMethod.GetNoteFromFile(id, _userId);
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync()
        {
            IEnumerable<Note> localNotes = _windowsNoteMethod.GetAllNotesFromFile(_userId);

            if (await InternetConnection.IsInternetConnectionAvailable())
            {
                string token = await new AuthenticationToken(ConnectionApi.HttpClient).PostAuthRecordAsync(_authenticationRecord);
                IEnumerable<Note> remoteNotes = await _windowsNoteMethod.GetAllNotesFromDatabase(token);

                UpdateNoteDatabase(remoteNotes, localNotes, token);

                return remoteNotes;
            }
            return localNotes;
        }

        private void UpdateNoteDatabase(IEnumerable<Note> remoteNotes, IEnumerable<Note> localNotes, string token)
        {
            foreach (var remoteNote in remoteNotes)
            {
                foreach (var localNote in localNotes)
                {
                    // Update notes that changed and the newer version is stored on server
                    if (remoteNote.Id == localNote.Id && localNote.LastChangeTimestamp < remoteNote.LastChangeTimestamp)
                    {
                        _windowsNoteMethod.UpdateNoteFromFile(remoteNote.Id, _userId, remoteNote);
                    }
                    // Update notes that changed and the newer version is stored locally
                    if (remoteNote.Id == localNote.Id && localNote.LastChangeTimestamp > remoteNote.LastChangeTimestamp)
                    {
                        _windowsNoteMethod.UpdateNoteByIdFromDatabase(token, localNote.Id, localNote);
                    }
                    // Update notes that exist only locally 
                    if (!ContainsNoteById(remoteNote.Id, localNotes))
                    {
                        _windowsNoteMethod.AddNoteToDatabase(remoteNote, token);
                    }
                    // Update notes that exist only on server
                    if (!ContainsNoteById(localNote.Id, remoteNotes))
                    {
                        _windowsNoteMethod.AddNoteToFile(localNote);
                    }
                }
            }
        }

        private bool ContainsNoteById(string noteId, IEnumerable<Note> notes)
        {
            foreach (var note in notes)
            {
                if (note.Id == noteId)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task DeleteNoteByIdAsync(string id)
        {
            _windowsNoteMethod.DeleteNoteFromFile(id, _userId);
            IEnumerable<Note> localNotes = _windowsNoteMethod.GetAllNotesFromFile(_userId);

            if (await InternetConnection.IsInternetConnectionAvailable())
            {
                string token = await new AuthenticationToken(ConnectionApi.HttpClient).PostAuthRecordAsync(_authenticationRecord);
                IEnumerable<Note> remoteNotes = await _windowsNoteMethod.GetAllNotesFromDatabase(token);
                await _windowsNoteMethod.DeleteNoteByIdFromDatabase(token, id);

                UpdateNoteDatabase(remoteNotes, localNotes, token);
            }
        }

        public async Task UpdateNoteAsync(string id, Note newNote)
        {
            _windowsNoteMethod.UpdateNoteFromFile(id, _userId, newNote);

            IEnumerable<Note> localNotes = _windowsNoteMethod.GetAllNotesFromFile(_userId);

            if (await InternetConnection.IsInternetConnectionAvailable())
            {
                string token = await new AuthenticationToken(ConnectionApi.HttpClient).PostAuthRecordAsync(_authenticationRecord);
                IEnumerable<Note> remoteNotes = await _windowsNoteMethod.GetAllNotesFromDatabase(token);

                UpdateNoteDatabase(remoteNotes, localNotes, token);
            }
        }
    }
}
