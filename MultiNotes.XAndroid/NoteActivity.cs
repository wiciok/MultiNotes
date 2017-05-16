using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using MultiNotes.XAndroid.Models;

namespace MultiNotes.XAndroid
{
    [Activity(MainLauncher = false,
        ParentActivity = typeof(MainActivity),
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/AppTheme.NoActionBar")]
    public sealed class NoteActivity : DefaultActivity
    {
        public static readonly string NOTE_ID = "NOTE_ID";
        public static readonly string NOTE_CONTENT = "NOTE_CONTENT";

        private INoteModel model;
        private EditText noteEditText;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_note);
            SetActionBar(FindViewById<Toolbar>(Resource.Id.toolbar_note));

            noteEditText = FindViewById<EditText>(Resource.Id.note_edit_text);

            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            model = new NoteModel(Intent.GetStringExtra(NOTE_ID), Intent.GetStringExtra(NOTE_CONTENT));
            noteEditText.Text = model.NoteContent;
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_note, menu);
            return base.OnCreateOptionsMenu(menu);
        }


        public override bool OnOptionsItemSelected(IMenuItem menuItem)
        {
            switch (menuItem.ItemId)
            {
                case Android.Resource.Id.Home:
                    return MenuHomeOnClick();

                case Resource.Id.note_menu_save:
                    return MenuSaveOnClick();

                case Resource.Id.note_menu_delete:
                    return MenuDeleteOnClick();

                default:
                    return base.OnOptionsItemSelected(menuItem);
            }
        }


        private bool MenuSaveOnClick()
        {
            model.NoteContent = noteEditText.Text;
            if (model.NoteContent.Length == 0)
            {
                Toast.MakeText(this, Resource.String.NoteEmptyAlert, ToastLength.Short).Show();
                return false;
            }
            if (model.NoteId.Length != 0)
            {
                model.SaveChanges();
            }
            else
            {
                // If NoteId is not set, a new note has to be created
                model.AddNote();
            }
            Finish();
            return true;
        }


        private bool MenuDeleteOnClick()
        {
            if (model.NoteId.Length != 0)
            {
                model.DeleteNote();
            }
            Finish();
            return true;
        }
    }
}
