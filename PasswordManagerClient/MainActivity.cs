using System;
using System.Threading;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Widget;
using Android.OS;
using Android.Views;

namespace PasswordManagerClient
{
    class SearchEventArgs
    {
        public bool Result { get; }

        public SearchEventArgs(bool result)
        {
            Result = result;
        }
    }

    [Activity(Label = "Main window", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private CommunicationManger commManager;
        private TextView txtState;
        private ProgressBar progressBar;

        private event EventHandler<SearchEventArgs> onSearchEnd;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            ClipboardInterface.GetInstance().SetClipboardManager((ClipboardManager) GetSystemService(Context.ClipboardService));

            commManager = CommunicationManger.GetInstance();

            txtState = FindViewById<TextView>(Resource.Id.txtState);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            var searchBtn = FindViewById<Button>(Resource.Id.searchBtn);
            
            txtState.Text = "Searching for server";
            txtState.Visibility = ViewStates.Gone;
            progressBar.Visibility = ViewStates.Gone;

            searchBtn.Click += (sender, args) => SearchServer(30);
            onSearchEnd += SearchEnded;

            if (!commManager.IsBluetoothExists())
            {
                Console.WriteLine("finished");
                Toast.MakeText(this, "No bluetooth adapter found", ToastLength.Long);
                Finish();
                return;
            }
            
            if (!commManager.IsBluetoothOn())
            {
                Intent enableIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
                StartActivityForResult(enableIntent, 2);
            }

            Console.WriteLine("finished setup");
        }

        private void SearchEnded(object sender, SearchEventArgs searchEventArgs)
        {
            if (searchEventArgs.Result)
            {
                RunOnUiThread(() => txtState.Text = "Server found");
                GoAuthorize();
            }
            else
            {
                RunOnUiThread(() => 
                {
                    txtState.Text = "Server not found";
                    progressBar.Visibility = ViewStates.Gone;
                });
            }
        }

        private void SearchServer(double sec)
        {
            if (!commManager.IsBluetoothOn())
            {
                Intent enableIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
                StartActivityForResult(enableIntent, 2);
            }

            if (commManager.Searching)
                return;

            txtState.Text = "Searching for server";
            txtState.Visibility = ViewStates.Visible;
            progressBar.Visibility = ViewStates.Visible;

            new Thread(() =>
            {
                bool res = commManager.TryConnect(sec);
                onSearchEnd.Invoke(this, new SearchEventArgs(res));
            }).Start();
        }

        private void GoAuthorize()
        {
            Console.WriteLine("go authorize");
            Intent intent = new Intent(this, typeof(AuthorizeActivity));
            this.StartActivity(intent);
        }

        protected override void OnPause()
        {
            base.OnPause();

            commManager.Searching = false;
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == 2 && resultCode != Result.Ok)
            {
                Toast.MakeText(this, "Bluetooth not enabled", ToastLength.Short).Show();
                Finish();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            commManager.close();
        }
    }
}

