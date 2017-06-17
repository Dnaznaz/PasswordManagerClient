using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace PasswordManagerClient
{
    public class ConfirmDeleteEventArgs : EventArgs
    {
        public int passID { get; }

        public ConfirmDeleteEventArgs(int id) : base()
        {
            passID = id;
        }
    }

    public class DeleteConfirmFragment : DialogFragment
    {
        private int id;

        public DeleteConfirmFragment(int id)
        {
            this.id = id;
        }

        public event EventHandler<ConfirmDeleteEventArgs> onConfirmChose;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.DeleteConfirm, container, false);

            var okBtn = view.FindViewById<Button>(Resource.Id.okBtn);
            var cancelBtn = view.FindViewById<Button>(Resource.Id.cancelBtn);

            okBtn.Click += RequestDeletePass;
            cancelBtn.Click += CancelDelete;

            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
        }

        private void RequestDeletePass(object sender, EventArgs e)
        {
            onConfirmChose.Invoke(this, new ConfirmDeleteEventArgs(id));
            this.Dismiss();
        }

        private void CancelDelete(object sender, EventArgs e)
        {
            this.Dismiss();
        }
    }
}