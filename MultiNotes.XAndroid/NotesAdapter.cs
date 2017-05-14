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

namespace MultiNotes.XAndroid
{
    public class NotesAdapter : BaseAdapter
    {
        private List<Note> notesList;
        private Activity activity;

        public NotesAdapter(Activity activity)
        {
            this.activity = activity;
            notesList = new List<Note>();
            FillNotes();
        }

        public override int Count
        {
            get { return notesList.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return new NoteWrapper(notesList[position]);
        }

        public override long GetItemId(int position)
        {
            try
            {
                return long.Parse(notesList[position].Id);
            }
            catch (FormatException)
            {
                return -1;
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.ListItemNote, parent, false);
            TextView notesTitleTextView = view.FindViewById<TextView>(Resource.Id.NoteTitle);

            notesTitleTextView.Text = 
                notesList[position].Content.Length > 20 ? 
                notesList[position].Content.Substring(0, 20) : notesList[position].Content;

            return view;
        }

        private void FillNotes()
        {
            notesList.Add(new Note() { Id = "1", Content = "Cześć!" });
            notesList.Add(new Note() { Id = "2", Content = "Siema!" });
            notesList.Add(new Note() { Id = "3", Content = "Eloszki!" });
            notesList.Add(new Note() { Id = "4", Content = "Pozdro!" });
        }

        public void Refresh()
        {
            notesList.Clear();
            FillNotes();
        }

        public interface INoteWrapper
        {
            Note Note { get; }
        }

        private class NoteWrapper : Java.Lang.Object, INoteWrapper
        {
            private Note note;

            public NoteWrapper(Note note) 
            {
                this.note = note;
            }

            public Note Note { get { return note; } }
        }
    }
}
