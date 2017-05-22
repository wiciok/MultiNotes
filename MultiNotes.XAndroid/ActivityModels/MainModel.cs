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
    public class MainModel : IMainModel
    {

        private IAuthorizationEngine authEngine;
        private INotesRepository repository;


        public MainModel()
        {
            authEngine = AuthorizationEngine.Instance;
            repository = new NotesRepository();
        }

        public User User { get { return authEngine.User; } }

        public UserHeader UserHeader { get { return authEngine.UserHeader; } }

        public List<Note> NotesList { get { return repository.NotesList; } }

        public bool SignedIn { get { return authEngine.SignedIn; } }

    }
}