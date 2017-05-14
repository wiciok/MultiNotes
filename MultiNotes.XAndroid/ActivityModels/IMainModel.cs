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
    public interface IMainModel
    {
        IAuthorization Authorization { get; }
        INotesRepository NotesRepository { get; }
    }
}
