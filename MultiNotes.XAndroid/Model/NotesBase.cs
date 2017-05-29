﻿using System;
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

namespace MultiNotes.XAndroid.Model
{
    internal sealed class NotesBase
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
                            InitializeInstance();
                        }
                    }
                }
                return instance;
            }
        }

        private static void InitializeInstance()
        {
            instance.NotesList.Add(new Note() { Id = Counter.Default.Next.ToString(), OwnerId = "", Content = "Notatka #1" });
            instance.NotesList.Add(new Note() { Id = Counter.Default.Next.ToString(), OwnerId = "", Content = "Notatka #2" });
            instance.NotesList.Add(new Note() { Id = Counter.Default.Next.ToString(), OwnerId = "", Content = "Notatka #3" });
            instance.NotesList.Add(new Note() { Id = Counter.Default.Next.ToString(), OwnerId = "", Content = "Notatka #4" });
        }


        // Non-static section starts here:

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