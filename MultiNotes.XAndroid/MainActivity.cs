using System;
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

using MultiNotes.Core;
using MultiNotes.XAndroid.ActivityModels;
using MultiNotes.XAndroid.ActivityModels.Base;
using MultiNotes.XAndroid.Models;

using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace MultiNotes.XAndroid
{
    [Activity(MainLauncher = true,
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/AppTheme.NoActionBar")]
    public sealed class MainActivity : MultiNotesBaseActivity
    {

        private ListView notesListView;

        private IMainModel model;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.activity_main);
            SetSupportActionBar(FindViewById<SupportToolbar>(Resource.Id.toolbar));

            // Set up field components
            notesListView = FindViewById<ListView>(Resource.Id.list_view_notes);

            // Set up local variable components
            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);

            notesListView.Adapter = new NoteAdapter(this);
            notesListView.ItemClick += NotesListItemOnClick;

            model = new MainModel();

            fab.Click += FloatingActionButtonOnClick;
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


        protected override void OnResume()
        {
            base.OnResume();
            if (!model.SignedIn)
            {
                StartActivity(typeof(SignInActivity));
            }
            else
            {
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
            RefreshNotesList();
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
