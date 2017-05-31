using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MultiNotes.Model;

namespace MultiNotes.XAndroid.Core
{
    public interface INoteRepository
    {
        bool Success { get; }

        List<Note> GetAllNotes();
        void AddNote(Note note);
        void UpdateNote(Note note);
        void DeleteNote(Note note);
    }
}