using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using MultiNotes.Model;
using MultiNotes.XAndroid.Model.Base;
using MultiNotes.XAndroid.Core;

namespace MultiNotes.XAndroid.Model
{
    public class NoteAdapter : BaseAdapter
    {

        //private INotesRepository notesRepository;
        private Activity activity;
        private int count;
        private List<Note> notesList;


        public NoteAdapter(Activity activity)
        {
            this.activity = activity;
            //notesRepository = new NotesRepository();
        }


        public override int Count
        {
            get { GetNotesCount(); return count; }//return notesRepository.NotesList.Count; }
        }

        private void GetNotesCount()
        {
            GetNotesList();
            count = notesList.Count();
        }


        public virtual List<Note> NotesList
        {
            get { GetNotesList(); return notesList; }
        }

        private void GetNotesList()
        {
            notesList = new LocalNoteRepository().GetAllNotes()
                .OrderByDescending(g => g.LastChangeTimestamp).ToList();
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return new NoteWrapper(NotesList[position]);
        }


        public override long GetItemId(int position)
        {
            try
            {
                return long.Parse(NotesList[position].Id);
            }
            catch (FormatException)
            {
                return -1;
            }
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.list_item_note, parent, false);
            TextView notesTitleTextView = view.FindViewById<TextView>(Resource.Id.title);

            notesTitleTextView.Text = NotesList[position].Content.Length > 20
                ? NotesList[position].Content.Substring(0, 20) + " . . ." 
                : NotesList[position].Content;

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
