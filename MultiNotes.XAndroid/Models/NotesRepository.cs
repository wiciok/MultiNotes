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
    public class NotesRepository : INotesRepository
    {
        public List<Note> NotesList { get { return NotesBase.Instance.NotesList; } }
    }
}
