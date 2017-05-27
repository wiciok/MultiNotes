using System.Collections.Generic;
using MultiNotes.Model;


namespace MultiNotes.Server.Repositories
{
    interface INoteRepository
    {
        IEnumerable<INote> GetAllNotes();
        INote GetNote(string id);
        INote AddNote(INote item);
        void RemoveNote(string id);
        void UpdateNote(string id, INote item);
        IEnumerable<INote> GetAllNotes(User user);
        bool CheckForNote(string id);
    }
}
