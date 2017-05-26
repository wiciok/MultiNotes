using MultiNotes.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiNotes.Core
{
    public interface INoteMethod
    {
        void AddNoteToFile(Note note);
        void AddNoteToDatabase(Note note, string token);
        Task<IEnumerable<Note>> GetAllNotesFromDatabase(string token);
        Task<Note> GetNoteByIdFromDatabase(string token, string id);
        Task<bool> DeleteNoteByIdFromDatabase(string token, string id);
        Task<bool> UpdateNoteByIdFromDatabase(string token, string id, Note newNote);
        List<Note> GetAllNotesFromFile(string userId);
        Note GetNoteFromFile(string id, string userId);
        void DeleteNoteFromFile(string id, string userId);
        void UpdateNoteFromFile(string id, string userId, Note newNote);
    }
}
