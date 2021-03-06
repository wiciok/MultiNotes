﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;

using MultiNotes.Model;
using MultiNotes.XAndroid.Core;

using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using System.Threading;

namespace MultiNotes.XAndroid
{
    [Activity(MainLauncher = true,
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/AppTheme.NoActionBar")]
    public sealed class MainActivity : MultiNotesBaseActivity
    {

        private ListView notesListView;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.activity_main);
            SetSupportActionBar(FindViewById<SupportToolbar>(Resource.Id.toolbar));

            // Set up field components
            notesListView = FindViewById<ListView>(Resource.Id.list_view_notes);

            // Set up local variable components
            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);

            // Run setup for application - all directories etc.
            Setup.Run();

            notesListView.Adapter = new NoteAdapter(this);
            notesListView.ItemClick += NotesListItemOnClick;

            fab.Click += FloatingActionButtonOnClick;

            // Set up all things we need from start
            // ConnectionApi.Configure();
        }

        private void NotesListItemOnClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            NoteAdapter adapter = notesListView.Adapter as NoteAdapter;
            if (adapter == null)
            {
                return;
            }

            NoteAdapter.INoteWrapper noteObject = adapter.GetItem(e.Position) as NoteAdapter.INoteWrapper;
            if (noteObject == null)
            {
                return;
            }
            // Get our item from the list adapter
            Note note = noteObject.Note;
            StartNoteActivity(note.Id, note.Content);
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return base.OnCreateOptionsMenu(menu);
        }


        public override bool OnOptionsItemSelected(IMenuItem menuItem)
        {
            switch (menuItem.ItemId)
            {
                case Resource.Id.menu_edit:
                    return MenuEditOnClick();

                case Resource.Id.menu_sync:
                    return MenuSyncOnClick();

                case Resource.Id.menu_account:
                    return MenuAccountOnClick();

                default:
                    return base.OnOptionsItemSelected(menuItem);
            }
        }

        private bool syncOnStartup = false;

        protected override void OnResume()
        {
            base.OnResume();
            if (!new Authorization().IsUserSigned)
            {
                StartActivity(typeof(SignInActivity));
            }
            else
            {
                if (!syncOnStartup)
                {
                    if (Utility.IsNetworkAvailable(this) && new Authorization().IsUserSigned)
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
                    syncOnStartup = true;
                }
                RefreshNotesList();
            }
        }


        private bool MenuEditOnClick()
        {
            StartNoteActivity();
            return true;
        }


        private bool MenuSyncOnClick()
        {
            new Thread(new ThreadStart(() =>
            {
                ProgressDialog progress = null;
                WebApiClientError error = WebApiClientError.OK;

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

                if (Utility.IsNetworkAvailable(this))
                {
                    try
                    {
                        new NoteSync().Sync();
                    }
                    catch (WebApiClientException e)
                    {
                        error = e.Error;
                    }
                }
                else
                {
                    error = WebApiClientError.InternetConnectionError;
                }

                RunOnUiThread(() =>
                {
                    progress.Hide();
                    if (error == WebApiClientError.InternetConnectionError)
                    {
                        RunOnUiThread(() =>
                        {
                            new AlertDialog.Builder(this)
                               .SetTitle(Resource.String.error)
                               .SetMessage(Resource.String.internet_connection_error)
                               .SetPositiveButton(Resource.String.confirm_dialog_ok, delegate { })
                               .Create().Show();
                        });
                    }
                    RefreshNotesList();
                });

            })).Start();

            return true;
        }


        private bool MenuAccountOnClick()
        {
            StartActivity(typeof(AccountActivity));
            return true;
        }


        private void FloatingActionButtonOnClick(object sender, EventArgs e)
        {
            StartNoteActivity();
        }


        private void RefreshNotesList()
        {
            // The following code works the same as:
            // 
            // NoteAdapter adapter = notesListView.Adapter as NoteAdapter;
            // if (adapter != null)
            // {
            //      adapter.NotifyDataSetChanged();
            // }
            if (notesListView.Adapter is NoteAdapter adapter)
            {
                adapter.NotifyDataSetChanged();
            }
        }


        private void StartNoteActivity()
        {
            StartNoteActivity("", "");
        }

        private void StartNoteActivity(string id, string content)
        {
            Intent intent = new Intent(this, typeof(NoteActivity));
            intent.PutExtra(NoteActivity.NOTE_ID, id);
            intent.PutExtra(NoteActivity.NOTE_CONTENT, content);
            StartActivity(intent);
        }

    }
}
