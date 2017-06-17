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
    class PasswordDisplayFragment : DialogFragment
    {
        private string name, pass;

        public PasswordDisplayFragment(string name, string pass)
        {
            this.name = name;
            this.pass = pass;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.PasswordDisplay, container, false);

            var txtName = view.FindViewById<TextView>(Resource.Id.txtName);
            var txtPass = view.FindViewById<TextView>(Resource.Id.txtPass);

            txtName.Text = name;
            txtPass.Text = pass;

            txtPass.LongClick += TxtPassOnLongClick;

            return view;
        }

        private void TxtPassOnLongClick(object sender, View.LongClickEventArgs longClickEventArgs)
        {
            ClipboardInterface.GetInstance().CopyToClipboard(pass);

            Toast.MakeText(this.Activity, "copied", ToastLength.Short).Show();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
        }
    }
}