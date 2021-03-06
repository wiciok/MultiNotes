﻿using System;
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

using MultiNotes.XAndroid.Core;

using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using MultiNotes.Model;
using System.Threading;

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

        private Note note;

        // private NoteModel model;
        // private XNoteMethod noteMethods;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_note);
            SetSupportActionBar(FindViewById<SupportToolbar>(Resource.Id.toolbar));

            // Set up field components
            noteEditText = FindViewById<EditText>(Resource.Id.edit_text);

            EnableSupportToolbarHomeMenu();

            note = new XNoteMethod().GetAllLocalNotes()
                .Where(g => g.Id == Intent.GetStringExtra(NOTE_ID))
                .FirstOrDefault()
                ?? new Note()
                {
                    Id = "",
                    Content = "",
                    CreateTimestamp = DateTime.Now,
                    LastChangeTimestamp = DateTime.Now,
                    OwnerId = "-1"
                };

            noteEditText.Text = "";
            noteEditText.Append(note?.Content ?? "");
        }


        protected override void OnResume()
        {
            base.OnResume();
            if (note == null)
            {
                Finish();
            }
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
                case Resource.Id.menu_save:
                    return MenuSaveOnClick();

                case Resource.Id.menu_delete:
                    return MenuDeleteOnClick();

                default:
                    return base.OnOptionsItemSelected(menuItem);
            }
        }

        public override void OnBackPressed()
        {
            if (noteEditText.Text.Trim() != note.Content)
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
            if (noteEditText.Text.Trim() != note.Content)
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
            new Thread(new ThreadStart(() =>
            {
                ProgressDialog progress = null;

                RunOnUiThread(() =>
                {
                    progress = ProgressDialog.Show(
                        this,
                        Resources.GetString(Resource.String.please_wait),
                        Resources.GetString(Resource.String.please_wait),
                        true,
                        false
                    );
                });


                SaveNote();

                if (Utility.IsNetworkAvailable(this))
                {
                    try
                    {
                        new NoteSync().Sync();
                    }
                    catch (WebApiClientException)
                    {
                        // Do nothing
                    }
                }

                RunOnUiThread(() =>
                {
                    progress.Hide();
                    Finish();
                });
            })).Start();

            return true;
        }

        private void SaveNote()
        {
            string newContent = noteEditText.Text.Trim();
            if (newContent.Length == 0)
            {
                RunOnUiThread(() => Toast.MakeText(
                    this, 
                    Resource.String.alert_note_empty, 
                    ToastLength.Short
                ).Show());
                return;
            }
            if (note.Id.Length != 0)
            {
                note.Content = newContent;
                new XNoteMethod().UpdateLocalNote(note);
            }
            else
            {
                note.Id = new LocalUniqueIdApi().GetUniqueId();
                // We operate on not-localized DataTime values
                note.CreateTimestamp = DateTime.Now.ToUniversalTime();
                note.LastChangeTimestamp = DateTime.Now.ToUniversalTime();
                note.OwnerId = new Authorization().UserId;

                note.Content = newContent;
                new XNoteMethod().AddLocalNote(note);
            }
        }
        


        private bool MenuDeleteOnClick()
        {
            if (note.Id.Length != 0)
            {
                ShowDeleteNoteAlert(delegate
                {
                    new Thread(new ThreadStart(() =>
                    {
                        ProgressDialog progress = null;

                        RunOnUiThread(() =>
                        {
                            progress = ProgressDialog.Show(
                                this,
                                Resources.GetString(Resource.String.please_wait),
                                Resources.GetString(Resource.String.please_wait),
                                true,
                                false
                            );
                        });

                        new XNoteMethod().DeleteLocalNote(note);

                        if (Utility.IsNetworkAvailable(this))
                        {
                            try
                            {
                                new NoteSync().Sync();
                            }
                            catch (WebApiClientException)
                            {
                                // Do nothing
                            }
                        }

                        RunOnUiThread(() =>
                        {
                            progress.Hide();
                            Finish();
                        });
                    })).Start();
                });
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
