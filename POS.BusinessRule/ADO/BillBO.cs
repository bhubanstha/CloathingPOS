using POS.Data;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace POS.BusinessRule.ADO
{
    public class BillBO
    {

        public Task<List<Bill>> GetAll()
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GetAllBill");
                DataTable tbl = await DataAccess.ExecuteReaderCommandAsync(cmd);
                List<Bill> bills = new List<Bill>();
                foreach (DataRow row in tbl.Rows)
                {
                    bills.Add(new Bill
                    {
                        Id = (long)row["Id"],
                        BillDate = (DateTime)row["BillDate"],
                        VAT = (decimal)row["VAT"],
                        BillTo = row["BillTo"].ToString(),
                        BillingAddress = row["BillingAddress"].ToString(),
                        BillingPAN = row["BillingPAN"].ToString(),
                        BranchId = row.IsNull("BranchId") ? 0 : (long)row["BranchId"],
                        UserId = row.IsNull("UserId") ? 0 : (long)row["UserId"]
                    });
                }
                return bills;
            });            
        }

        public Task<int> GetRemainingSalesCount(long BillNo)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GetBillSalesCount");
                cmd.Parameters.AddWithValue("@BillId", BillNo);
                int i = await DataAccess.ExecuteScalarCommandAsync<int>(cmd);
                return i;
            });
        }

        public Task<long> GetNewBillNo()
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GenerateNewBillNo");
                long bn = await DataAccess.ExecuteScalarCommandAsync<long>(cmd);
                return bn;
            });
        }

        public Task<long> CreateNewBill(Bill bill)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("SaveBill");
                cmd.Parameters.AddWithValue("@BillDate", bill.BillDate);
                cmd.Parameters.AddWithValue("@VAT", bill.VAT);
                cmd.Parameters.AddWithValue("@BillTo", bill.BillTo);
                cmd.Parameters.AddWithValue("@BillingAddress", bill.BillingAddress);
                cmd.Parameters.AddWithValue("@BillingPAN", bill.BillingPAN);
                cmd.Parameters.AddWithValue("@BranchId", bill.BranchId);
                cmd.Parameters.AddWithValue("@UserId", bill.UserId);
                cmd.Parameters.AddWithValue("@CalculateVAT", bill.CalculateVAT);
                long i = await DataAccess.ExecuteScalarCommandAsync<long>(cmd);
                return i;
            });
        }

        public Task<Bill> GetById(long billId)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GetBillById");
                cmd.Parameters.AddWithValue("@Id", billId);
                DataTable table = await DataAccess.ExecuteReaderCommandAsync(cmd);
                
                Bill b = new Bill
                {
                    Id = (long)table.Rows[0]["Id"],
                    BillDate = (DateTime)table.Rows[0]["BillDate"],
                    VAT = (decimal)table.Rows[0]["VAT"],
                    BillTo = table.Rows[0]["BillTo"].ToString(),
                    BillingAddress = table.Rows[0]["BillingAddress"].ToString(),
                    BillingPAN = table.Rows[0]["BillingPAN"].ToString(),
                    BranchId = table.Rows[0].IsNull("BranchId") ? 0 : (long)table.Rows[0]["BranchId"],
                    UserId = table.Rows[0].IsNull("UserId") ? 0 : (long)table.Rows[0]["UserId"]
                };
                return b;
            });
        }

        public Task<int> Update(Bill bill)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("UpdateBill");
                cmd.Parameters.AddWithValue("@Id", bill.Id);
                cmd.Parameters.AddWithValue("@BillDate", bill.BillDate);
                cmd.Parameters.AddWithValue("@VAT", bill.VAT);
                cmd.Parameters.AddWithValue("@BillTo", bill.BillTo);
                cmd.Parameters.AddWithValue("@BillingAddress", bill.BillingAddress);
                cmd.Parameters.AddWithValue("@BillingPAN", bill.BillingPAN);
                cmd.Parameters.AddWithValue("@BranchId", bill.BranchId);
                cmd.Parameters.AddWithValue("@UserId", bill.UserId);
                cmd.Parameters.AddWithValue("@CalculateVAT", bill.CalculateVAT);
                int i = await DataAccess.ExecuteNonQueryAsync(cmd);
                return i;
            });
        }

        public Task<int> Remove(long id)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("RemoveBill");
                cmd.Parameters.AddWithValue("@Id", id);
                int i = await DataAccess.ExecuteNonQueryAsync(cmd);
                return i; 
            });
        }
    }
}
