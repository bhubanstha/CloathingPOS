using Org.BouncyCastle.Crypto.Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.Encryption
{
    public class PasswordEncryptDecryptor_RemoveThis
    {
        static BouncyCastleEncryption encryption;

        static PasswordEncryptDecryptor_RemoveThis()
        {
            encryption  = new BouncyCastleEncryption();
        }
        public async static Task<string> Encrypt(string text)
        {
            return await encryption.EncryptAsAsync(text);
        }

        public async static Task<string> Decrypt(string encyptedText)
        {
            return await encryption.DecryptAsAsync(encyptedText);
        }
    }
}
