using Org.BouncyCastle.Crypto.Paddings;
using System.Threading.Tasks;

namespace POS.Utilities.Encryption
{
    public interface IBouncyCastleEncryption
    {
        Task<string> DecryptAsAsync(string cipher);
        Task<string> EncryptAsAsync(string plainText);
        void SetPadding(IBlockCipherPadding padding);
    }
}