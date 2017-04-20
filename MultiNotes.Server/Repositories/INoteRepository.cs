using MultiNotes.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MultiNotes.Server.Repositories
{
    interface INoteRepository
    {
        IEnumerable<Note> GetAllNotes();
        Note GetNote(string id);
        Note AddNote(Note item);
        void RemoveNote(string id);
        void UpdateNote(string id, Note item);
        IEnumerable<Note> GetAllNotes(User user);
        bool CheckForNote(string id);
    }
}
