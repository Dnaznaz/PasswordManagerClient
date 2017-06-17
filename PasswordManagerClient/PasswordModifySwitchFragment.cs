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
    public class PasswordModifyEventArgs : EventArgs
    {
        public int Position { get; }

        public PasswordModifyEventArgs(int pos) : base()
        {
            Position = pos;
        }
    }

    class PasswordModifySwitchFragment : DialogFragment
    {
        private int pos;

        public PasswordModifySwitchFragment(int pos)
        {
            this.pos = pos;
        }

        public event EventHandler<PasswordModifyEventArgs> onDelete;
        public event EventHandler<PasswordModifyEventArgs> onEdit;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.PasswordModifySwitch, container, false);
            
            var deletBtn = view.FindViewById<Button>(Resource.Id.deleteBtn);
            var editBtn = view.FindViewById<Button>(Resource.Id.editBtn);

            deletBtn.Click += InvokDelete;
            editBtn.Click += InvokeEdit;

            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
        }

        private void InvokeEdit(object sender, EventArgs e)
        {
            onEdit.Invoke(this, new PasswordModifyEventArgs(pos));
            this.Dismiss();
        }

        private void InvokDelete(object sender, EventArgs e)
        {
            onDelete.Invoke(this, new PasswordModifyEventArgs(pos));
            this.Dismiss();
        }
    }
}