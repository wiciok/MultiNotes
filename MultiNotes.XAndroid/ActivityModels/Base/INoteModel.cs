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

namespace MultiNotes.XAndroid.ActivityModels.Base
{
    public interface INoteModel
    {

        string NoteId { get; }

        string NoteContent { get; set; }


        void SaveChanges();

        void DeleteNote();

        void AddNote();

    }
}
