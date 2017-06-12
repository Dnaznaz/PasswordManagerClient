using Android.App;
using Android.Bluetooth;
using Android.Widget;
using Android.OS;

namespace PasswordManagerClient
{
    [Activity(Label = "PasswordManagerClient", MainLauncher = false, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private BluetoothAdapter bluetoothAdapter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            CommunicationManger manager = CommunicationManger.GetInstance();
            if (!manager.IsBluetoothExists())
            {
                
            }
            else if (!manager.IsBluetoothOn())
            {
                
            }
            
            //TODO search server
        }
    }
}

