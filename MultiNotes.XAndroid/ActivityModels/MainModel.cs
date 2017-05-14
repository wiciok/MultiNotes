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

using MultiNotes.XAndroid.Models;

namespace MultiNotes.XAndroid.ActivityModels
{
    public class MainModel : IMainModel
    {
        private IAuthorization authorization;
        private INotesRepository notesRepository;


        public MainModel()
        {
            authorization = new Authorization();
            notesRepository = new NotesRepository();
        }


        public IAuthorization Authorization
        {
            get { return authorization; }
        }


        public INotesRepository NotesRepository
        {
            get { return notesRepository; }
        }
    }
}
