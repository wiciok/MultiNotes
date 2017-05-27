using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiNotes.Model;

namespace MultiNotes.Core
{
    class AndroidNoteApi : INoteApi
    {
        private string _userId;
        private AuthenticationRecord _authenticationRecord;
        private INoteMethod _androidNoteMethod;

        public AndroidNoteApi(AuthenticationRecord authenticationRecord, string userId)
        {
            this._authenticationRecord = authenticationRecord;
            this._userId = userId;
            _androidNoteMethod = new NoteMethod(ConnectionApi.HttpClient); // TODO Change to android api
        }

        public async Task AddNoteAsync(Note note)
        {
            throw new NotImplementedException();
        }

        public async Task<Note> GetNoteByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task DeleteNoteByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateNoteAsync(string id, Note newNote)
        {
            throw new NotImplementedException();
        }
    }
}
