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
    class ListViewAdapter : BaseAdapter<PasswordEntry>
    {
        private List<PasswordEntry> items;
        private Context context;


        public ListViewAdapter(Context context, List<PasswordEntry> items)
        {
            this.items = items;
            this.context = context;
        }

        public override int Count => items.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override PasswordEntry this[int position]
        {
            get { return items[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(context).Inflate(Resource.Layout.ListViewRow, null, false);
            }

            TextView passName = row.FindViewById<TextView>(Resource.Id.passName);
            passName.Text = items[position].Name;

            return row;
        }
    }
}