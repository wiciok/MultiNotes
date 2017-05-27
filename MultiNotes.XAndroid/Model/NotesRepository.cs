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
using MultiNotes.Model;
using MultiNotes.XAndroid.Model.Base;

namespace MultiNotes.XAndroid.Model
{
    public class NotesRepository : INotesRepository
    {

        /**
         * Implements INotesRepository.NotesList { get; }
         */
        public List<Note> NotesList
        {
            get { return NotesBase.Instance.NotesList; }
        }


        /**
         * Implements INotesRepository.Reorder()
         */
        public void SortDescending()
        {
            NotesBase.Instance.Reorder();
        }

    }
}
