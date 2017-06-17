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
    class ClipboardInterface
    {
        private static ClipboardInterface instance = null;
        private ClipboardManager clipboard;

        private ClipboardInterface()
        {
        }

        public static ClipboardInterface GetInstance()
        {
            if (instance == null)
                instance = new ClipboardInterface();

            return instance;
        }

        public void SetClipboardManager(ClipboardManager clipboardManager)
        {
            clipboard = clipboardManager;
        }

        public void CopyToClipboard(string text)
        {
            ClipData clip = ClipData.NewPlainText("password", text);
            clipboard.PrimaryClip = clip;
        }
    }
}