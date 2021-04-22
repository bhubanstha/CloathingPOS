using Org.BouncyCastle.Crypto.Engines;
using POS.Data;
using POS.Data.Repository;
using POS.Model;
using POS.Utilities.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.BusinessRule
{
    public class UserBO
    {
        private IGenericDataRepository<User> genericDataRepository;
        private IBouncyCastleEncryption bouncyCastleEncryption;

        public UserBO()
        {
            genericDataRepository = new DataRepository<User>(new POSDataContext());
            bouncyCastleEncryption = new BouncyCastleEncryption(Encoding.UTF8, new AesEngine());
        }

        public async Task<int> SaveUser(User u)
        {
            u.Password = await bouncyCastleEncryption.EncryptAsAsync(u.Password);
            genericDataRepository.Insert(u);
            return await genericDataRepository.SaveAsync();
        }
    }
}
