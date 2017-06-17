using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PasswordManagerClient
{
    public class KeyPair
    {
        public RSAParameters PublicKey { get; }
        public RSAParameters PrivateKey { get; }

        public KeyPair(RSAParameters publicKey, RSAParameters privateKey)
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
        }
    }

    class CryptoManager
    {
        public static KeyPair GetKeys()
        {
            var csp = new RSACryptoServiceProvider(2048);

            return new KeyPair(csp.ExportParameters(false), csp.ExportParameters(true));
        }
    }
}