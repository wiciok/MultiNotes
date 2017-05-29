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

using MultiNotes.Model;

namespace MultiNotes.XAndroid.Model.Base
{
    public interface INotesRepository
    {

        List<Note> NotesList { get; }

        void SortDescending();

    }
}
