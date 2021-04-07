using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.Encryption
{
    public class BouncyCastleEncryption : IBouncyCastleEncryption
    {
        private readonly Encoding _encoding;
        private readonly IBlockCipher _blockCipher;
        private PaddedBufferedBlockCipher _cipher;
        private IBlockCipherPadding _padding;
        private string _encryptionKey = "1234567890123456";


        /// <summary>
        /// 
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="blockCipher">Org.BouncyCastle.Crypto.Engines</param>
        public BouncyCastleEncryption(Encoding encoding, IBlockCipher blockCipher)
        {
            _blockCipher = blockCipher;
            _encoding = encoding;
        }

        public void SetPadding(IBlockCipherPadding padding)
        {
            if (padding != null)
                _padding = padding;
        }

        public async Task<string> EncryptAsAsync(string plainText)
        {
            byte[] result = await BouncyCastleCryptoAsAsync(true, _encoding.GetBytes(plainText));
            return await Task.FromResult(Convert.ToBase64String(result));
        }

        public async Task<string> DecryptAsAsync(string cipher)
        {
            byte[] result = await BouncyCastleCryptoAsAsync(false, Convert.FromBase64String(cipher));
            return await Task.FromResult(_encoding.GetString(result));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="forEncrypt"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="CryptoException"></exception>
        private async Task<byte[]> BouncyCastleCryptoAsAsync(bool forEncrypt, byte[] input)
        {
            try
            {
                _cipher = _padding == null ? new PaddedBufferedBlockCipher(_blockCipher) : new PaddedBufferedBlockCipher(_blockCipher, _padding);
                byte[] keyByte = _encoding.GetBytes(_encryptionKey);
                _cipher.Init(forEncrypt, new KeyParameter(keyByte));
                //return _cipher.DoFinal(input);
                return await Task.FromResult(_cipher.DoFinal(input));
            }
            catch (CryptoException ex)
            {
                throw ex;
            }
        }
    }
}
