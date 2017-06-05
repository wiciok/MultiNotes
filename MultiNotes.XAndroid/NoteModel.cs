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

using MultiNotes.Model;
using MultiNotes.XAndroid.Model;
using MultiNotes.XAndroid.Model.Base;

namespace MultiNotes.XAndroid
{
    public class NoteModel
    {
        private string noteId;
        private string noteContent;


        public NoteModel(string noteId, string noteContent)
        {
            this.noteId = noteId;
            this.noteContent = noteContent;
        }


        public string NoteId { get { return noteId; } }


        public string NoteContent
        {
            get { return noteContent; }
            set { noteContent = value; }
        }
    }
}
