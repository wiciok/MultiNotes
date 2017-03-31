using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiNotes.Server
{
    //todo: tutaj oczywiscie beda zmiany po wprowadzeniu userów

    interface INoteRepository
    {
        IEnumerable<Note> GetAllNotes();
        Note GetNote(string id);
        Note AddNote(Note item);
        bool RemoveNote(string id);
        bool UpdateNote(string id, Note item);
    }
}
