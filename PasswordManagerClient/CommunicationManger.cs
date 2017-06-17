using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Bluetooth;
using Java.Util;

namespace PasswordManagerClient
{
    public class Response
    {
        public bool Success { get; }
        public string ErrorMsg { get; }
        public int ID { get; }
        public string Name { get; }
        public string Password { get; }

        public Response(bool success, int id = -1, string name = "", string pass = "", string errorMsg = "")
        {
            Success = success;
            ID = id;
            Name = name;
            Password = pass;
            ErrorMsg = errorMsg;
        }
    }

    class CommunicationManger
    {
        private string SERVER_NAME = "PassServer";

        private static CommunicationManger instance = null;
        private BluetoothAdapter bluetoothAdapter = null;
        private BluetoothSocket connectedDevice;
        private bool auth = false;
        private Stream outStream, inStream;
        private RSAParameters privateKey, publicKey;
        private string serverKey;

        public bool Connected { get; private set; }
        public bool Searching { get; set; }

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

        public bool TryConnect(double sec)
        {
            Stopwatch s = new Stopwatch();
            Searching = true;
            
            s.Start();
            while (Searching && s.Elapsed < TimeSpan.FromSeconds(sec))
            {
                var deviceList = bluetoothAdapter.BondedDevices;
                if (deviceList != null)
                {
                    foreach (var device in deviceList)
                    {
                        if (device.Name == SERVER_NAME)
                        {
                            connectedDevice = device.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));
                            Connected = true;
                            break;
                        }
                    }
                }
            }

            Searching = false;
            s.Stop();
            return false;
        }

        public bool StartAuthorize(string pass)
        {
            try
            {
                connectedDevice.Connect();
                outStream = connectedDevice.OutputStream;
                inStream = connectedDevice.InputStream;

                byte[] buffer = new byte[1024];

                while (!inStream.IsDataAvailable()) { }
                inStream.Read(buffer, 0, 1024);
                string data2 = Encoding.UTF8.GetString(buffer);

                if (data2 == "NO-OWNER")
                {
                    byte[] msg = Encoding.UTF8.GetBytes("COMMAND=SET-USER PASSWORD=" + pass);
                    outStream.Write(msg, 0, msg.Length);
                }
                else if (data2 == "AUTHORIZE")
                {
                    byte[] msg = Encoding.UTF8.GetBytes(pass);
                    outStream.Write(msg, 0, msg.Length);
                }

                while (!inStream.IsDataAvailable()) { }
                inStream.Read(buffer, 0, 1024);
                string data3 = Encoding.UTF8.GetString(buffer);

                if (data3 == "AUTHORIZED")
                {
                    auth = true;
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        public Response RequestDeletePassword(int passID)
        {
            throw new NotImplementedException();
        }

        internal Response RequestAddPassword(string name, string pass)
        {
            throw new NotImplementedException();
        }

        internal Response RequestUpdatePassword(int iD, string name, string pass)
        {
            throw new NotImplementedException();
        }

        internal Response RequestGetPassword(int id)
        {
            throw new NotImplementedException();
        }

        public void close()
        {

        }
    }
}