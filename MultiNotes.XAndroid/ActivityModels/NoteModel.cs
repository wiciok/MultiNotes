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

using MultiNotes.Core;
using MultiNotes.XAndroid.ActivityModels.Base;
using MultiNotes.XAndroid.Models;
using MultiNotes.XAndroid.Models.Base;

namespace MultiNotes.XAndroid.ActivityModels
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

        
        public string NoteId { get { return noteId; } }
        
        public string NoteContent
        {
            get { return noteContent; }
            set { noteContent = value; }
        }

        
        public void SaveChanges()
        {
            INotesRepository notesRepository = new NotesRepository();
            notesRepository.NotesList.Where(x => x.Id == noteId).FirstOrDefault().Content = noteContent;
            notesRepository.NotesList.Where(x => x.Id == noteId).FirstOrDefault().LastChangeTimestamp = DateTime.Now;
            notesRepository.SortDescending();
        }


        public void DeleteNote()
        {
            INotesRepository notesRepository = new NotesRepository();
            notesRepository.NotesList.Remove(notesRepository.NotesList.Where(x => x.Id == noteId).FirstOrDefault());
            notesRepository.SortDescending();
        }


        public void AddNote()
        {
            INotesRepository notesRepository = new NotesRepository();
            notesRepository.NotesList.Add(new Note()
            {
                Id = Counter.Default.Next.ToString(),
                Content = noteContent,
                LastChangeTimestamp = DateTime.Now
            });
            notesRepository.SortDescending();
        }

    }
}