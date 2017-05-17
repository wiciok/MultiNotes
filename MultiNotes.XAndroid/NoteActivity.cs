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

using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace MultiNotes.XAndroid
{
    [Activity(MainLauncher = false,
        ParentActivity = typeof(MainActivity),
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/AppTheme.NoActionBar")]
    public sealed class NoteActivity : MultiNotesBaseActivity
    {

        public static readonly string NOTE_ID = "NOTE_ID";
        public static readonly string NOTE_CONTENT = "NOTE_CONTENT";


        private EditText noteEditText;

        private INoteModel model;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_note);
            SetSupportActionBar(FindViewById<SupportToolbar>(Resource.Id.toolbar_note));

            noteEditText = FindViewById<EditText>(Resource.Id.note_edit_text);

            EnableSupportToolbarHomeMenu();

            model = new NoteModel(Intent.GetStringExtra(NOTE_ID), Intent.GetStringExtra(NOTE_CONTENT));
            noteEditText.Text = "";
            noteEditText.Append(model.NoteContent);
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
                case Resource.Id.note_menu_save:
                    return MenuSaveOnClick();

                case Resource.Id.note_menu_delete:
                    return MenuDeleteOnClick();

                default:
                    return base.OnOptionsItemSelected(menuItem);
            }
        }

        public override void OnBackPressed()
        {
            if (noteEditText.Text.Trim() != model.NoteContent)
            {
                ShowNoteNotSavedAlert(delegate { base.OnBackPressed(); });
            }
            else
            {
                base.OnBackPressed();
            }
        }

        protected override bool MenuHomeOnClick()
        {
            if (noteEditText.Text.Trim() != model.NoteContent)
            {
                bool result = true;
                ShowNoteNotSavedAlert(delegate { result = base.MenuHomeOnClick(); });
                return result;
            }
            else
            {
                return base.MenuHomeOnClick();
            }
        }


        private bool MenuSaveOnClick()
        {
            model.NoteContent = noteEditText.Text.Trim();
            if (model.NoteContent.Length == 0)
            {
                Toast.MakeText(this, Resource.String.alert_note_empty, ToastLength.Short).Show();
                return true;
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
                ShowDeleteNoteAlert(delegate { model.DeleteNote(); Finish(); });
            }
            else
            {
                if (noteEditText.Text.Length > 0)
                {
                    ShowNoteNotSavedAlert(delegate { Finish(); });
                }
                else
                {
                    Finish();
                }
            }
            return true;
        }

        private void ShowDeleteNoteAlert(EventHandler<DialogClickEventArgs> onPositive)
        {
            ShowDeleteNoteAlert(onPositive, delegate { });
        }

        private void ShowDeleteNoteAlert(EventHandler<DialogClickEventArgs> onPositive, 
                                         EventHandler<DialogClickEventArgs> onNegative)
        {
            new AlertDialog.Builder(this)
                .SetMessage(Resource.String.confirm_delete_note_text)
                .SetIcon(Resource.Drawable.ic_warning_black)
                .SetPositiveButton(Resource.String.confirm_dialog_yes, onPositive)
                .SetNegativeButton(Resource.String.confirm_dialog_no, onNegative)
                .Show();
        }

        private void ShowNoteNotSavedAlert(EventHandler<DialogClickEventArgs> onPositive)
        {
            ShowNoteNotSavedAlert(onPositive, delegate { });
        }

        private void ShowNoteNotSavedAlert(EventHandler<DialogClickEventArgs> onPositive,
                                           EventHandler<DialogClickEventArgs> onNegative)
        {
            new AlertDialog.Builder(this)
                .SetMessage(Resource.String.confirm_note_not_saved)
                .SetIcon(Resource.Drawable.ic_warning_black)
                .SetPositiveButton(Resource.String.confirm_dialog_yes, onPositive)
                .SetNegativeButton(Resource.String.confirm_dialog_no, onNegative)
                .Show();
        }

    }
}
