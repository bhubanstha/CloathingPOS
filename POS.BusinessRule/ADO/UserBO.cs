using POS.Data;
using POS.Model;
using POS.Utilities.Encryption;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace POS.BusinessRule
{
    public class UserBO
    {
        private IBouncyCastleEncryption bouncyCastleEncryption;
        public UserBO(IBouncyCastleEncryption encryption)
        {
            bouncyCastleEncryption = encryption;
        }

        public Task<List<User>> GetAllUser(string myUserName, long branchId)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GetAllUser");
                cmd.Parameters.AddWithValue("@UserName", myUserName);
                cmd.Parameters.AddWithValue("@BranchId", branchId);
                DataTable tbl = await DataAccess.ExecuteReaderCommandAsync(cmd);
                List<User> users = new List<User>();
                foreach (DataRow row in tbl.Rows)
                {
                    DateTime? deactive = null;
                    DateTime? lastpwdChange = null;
                    if (!row.IsNull("DeactivationDate"))
                    {
                        deactive = (DateTime)row["DeactivationDate"];
                    }

                    if (!row.IsNull("LastPasswordChangeDate"))
                    {
                        lastpwdChange = (DateTime)row["LastPasswordChangeDate"];
                    }

                    users.Add(new User
                    {
                        Id = (long)row["Id"],
                        UserName = row["UserName"].ToString(),
                        DisplayName = row["DisplayName"].ToString(),
                        Password = row["Password"].ToString(),
                        IsAdmin = (bool)row["IsAdmin"],
                        IsActive = (bool)row["IsActive"],
                        PromptForPasswordReset = (bool)row["PromptForPasswordReset"],
                        ProfileImage = row["ProfileImage"].ToString(),
                        CreatedDate = (DateTime)row["CreatedDate"],
                        DeactivationDate = deactive,
                        LastPasswordChangeDate = lastpwdChange,
                        BranchId = (long)row["BranchId"],
                        CanAccessAllBranch = (bool)row["CanAccessAllBranch"],
                        Branch = await new BranchBO().GetById((long)row["BranchId"])
                    }); ;
                }
                return users;
            });
        }


        public Task<User> Login(string userName, string password, long branchId)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("LoginUser");
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@BranchId", branchId);
                DataTable tbl = await DataAccess.ExecuteReaderCommandAsync(cmd);
                if (tbl.Rows.Count > 0)
                {
                    DateTime? deactive = null;
                    DateTime? lastpwdChange = null;
                    if (!tbl.Rows[0].IsNull("DeactivationDate"))
                    {
                        deactive = (DateTime)tbl.Rows[0]["DeactivationDate"];
                    }

                    if (!tbl.Rows[0].IsNull("LastPasswordChangeDate"))
                    {
                        lastpwdChange = (DateTime)tbl.Rows[0]["LastPasswordChangeDate"];
                    }
                    User u = new User
                    {
                        Id = (long)tbl.Rows[0]["Id"],
                        UserName = tbl.Rows[0]["UserName"].ToString(),
                        DisplayName = tbl.Rows[0]["DisplayName"].ToString(),
                        Password = tbl.Rows[0]["Password"].ToString(),
                        IsAdmin = (bool)tbl.Rows[0]["IsAdmin"],
                        IsActive = (bool)tbl.Rows[0]["IsActive"],
                        PromptForPasswordReset = (bool)tbl.Rows[0]["PromptForPasswordReset"],
                        ProfileImage = tbl.Rows[0]["ProfileImage"].ToString(),
                        CreatedDate = (DateTime)tbl.Rows[0]["CreatedDate"],
                        DeactivationDate = deactive,
                        LastPasswordChangeDate = lastpwdChange,
                        BranchId = (long)tbl.Rows[0]["BranchId"],
                        CanAccessAllBranch = (bool)tbl.Rows[0]["CanAccessAllBranch"],
                        Branch = await new BranchBO().GetById((long)tbl.Rows[0]["BranchId"])
                    };
                    return u;
                }
                return null;
            });
        }

        public Task<long> SaveUser(User u)
        {
            return Task.Run(async () =>
            {
                u.Password = await EncryptPassword(u.Password);
                SqlCommand cmd = DataAccess.CreateCommand("SaveUser");
                cmd.Parameters.AddWithValue("@UserName", u.UserName);
                cmd.Parameters.AddWithValue("@DisplayName", u.DisplayName);
                cmd.Parameters.AddWithValue("@Password", u.Password);
                cmd.Parameters.AddWithValue("@IsAdmin", u.IsAdmin);
                cmd.Parameters.AddWithValue("@IsActive", u.IsActive);
                cmd.Parameters.AddWithValue("@PromptForPasswordReset", u.PromptForPasswordReset);
                cmd.Parameters.AddWithValue("@ProfileImage", u.ProfileImage);
                cmd.Parameters.AddWithValue("@CreatedDate", u.CreatedDate);
                cmd.Parameters.AddWithValue("@DeactivationDate", u.DeactivationDate);
                cmd.Parameters.AddWithValue("@LastPasswordChangeDate", u.LastPasswordChangeDate);
                cmd.Parameters.AddWithValue("@BranchId", u.BranchId);
                cmd.Parameters.AddWithValue("@CanAccessAllBranch", u.CanAccessAllBranch);
                long i = await DataAccess.ExecuteScalarCommandAsync<long>(cmd);

                return i;
            });
        }

        public Task<int>UpdateUser(User u)
        {
            return Task.Run(async () =>
            {
                u.Password = await EncryptPassword(u.Password);
                SqlCommand cmd = DataAccess.CreateCommand("UpdateUser");
                cmd.Parameters.AddWithValue("@Id", u.Id);
                cmd.Parameters.AddWithValue("@UserName", u.UserName);
                cmd.Parameters.AddWithValue("@DisplayName", u.DisplayName);
                cmd.Parameters.AddWithValue("@Password", u.Password);
                cmd.Parameters.AddWithValue("@IsAdmin", u.IsAdmin);
                cmd.Parameters.AddWithValue("@IsActive", u.IsActive);
                cmd.Parameters.AddWithValue("@PromptForPasswordReset", u.PromptForPasswordReset);
                cmd.Parameters.AddWithValue("@ProfileImage", u.ProfileImage);
                cmd.Parameters.AddWithValue("@CreatedDate", u.CreatedDate);
                cmd.Parameters.AddWithValue("@DeactivationDate", u.DeactivationDate);
                cmd.Parameters.AddWithValue("@LastPasswordChangeDate", u.LastPasswordChangeDate);
                cmd.Parameters.AddWithValue("@BranchId", u.BranchId);
                cmd.Parameters.AddWithValue("@CanAccessAllBranch", u.CanAccessAllBranch);
                int i = await DataAccess.ExecuteNonQueryAsync(cmd);

                return i;
            });
        }

        public Task<User> GetUserFromUserName(string userName)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GetUserByUserName");
                cmd.Parameters.AddWithValue("@UserName", userName);
                DataTable tbl = await DataAccess.ExecuteReaderCommandAsync(cmd);
                if (tbl.Rows.Count > 0)
                {
                    DateTime? deactive = null;
                    DateTime? lastpwdChange = null;
                    if (!tbl.Rows[0].IsNull("DeactivationDate"))
                    {
                        deactive = (DateTime)tbl.Rows[0]["DeactivationDate"];
                    }

                    if (!tbl.Rows[0].IsNull("LastPasswordChangeDate"))
                    {
                        lastpwdChange = (DateTime)tbl.Rows[0]["LastPasswordChangeDate"];
                    }
                    User u = new User
                    {
                        Id = (long)tbl.Rows[0]["Id"],
                        UserName = tbl.Rows[0]["UserName"].ToString(),
                        DisplayName = tbl.Rows[0]["DisplayName"].ToString(),
                        Password = tbl.Rows[0]["Password"].ToString(),
                        IsAdmin = (bool)tbl.Rows[0]["IsAdmin"],
                        IsActive = (bool)tbl.Rows[0]["IsActive"],
                        PromptForPasswordReset = (bool)tbl.Rows[0]["PromptForPasswordReset"],
                        ProfileImage = tbl.Rows[0]["ProfileImage"].ToString(),
                        CreatedDate = (DateTime)tbl.Rows[0]["CreatedDate"],
                        DeactivationDate = deactive,
                        LastPasswordChangeDate = lastpwdChange,
                        BranchId = (long)tbl.Rows[0]["BranchId"],
                        CanAccessAllBranch = (bool)tbl.Rows[0]["CanAccessAllBranch"],
                        Branch = await new BranchBO().GetById((long)tbl.Rows[0]["BranchId"])
                    };
                    return u;
                }
                return null;
            });
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

    }
}
