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

namespace MultiNotes.XAndroid
{
    public class NotesAdapter : BaseAdapter
    {
        private List<Note> notesList;
        private Activity activity;

        public NotesAdapter(Activity activity)
        {
            this.activity = activity;
            //FillContacts();
        }

        public override int Count
        {
            get { return notesList.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            // Could wrap a Ntoe in a Java.Lang.Object
            // to return it here if needed.
            return null;
        }

        public override long GetItemId(int position)
        {
            try
            {
                return long.Parse(notesList[position].Id);
            }
            catch (FormatException)
            {
                return -1;
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            throw new NotImplementedException();
        }

        /*
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView ?? activity.LayoutInflater.Inflate(
                Resource.Layout.ListItemDefault, parent, false);
            var contactName = view.FindViewById<TextView>(Resource.Id.ContactName);
            var contactImage = view.FindViewById<ImageView>(Resource.Id.ContactImage);
            contactName.Text = notesList[position].Content.Substring(0, 50);

            if (_contactList[position].PhotoId == null)
            {
                contactImage = view.FindViewById<ImageView>(Resource.Id.ContactImage);
                contactImage.SetImageResource(Resource.Drawable.contactImage);
            }
            else
            {
                var contactUri = ContentUris.WithAppendedId(
                    ContactsContract.Contacts.ContentUri, _contactList[position].Id);
                var contactPhotoUri = Android.Net.Uri.WithAppendedPath(contactUri,
                    Contacts.Photos.ContentDirectory);
                contactImage.SetImageURI(contactPhotoUri);
            }
            return view;
        }
        */
    }
}