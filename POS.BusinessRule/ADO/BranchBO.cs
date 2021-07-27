using POS.Data;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.BusinessRule.ADO
{
    public class BranchBO
    {
        public Task<List<Branch>> GetAll()
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GetAllBranch");
                DataTable tbl = await DataAccess.ExecuteReaderCommandAsync(cmd);
                List<Branch> branches = new List<Branch>();
                foreach (DataRow row in tbl.Rows)
                {
                    branches.Add(new Branch
                    {
                        Id = (long)row["Id"],
                        BranchName = row["BranchName"].ToString(),
                        BranchAddress = row["BranchAddress"].ToString(),
                        ShopId = (long)row["ShopId"]
                    });
                }
                return branches;
            });
        }

        public Task<List<Branch>> GetAll(long branchId, bool canAccessAllBranch)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GetBranchByAccessLevel");
                cmd.Parameters.AddWithValue("@Id", branchId);
                cmd.Parameters.AddWithValue("@CanAccessAllBranch", canAccessAllBranch);
                DataTable tbl = await DataAccess.ExecuteReaderCommandAsync(cmd);
                List<Branch> branches = new List<Branch>();
                foreach (DataRow row in tbl.Rows)
                {
                    branches.Add(new Branch
                    {
                        Id = (long)row["Id"],
                        BranchName = row["BranchName"].ToString(),
                        BranchAddress = row["BranchAddress"].ToString(),
                        ShopId = (long)row["ShopId"]
                    });
                }
                return branches;
            });
        }

        public Task<Branch> GetById(long Id)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GetBranchById");
                cmd.Parameters.AddWithValue("@Id", Id);
                DataTable table = await DataAccess.ExecuteReaderCommandAsync(cmd);
                Branch b = new Branch
                {
                    Id = (long)table.Rows[0]["Id"],
                    BranchName = table.Rows[0]["BranchName"].ToString(),
                    BranchAddress = table.Rows[0]["BranchAddress"].ToString(),
                    ShopId = (long)table.Rows[0]["ShopId"]
                };
                return b;
            });
        }

        public Task<long> Save(Branch branch)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("SaveBranch");
                cmd.Parameters.AddWithValue("@BranchName", branch.BranchName);
                cmd.Parameters.AddWithValue("@BranchAddress", branch.BranchAddress);
                cmd.Parameters.AddWithValue("@ShopId", branch.ShopId);
                long i = await DataAccess.ExecuteScalarCommandAsync<long>(cmd);
                return i;
            });
        }

        public Task<int> Update(Branch branch)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("UpdateBranch");
                cmd.Parameters.AddWithValue("@Id", branch.Id);
                cmd.Parameters.AddWithValue("@BranchName", branch.BranchName);
                cmd.Parameters.AddWithValue("@BranchAddress", branch.BranchAddress);
                cmd.Parameters.AddWithValue("@ShopId", branch.ShopId);
                int i = await DataAccess.ExecuteNonQueryAsync(cmd);
                return i;
            });
        }

        public  Task<int> Delete(long Id)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("RemoveBranch");
                cmd.Parameters.AddWithValue("@Id", Id);
                int i = await DataAccess.ExecuteNonQueryAsync(cmd);
                return i;
            });
        }
    }
}
