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
    internal class NotesBase
    {
        private static NotesBase instance = null;
        private static readonly object syncRoot = new object();

        public static NotesBase Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new NotesBase();

                            instance.NotesList.Add(new Note() { Id = Counter.Next, Content = "Notatka #1", LastChangeTimestamp = DateTime.Now });
                            instance.NotesList.Add(new Note() { Id = Counter.Next, Content = "Notatka #2", LastChangeTimestamp = DateTime.Now });
                            instance.NotesList.Add(new Note() { Id = Counter.Next, Content = "Notatka #3", LastChangeTimestamp = DateTime.Now });
                            instance.NotesList.Add(new Note() { Id = Counter.Next, Content = "Notatka #4", LastChangeTimestamp = DateTime.Now });
                        }
                    }
                }
                return instance;
            }
        }



        private List<Note> notesList;


        private NotesBase()
        {
            notesList = new List<Note>();
        }


        public List<Note> NotesList
        {
            get { return notesList; }
        }


        public void Reorder()
        {
            notesList = notesList.OrderByDescending(x => x.LastChangeTimestamp).ToList();
        }
    }
}