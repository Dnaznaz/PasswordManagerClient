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
    class PasswordEntry
    {
        public int ID { get; }
        public string Name { get; }

        public PasswordEntry(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}