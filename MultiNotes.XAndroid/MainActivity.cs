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

using MultiNotes.Core;
using MultiNotes.XAndroid.Models;

namespace MultiNotes.XAndroid
{
    [Activity(MainLauncher = true, 
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : DefaultActivity
    {
        private ListView notesListView;
        

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ActivityMain);

            NotesFileSystem.Instance.FilesDir = FilesDir;

            SetActionBar(FindViewById<Toolbar>(Resource.Id.ToolbarMain));
            notesListView = FindViewById<ListView>(Resource.Id.Main_NotesListView);
            
            notesListView.Adapter = new NotesAdapter(this);
            notesListView.ItemClick += NotesListItemOnClick;

            if (!(new AuthorizationFactory().Create().Successful))
            {
                StartActivity(typeof(SignInActivity));
            }
        }

        private void NotesListItemOnClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            NotesAdapter.INoteWrapper noteObject 
                = notesListView.Adapter.GetItem(e.Position) as NotesAdapter.INoteWrapper;

            // If noteObject is null, show alert
            if (!(noteObject is NotesAdapter.INoteWrapper))
            {
                Toast.MakeText(this, "Coś poszło nie tak . . .", ToastLength.Short).Show();
                return;
            }

            // Get our item from the list adapter
            Note note = noteObject.Note;
            
            Toast.MakeText(this, note.Content, ToastLength.Short).Show();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MenuMain, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem menuItem)
        {
            Toast.MakeText(this, "Action selected: " + menuItem.TitleFormatted,
                ToastLength.Short).Show();

            switch (menuItem.ItemId)
            {
                case Resource.Id.Main_MenuEdit:
                    return MenuEditOnClick();

                case Resource.Id.Main_MenuSync:
                    return MenuSyncOnClick();

                case Resource.Id.Main_MenuAccount:
                    return MenuAccountOnClick();

                default:
                    return base.OnOptionsItemSelected(menuItem);
            }
        }

        private bool MenuEditOnClick()
        {
            return true;
        }

        private bool MenuSyncOnClick()
        { 
            NotesAdapter adapter = FindViewById<ListView>(Resource.Id.Main_NotesListView).Adapter as NotesAdapter;
            if (adapter == null)
            {
                return false;
            }
            adapter.Refresh();
            return true;
        }

        private bool MenuAccountOnClick()
        {
            StartActivity(typeof(AccountActivity));
            return true;
        }
    }
}
