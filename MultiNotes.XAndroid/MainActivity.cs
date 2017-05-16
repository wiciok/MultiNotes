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
using MultiNotes.XAndroid.Models;

namespace MultiNotes.XAndroid
{
    [Activity(MainLauncher = true,
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/AppTheme.NoActionBar")]
    public sealed class MainActivity : DefaultActivity
    {
        private ListView notesListView;
        private FloatingActionButton fab;

        private IMainModel model;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.activity_main);

            SetActionBar(FindViewById<Toolbar>(Resource.Id.toolbar_main));
            notesListView = FindViewById<ListView>(Resource.Id.main_list_view_notes);

            notesListView.Adapter = new NotesAdapter(this);
            notesListView.ItemClick += NotesListItemOnClick;

            model = new MainModel();

            if (!(model.Authorization.Successful))
            {
                StartActivity(typeof(SignInActivity));
            }
        }


        private void NotesListItemOnClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            NotesAdapter adapter = notesListView.Adapter as NotesAdapter;
            if (adapter == null)
            {
                return;
            }

            NotesAdapter.INoteWrapper noteObject = adapter.GetItem(e.Position) as NotesAdapter.INoteWrapper;
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
                case Resource.Id.main_menu_edit:
                    return MenuEditOnClick();

                case Resource.Id.main_menu_sync:
                    return MenuSyncOnClick();

                case Resource.Id.main_menu_account:
                    return MenuAccountOnClick();

                default:
                    return base.OnOptionsItemSelected(menuItem);
            }
        }


        protected override void OnResume()
        {
            base.OnResume();
            RefreshNotesList();
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


        private void RefreshNotesList()
        {
            NotesAdapter adapter = notesListView.Adapter as NotesAdapter;
            if (adapter != null)
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
