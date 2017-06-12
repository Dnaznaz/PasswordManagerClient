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
using Android.Bluetooth;

namespace PasswordManagerClient
{
    class CommunicationManger
    {
        private static CommunicationManger instance = null;
        private BluetoothAdapter bluetoothAdapter = null;

        private CommunicationManger()
        {
            InitBluetooth();
        }

        public static CommunicationManger GetInstance()
        {
            if (instance == null)
                instance = new CommunicationManger();

            return instance;
        }

        public void InitBluetooth()
        {
            bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
        }

        public bool IsBluetoothExists()
        {
            return bluetoothAdapter != null;
        }

        public bool IsBluetoothOn()
        {
            return bluetoothAdapter.IsEnabled;
        }

        public bool RequestDeletePassword(int passID)
        {
            return false;
            throw new NotImplementedException();
        }
    }
}