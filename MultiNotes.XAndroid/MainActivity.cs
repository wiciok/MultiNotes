using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content.PM;

namespace MultiNotes.XAndroid
{
    [Activity(MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.TopMenus, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.ToolbarMain);
            SetActionBar(toolbar);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Toast.MakeText(this, "Action selected: " + item.TitleFormatted,
                ToastLength.Short).Show();
            if (item.ItemId == Resource.Id.MenuAccount)
            {
                StartActivity(typeof(SignInActivity));
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}
