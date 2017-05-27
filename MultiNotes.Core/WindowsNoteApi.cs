using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiNotes.Model;

namespace MultiNotes.Core
{
    class WindowsNoteApi : INoteApi
    {
        private string userId;
        public WindowsNoteApi(string userId)
        {
            this.userId = userId;
        }
        public void AddNote(Note note)
        {
            throw new NotImplementedException();
        }

        public Task<Note> GetNoteById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Note>> GetAllNotes()
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteNoteById(string id)
        {
            throw new NotImplementedException();
        }

        public void UpdateNote(string id, Note newNote)
        {
            throw new NotImplementedException();
        }
    }
}
