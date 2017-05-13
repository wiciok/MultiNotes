﻿using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content.PM;
using System.Collections.Generic;

namespace MultiNotes.XAndroid
{
    [Activity(MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.ToolbarMain);
            SetActionBar(toolbar);

            Button signInButton = (Button)FindViewById(Resource.Id.Main_SignInButton);
            signInButton.Click += delegate
            {
                StartActivity(typeof(SignInActivity));
            };


            ListView list = (ListView)FindViewById(Resource.Id.Main_NotesListView);

            string[] cars = new string[]
            {
                "Mercedes",
                "Fiat",
                "Ferrari",
                "Aston Martin",
                "Lamborghini",
                "Skoda",
                "Volkswagen",
                "Audi",
                "Citroen"
            };

            List<string> carL = new List<string>();
            carL.AddRange(cars);

            ArrayAdapter adapter = new ArrayAdapter<string>(this, Resource.Id.Main_NotesListView, carL);

            list.Adapter = adapter);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.TopMenus, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Toast.MakeText(this, "Action selected: " + item.TitleFormatted,
                ToastLength.Short).Show();

            if (item.ItemId == Resource.Id.MenuAccount)
            {
                return MenuAccountOnClick();
            }
            switch (item.ItemId)
            {
                case Resource.Id.MenuAccount:
                    return MenuAccountOnClick();
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private bool MenuAccountOnClick()
        {
            StartActivity(typeof(AccountActivity));
            return true;
        }
    }
}