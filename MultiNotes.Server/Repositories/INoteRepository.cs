using System.Collections.Generic;
using MultiNotes.Model;


namespace MultiNotes.Server.Repositories
{
    interface INoteRepository
    {
        IEnumerable<Note> GetAllNotes();
        Note GetNote(string id);
        Note AddNote(Note item);
        void RemoveNote(string id);
        void RemoveAllUserNotes(string idUser);
        void UpdateNote(string id, Note item);
        IEnumerable<Note> GetAllNotes(User user);
        bool CheckForNote(string id);
        bool CheckForAnyUserNote(User user);
    }
}
