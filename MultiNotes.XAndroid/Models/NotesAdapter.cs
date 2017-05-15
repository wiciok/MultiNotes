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
    public class NotesAdapter : BaseAdapter
    {
        private INotesRepository notesRepository;
        private Activity activity;


        public NotesAdapter(Activity activity)
        {
            this.activity = activity;
            notesRepository = new NotesRepository();
        }


        public override int Count
        {
            get { return notesRepository.NotesList.Count; }
        }


        public virtual List<Note> NotesList
        {
            get { return notesRepository.NotesList; }
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return new NoteWrapper(notesRepository.NotesList[position]);
        }


        public override long GetItemId(int position)
        {
            try
            {
                return long.Parse(notesRepository.NotesList[position].Id);
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
                notesRepository.NotesList[position].Content.Length > 20 ?
                notesRepository.NotesList[position].Content.Substring(0, 20) : notesRepository.NotesList[position].Content;

            return view;
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
