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

namespace MultiNotes.XAndroid.Core
{
    public class RemoteNoteRepository : INoteRepository
    {

        public List<Note> GetAllNotes()
        {
            throw new NotImplementedException();
        }


        public void AddNote(Note note)
        {
            throw new NotImplementedException();
        }


        public void UpdateNote(Note note)
        {
            throw new NotImplementedException();
        }


        public void DeleteNote(Note note)
        {
            throw new NotImplementedException();
        }

    }
}