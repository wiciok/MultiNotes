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
using MultiNotes.Model;
using MultiNotes.XAndroid.Model;
using MultiNotes.XAndroid.Model.Base;

namespace MultiNotes.XAndroid.ActivityModels.Base
{
    public interface IMainModel
    {

        User User { get; }

        UserHeader UserHeader { get; }

        List<Note> NotesList { get; }

        bool SignedIn { get; }

    }
}
