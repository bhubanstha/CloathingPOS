using Org.BouncyCastle.Crypto.Engines;
using POS.Data;
using POS.Data.Repository;
using POS.Model;
using POS.Utilities.Encryption;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.BusinessRule
{
    public class UserBO
    {
        private IGenericDataRepository<User> genericDataRepository;
        private IBouncyCastleEncryption bouncyCastleEncryption;

        public UserBO(IBouncyCastleEncryption encryption)
        {
            genericDataRepository = new DataRepository<User>(new POSDataContext());
            bouncyCastleEncryption = encryption;
        }

        public bool HasChanges()
        {
            return true;
            //return genericDataRepository.HasChanges();
        }

        public List<User> GetAllUser(string myUserName, Int64 branchId)
        {
            List<User> users =  genericDataRepository
                .GetAll()
                .Where(x => x.UserName != "sysadmin" && x.UserName != myUserName && x.BranchId == branchId)
                .Include(i=>i.Branch)
                .ToList();
            return users;
        }

        public User Login(string userName, string password, Int64 branchId)
        {
            return genericDataRepository
                            .GetAll()
                            .Where(f => f.UserName == userName
                                    && f.Password == password
                                    && f.DeactivationDate.ToString() == ""
                                    && (f.BranchId == branchId || f.CanAccessAllBranch == true)
                                    )
                            .FirstOrDefault();
        }

        public async Task<int> SaveUser(User u)
        {
            u.Password = await bouncyCastleEncryption.EncryptAsAsync(u.Password);
            genericDataRepository.Insert(u);
            return await genericDataRepository.SaveAsync();
        }

        public async Task<int> UpdateUser(User u)
        {
            genericDataRepository.Update(u);
            return await genericDataRepository.SaveAsync();
        }

        public async Task<string> DecryptPassword(string encryptedPassword)
        {
            string pwd = await bouncyCastleEncryption.DecryptAsAsync(encryptedPassword);
            return pwd;
        }

        public async Task<string> EncryptPassword(string password)
        {
            string pwd = await bouncyCastleEncryption.EncryptAsAsync(password);
            return pwd;
        }

        public User GetUserFromUserName(string userName)
        {
            return genericDataRepository.GetAll().Where(x => x.UserName == userName).FirstOrDefault();
        }
    }
}
