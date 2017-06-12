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
using System.Threading;

namespace PasswordManagerClient
{
    [Activity(Label = "ItemsSelection", MainLauncher = true)]
    public class ItemsSelectionActivity : Activity
    {
        private List<PasswordEntry> items;
        private ListView listView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ItemsSelection);
            listView = FindViewById<ListView>(Resource.Id.PassList);

            items = new List<PasswordEntry>();
            items.Add(new PasswordEntry(1,"facebook"));
            items.Add(new PasswordEntry(2, "google"));
            items.Add(new PasswordEntry(3, "instagram"));

            ListViewAdapter adapter = new ListViewAdapter(this, items);

            listView.Adapter = adapter;
            listView.ItemClick += ListView_ItemClick;
            listView.ItemLongClick += ListView_ItemLongClick;
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int passID = items[e.Position].ID;

            //TODO get pass
        }

        private void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            int passID = items[e.Position].ID;

            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            DeleteConfirmFragment deleteFragment = new DeleteConfirmFragment(passID);
            deleteFragment.onConfirmChose += DeleteFragmentOnConfirmChose;

            deleteFragment.Show(transaction, "confirm delete dialog");
        }

        private void DeleteFragmentOnConfirmChose(object sender, ConfirmDeleteEventArgs confirmDeleteEventArgs)
        {
            new Thread(() =>
            {
                bool success = CommunicationManger.GetInstance().RequestDeletePassword(confirmDeleteEventArgs.passID);

                if (success)
                    items.Remove(items.Find(x => x.ID == confirmDeleteEventArgs.passID));
                else
                    this.RunOnUiThread(() => { Toast.MakeText(this, "Delete failed", ToastLength.Long).Show(); });
            }).Start();
        }
    }
}