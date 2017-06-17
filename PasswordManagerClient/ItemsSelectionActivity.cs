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
    [Activity(Label = "Item selection window", MainLauncher = false)]
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
            var addBtn = FindViewById<Button>(Resource.Id.addBtn);

            items = new List<PasswordEntry>();
            items.Add(new PasswordEntry(1, "Google"));

            ListViewAdapter adapter = new ListViewAdapter(this, items);

            listView.Adapter = adapter;
            listView.ItemClick += ListView_ItemClick;
            listView.ItemLongClick += ListView_ItemLongClick;

            addBtn.Click += AddPasswordDialog;
        }

        private void AddPasswordDialog(object sender, EventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            AddPasswordFragment addPasswordFragment = new AddPasswordFragment();
            addPasswordFragment.onConfirm += AddPassword;

            addPasswordFragment.Show(transaction, "add password dialog");
        }

        private void UpdatePasswordDialog(object sender, PasswordModifyEventArgs e)
        {
            int id = items[e.Position].ID;
            string name = items[e.Position].Name;

            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            AddPasswordFragment addPasswordFragment = new AddPasswordFragment(id, name);
            addPasswordFragment.onConfirm += UpdatePassword;

            addPasswordFragment.Show(transaction, "add password dialog");
        }

        private void DeletePasswordDialog(object sender, PasswordModifyEventArgs e)
        {
            int id = items[e.Position].ID;

            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            DeleteConfirmFragment deleteFragment = new DeleteConfirmFragment(id);
            deleteFragment.onConfirmChose += DeletePassword;

            deleteFragment.Show(transaction, "confirm delete dialog");
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int passID = items[e.Position].ID;

            GetPassword(passID);
        }

        private void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            PasswordModifySwitchFragment modifySwitchFragment = new PasswordModifySwitchFragment(e.Position);
            modifySwitchFragment.onDelete += DeletePasswordDialog;
            modifySwitchFragment.onEdit += UpdatePasswordDialog;

            modifySwitchFragment.Show(transaction, "modify switch");
        }

        private void GetPassword(int id)
        {
            Response res = CommunicationManger.GetInstance().RequestGetPassword(id);

            if (res.Success)
            {
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                PasswordDisplayFragment displayFragment = new PasswordDisplayFragment(res.Name, res.Password);

                displayFragment.Show(transaction, "password display");
            }
            else
                this.RunOnUiThread(
                        () => { Toast.MakeText(this, "Could not get password\n" + res.ErrorMsg, ToastLength.Long).Show(); });
        }

        private void DeletePassword(object sender, ConfirmDeleteEventArgs confirmDeleteEventArgs)
        {
            new Thread(() =>
            {
                Response res = CommunicationManger.GetInstance().RequestDeletePassword(confirmDeleteEventArgs.passID);

                if (res.Success)
                    items.Remove(items.Find(x => x.ID == confirmDeleteEventArgs.passID));
                else
                    this.RunOnUiThread(
                        () => { Toast.MakeText(this, "Could not delete password\n" + res.ErrorMsg, ToastLength.Long).Show(); });
            }).Start();
        }

        private void AddPassword(object sender, EntryDataPasswordEventArgs addPasswordEventArgs)
        {
            new Thread(() =>
            {
                Response res = CommunicationManger.GetInstance()
                    .RequestAddPassword(addPasswordEventArgs.Name, addPasswordEventArgs.Pass);

                if (res.Success)
                    items.Add(new PasswordEntry(res.ID, addPasswordEventArgs.Name));
                else
                    this.RunOnUiThread(
                        () =>
                        {
                            Toast.MakeText(this, "Could not add password\n" + res.ErrorMsg, ToastLength.Long).Show();
                        });
            }).Start();
        }

        private void UpdatePassword(object sender, EntryDataPasswordEventArgs updatePasswordEventArgs)
        {
            new Thread(() =>
            {
                Response res = CommunicationManger.GetInstance()
                    .RequestUpdatePassword(updatePasswordEventArgs.ID, updatePasswordEventArgs.Name,
                        updatePasswordEventArgs.Pass);

                if (res.Success)
                {
                    int i = items.FindIndex(x => x.ID == res.ID);
                    items[i] = new PasswordEntry(res.ID, updatePasswordEventArgs.Name);
                }
                else
                    this.RunOnUiThread(
                        () => { Toast.MakeText(this, "Could not update password\n" + res.ErrorMsg, ToastLength.Long).Show(); });

            }).Start();
        }
    }
}