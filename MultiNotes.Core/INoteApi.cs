using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using MultiNotes.Model;

namespace MultiNotes.Core
{
    public interface INoteApi
    {
        /// <summary>
        /// Saves note to database and locally
        /// </summary>
        /// <param name="note">Note to save</param>
        void AddNote(Note note);
        /// <summary>
        /// Gets specific note 
        /// </summary>
        /// <param name="id">ID of a note</param>
        /// <returns>A note with a given ID</returns>
        Task<Note> GetNoteById(string id);
        /// <summary>
        /// Gets all notes
        /// </summary>
        /// <returns>List of all notes</returns>
        Task<IEnumerable<Note>> GetAllNotes();
        /// <summary>
        /// Removes note from database and locally
        /// </summary>
        /// <param name="id">Note ID</param>
        /// <returns>True, if note was successfully deleted , false if it was not found</returns>
        Task<bool> DeleteNoteById(string id);
        /// <summary>
        /// Replaces a note by a new one
        /// </summary>
        /// <param name="id">ID of an old note</param>
        /// <param name="newNote">Note that replaces the old one</param>
        void UpdateNote(string id, Note newNote);
    }
}