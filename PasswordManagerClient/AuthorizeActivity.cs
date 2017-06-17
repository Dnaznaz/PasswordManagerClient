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
    [Activity(Label = "AuthorizeActivity")]
    public class AuthorizeActivity : Activity
    {
        private EditText passwordBox;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Authorize);

            passwordBox = FindViewById<EditText>(Resource.Id.passwordBox);
            var okBtn = FindViewById<Button>(Resource.Id.okBtn);

            okBtn.Click += OkBtnOnClick;
        }

        private void OkBtnOnClick(object sender, EventArgs eventArgs)
        {
            var commManager = CommunicationManger.GetInstance();

            if (commManager.Connected)
            {
                if (commManager.StartAuthorize(passwordBox.Text))
                {
                    Intent intent = new Intent(this, typeof(ItemsSelectionActivity));
                    this.StartActivity(intent);
                }
                else
                {
                    Toast.MakeText(this, "Connection refused", ToastLength.Long).Show();
                    Finish();
                }
            }
        }
    }
}