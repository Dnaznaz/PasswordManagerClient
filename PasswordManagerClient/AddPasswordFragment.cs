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

namespace PasswordManagerClient
{
    public class EntryDataPasswordEventArgs : EventArgs
    {
        public int ID { get; }
        public string Name { get; }
        public string Pass { get; }

        public EntryDataPasswordEventArgs(int id, string name, string pass) : base()
        {
            ID = id;
            Name = name;
            Pass = pass;
        }
    }

    class AddPasswordFragment : DialogFragment
    {
        private EditText txtName, txtPass;
        private int id;
        private string name;

        public AddPasswordFragment(int id=-1, string name="")
        {
            this.id = id;
            this.name = name;
        }

        public event EventHandler<EntryDataPasswordEventArgs> onConfirm;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.AddPassword, container, false);

            txtName = view.FindViewById<EditText>(Resource.Id.txtName);
            txtPass = view.FindViewById<EditText>(Resource.Id.txtPass);
            var okBtn = view.FindViewById<Button>(Resource.Id.okBtn);
            var cancelBtn = view.FindViewById<Button>(Resource.Id.cancelBtn);

            txtName.Text = name;

            okBtn.Click += RequestAddPassword;
            cancelBtn.Click += CancelDelete;

            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
        }

        private void RequestAddPassword(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string pass = txtPass.Text;

            if (string.IsNullOrEmpty(name))
            {
                Toast.MakeText(this.Activity, "Name is empty, please fill it", ToastLength.Long).Show();
                return;
            }
            if (string.IsNullOrEmpty(pass))
            {
                Toast.MakeText(this.Activity, "Password is empty, please fill it", ToastLength.Long).Show();
                return;
            }

            onConfirm.Invoke(this, new EntryDataPasswordEventArgs(id, name, pass));
            this.Dismiss();
        }

        private void CancelDelete(object sender, EventArgs e)
        {
            this.Dismiss();
        }
    }
}