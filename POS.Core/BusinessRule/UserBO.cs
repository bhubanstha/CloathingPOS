﻿using POS.Core.Model;
using POS.Core.Repo;
using POS.Core.Utilities.Encryption;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Core.BusinessRule
{
    public class UserBO
    {
        private IGenericDataRepository<User> genericDataRepository;
        private IBouncyCastleEncryption bouncyCastleEncryption;

        public UserBO()
        {
            genericDataRepository = new DataRepository<User>(new POSDataContext());
            bouncyCastleEncryption = new BouncyCastleEncryption(Encoding.UTF8);
        }

        public bool HasChanges()
        {
            return true;
            //return genericDataRepository.HasChanges();
        }

        public List<User> GetAllUser(string myUserName)
        {
            List<User> users =  genericDataRepository
                .GetAll()
                .Where(x => x.UserName != "sysadmin" && x.UserName != myUserName)
                .ToList();
            return users;
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
