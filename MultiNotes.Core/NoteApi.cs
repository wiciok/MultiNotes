using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultiNotes.Model;
using MultiNotes.Core.Util;

namespace MultiNotes.Core
{
    public class NoteApi : INoteApi
    {
        private readonly string _userId;
        private readonly AuthenticationRecord _authenticationRecord;
        private readonly INoteMethod _noteMethod;

        public NoteApi(AuthenticationRecord authenticationRecord, string userId)
        {
            _authenticationRecord = authenticationRecord;
            _userId = userId;
            _noteMethod = new NoteMethod(ConnectionApi.HttpClient);
        }

        public async Task AddNoteAsync(Note note)
        {
            _noteMethod.AddNoteToFile(note);
            IEnumerable<Note> localNotes = _noteMethod.GetAllNotesFromFile(_userId);

            if (await InternetConnection.IsInternetConnectionAvailable())
            {
                var token = await new AuthenticationToken(ConnectionApi.HttpClient).PostAuthRecordAsync(_authenticationRecord);
                await _noteMethod.AddNoteToDatabase(note, token);
                var remoteNotes = await _noteMethod.GetAllNotesFromDatabase(token);

                await UpdateNoteDatabase(remoteNotes, localNotes, token);
            }
        }

        public async Task<Note> GetNoteByIdAsync(string id)
        {
            IEnumerable<Note> localNotes = _noteMethod.GetAllNotesFromFile(_userId);

            if (!await InternetConnection.IsInternetConnectionAvailable())
                return _noteMethod.GetNoteFromFile(id, _userId);
            var token = await new AuthenticationToken(ConnectionApi.HttpClient).PostAuthRecordAsync(_authenticationRecord);
            var remoteNotes = await _noteMethod.GetAllNotesFromDatabase(token);

            await UpdateNoteDatabase(remoteNotes, localNotes, token);
            return _noteMethod.GetNoteFromFile(id, _userId);
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync()
        {
            IEnumerable<Note> localNotes = _noteMethod.GetAllNotesFromFile(_userId);

            if (!await InternetConnection.IsInternetConnectionAvailable())
                return localNotes;
            var token = await new AuthenticationToken(ConnectionApi.HttpClient).PostAuthRecordAsync(_authenticationRecord);
            var remoteNotes = await _noteMethod.GetAllNotesFromDatabase(token);

            await UpdateNoteDatabase(remoteNotes, localNotes, token);

            return remoteNotes;
        }

        private async Task UpdateNoteDatabase(IEnumerable<Note> remoteNotes, IEnumerable<Note> localNotes, string token)
        {
            foreach (var remoteNote in remoteNotes)
            {
                foreach (var localNote in localNotes)
                {
                    // Update notes that changed and the newer version is stored on server
                    if (remoteNote.Id == localNote.Id && localNote.LastChangeTimestamp < remoteNote.LastChangeTimestamp)
                    {
                        _noteMethod.UpdateNoteFromFile(remoteNote.Id, _userId, remoteNote);
                    }
                    // Update notes that changed and the newer version is stored locally
                    if (remoteNote.Id == localNote.Id && localNote.LastChangeTimestamp > remoteNote.LastChangeTimestamp)
                    {
                        await _noteMethod.UpdateNoteByIdFromDatabase(token, localNote.Id, localNote);
                    }
                    // Update notes that exist only locally 
                    if (!ContainsNoteById(remoteNote.Id, localNotes))
                    {
                        _noteMethod.AddNoteToFile(localNote);
                    }
                    // Update notes that exist only on server
                    if (!ContainsNoteById(localNote.Id, remoteNotes))
                    {
                        await _noteMethod.AddNoteToDatabase(remoteNote, token);
                    }
                }
            }
            //_noteMethod.CleanLocalNotes();
        }

        private static bool ContainsNoteById(string noteId, IEnumerable<Note> notes)
        {
            return notes.Any(note => note.Id.Equals(noteId));
        }

        public async Task DeleteNoteByIdAsync(string id)
        {
            _noteMethod.DeleteNoteFromFile(id, _userId);
            IEnumerable<Note> localNotes = _noteMethod.GetAllNotesFromFile(_userId);

            if (await InternetConnection.IsInternetConnectionAvailable())
            {
                var token = await new AuthenticationToken(ConnectionApi.HttpClient).PostAuthRecordAsync(_authenticationRecord);
                await _noteMethod.DeleteNoteByIdFromDatabase(token, id);
                var remoteNotes = await _noteMethod.GetAllNotesFromDatabase(token);

                await UpdateNoteDatabase(remoteNotes, localNotes, token);
            }
        }

        public async Task UpdateNoteAsync(string id, Note newNote)
        {
            _noteMethod.UpdateNoteFromFile(id, _userId, newNote);

            IEnumerable<Note> localNotes = _noteMethod.GetAllNotesFromFile(_userId);

            if (await InternetConnection.IsInternetConnectionAvailable())
            {
                var token = await new AuthenticationToken(ConnectionApi.HttpClient).PostAuthRecordAsync(_authenticationRecord);
                var remoteNotes = await _noteMethod.GetAllNotesFromDatabase(token);

                await UpdateNoteDatabase(remoteNotes, localNotes, token);
            }
        }
    }
}
