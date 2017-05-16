using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using MultiNotes.Core;

namespace MultiNotes.XAndroid.Models
{
    public class NoteModel : INoteModel
    {

        private string noteId;
        private string noteContent;


        public NoteModel(string noteId, string noteContent)
        {
            this.noteId = noteId;
            this.noteContent = noteContent;
        }


        /**
         * Implements INoteModel.NoteId { get; }
         */
        public string NoteId
        {
            get { return noteId; }
        }


        /**
         * Implements INoteModel.NoteContent { get; set; }
         */
        public string NoteContent
        {
            get { return noteContent; }
            set { noteContent = value; }
        }


        /**
         * Implements INoteModel.SaveChanges()
         */
        public void SaveChanges()
        {
            INotesRepository notesRepository = new NotesRepository();
            notesRepository.NotesList.Where(x => x.Id == noteId).FirstOrDefault().Content = noteContent;
            notesRepository.NotesList.Where(x => x.Id == noteId).FirstOrDefault().LastChangeTimestamp = DateTime.Now;
            notesRepository.Reorder();
        }


        /**
         * Implements INoteModel.DeleteNote()
         */
        public void DeleteNote()
        {
            INotesRepository notesRepository = new NotesRepository();
            notesRepository.NotesList.Remove(notesRepository.NotesList.Where(x => x.Id == noteId).FirstOrDefault());
            notesRepository.Reorder();
        }


        /**
         * Implements INoteModel.AddNote()
         */
        public void AddNote()
        {
            INotesRepository notesRepository = new NotesRepository();
            notesRepository.NotesList.Add(new Note()
            {
                Id = Counter.Next.ToString(),
                Content = noteContent,
                LastChangeTimestamp = DateTime.Now
            });
            notesRepository.Reorder();
        }

    }
}
