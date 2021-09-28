using POS.Data;
using POS.Model;
using POS.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace POS.BusinessRule
{
    public class SalesBO
    {
        public Task<List<Sales>> GetAll()
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GetAllSales");
                DataTable tbl = await DataAccess.ExecuteReaderCommandAsync(cmd);
                List<Sales> sales = new List<Sales>();
                foreach (DataRow row in tbl.Rows)
                {
                    sales.Add(new Sales
                    {
                        Id = (long)row["Id"],
                        SalesQuantity = (int)row["SalesQuantity"],
                        PurchaseRate = (decimal)row["PurchaseRate"],
                        Discount = (decimal)row["Discount"],
                        ProductId = (long)row["ProductId"],
                        BillNo = (long)row["BillNo"],
                        SalesRate = (decimal)row["SalesRate"],
                        Inventory = await new InventoryBO().GetById((long)row["ProductId"]),
                        Bill = await new BillBO().GetById((long)row["BillNo"])
                    });
                }
                return sales;
            });
        }


        public Task<List<Sales>> GetAllOnDate(DateTime billingdate, Int64 branchId)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GetSalesOnDate");
                cmd.Parameters.AddWithValue("@SalesDate", billingdate);
                cmd.Parameters.AddWithValue("@BranchId", branchId);
                DataTable tbl = await DataAccess.ExecuteReaderCommandAsync(cmd);
                List<Sales> sales = new List<Sales>();
                foreach (DataRow row in tbl.Rows)
                {
                    sales.Add(new Sales
                    {
                        Id = (long)row["Id"],
                        SalesQuantity = (int)row["SalesQuantity"],
                        PurchaseRate = (decimal)row["PurchaseRate"],
                        Discount = (decimal)row["Discount"],
                        ProductId = (long)row["ProductId"],
                        BillNo = (long)row["BillNo"],
                        SalesRate = (decimal)row["SalesRate"],
                        Inventory = await new InventoryBO().GetById((long)row["ProductId"]),
                        Bill = await new BillBO().GetById((long)row["BillNo"])
                    });
                }
                return sales;
            });
        }


        public Task<List<Sales>> GetCustomerPurchaseHitory(Int64 customerId)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GetCustomerPurchaseHistory");
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                DataTable tbl = await DataAccess.ExecuteReaderCommandAsync(cmd);
                List<Sales> sales = new List<Sales>();
                foreach (DataRow row in tbl.Rows)
                {
                    Sales s = new Sales
                    {
                        Id = (long)row["Id"],
                        SalesQuantity = (int)row["SalesQuantity"],
                        PurchaseRate = (decimal)row["PurchaseRate"],
                        Discount = (decimal)row["Discount"],
                        ProductId = (long)row["ProductId"],
                        BillNo = (long)row["BillNo"],
                        SalesRate = (decimal)row["SalesRate"],
                        Inventory = await new InventoryBO().GetById((long)row["ProductId"]),
                        Bill = await new BillBO().GetById((long)row["BillNo"]),

                    };
                    s.Bill.Branch = await new BranchBO().GetById((long)row["BranchId"]);
                    sales.Add(s);
                }
                return sales;
            });
        }

        public Task<int> CheckoutSales(Sales item)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("CheckoutSales");
                cmd.Parameters.AddWithValue("@SalesQuantity", item.SalesQuantity);
                cmd.Parameters.AddWithValue("@PurchaseRate", item.PurchaseRate);
                cmd.Parameters.AddWithValue("@Discount", item.Discount);
                cmd.Parameters.AddWithValue("@ProductId", item.ProductId);
                cmd.Parameters.AddWithValue("@BillNo", item.BillNo);
                cmd.Parameters.AddWithValue("@SalesRate", item.SalesRate);
                int i = await DataAccess.ExecuteNonQueryAsync(cmd);
                return i;
            });
        }

        public Task<int> Update(Sales item)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("UpdateSales");
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@SalesQuantity", item.SalesQuantity);
                cmd.Parameters.AddWithValue("@PurchaseRate", item.PurchaseRate);
                cmd.Parameters.AddWithValue("@Discount", item.Discount);
                cmd.Parameters.AddWithValue("@ProductId", item.ProductId);
                cmd.Parameters.AddWithValue("@BillNo", item.BillNo);
                cmd.Parameters.AddWithValue("@SalesRate", item.SalesRate);
                int i = await DataAccess.ExecuteNonQueryAsync(cmd);
                return i;
            });
        }


        public Task<int> Remove(Sales item)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("RemoveSales");
                cmd.Parameters.AddWithValue("@Id", item.Id);
                int i = await DataAccess.ExecuteNonQueryAsync(cmd);
                return i;
            });
        }

        public Task<List<Sales>> GetSalesByBillNo(Int64 BillNo, Int64 branchId)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GetSalesByBill");
                cmd.Parameters.AddWithValue("@BillNo", BillNo);
                cmd.Parameters.AddWithValue("@BranchId", branchId);
                DataTable tbl = await DataAccess.ExecuteReaderCommandAsync(cmd);
                List<Sales> sales = new List<Sales>();
                foreach (DataRow row in tbl.Rows)
                {
                    sales.Add(new Sales
                    {
                        Id = (long)row["Id"],
                        SalesQuantity = (int)row["SalesQuantity"],
                        PurchaseRate = (decimal)row["PurchaseRate"],
                        Discount = (decimal)row["Discount"],
                        ProductId = (long)row["ProductId"],
                        BillNo = (long)row["BillNo"],
                        SalesRate = (decimal)row["SalesRate"],
                        Inventory = await new InventoryBO().GetById((long)row["ProductId"]),
                        Bill = await new BillBO().GetById((long)row["BillNo"])
                    });
                }
                return sales;
            });
        }

        public Task<List<Sales>> GetSalesHistory(Int64 itemId)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GetSalesHistory");
                cmd.Parameters.AddWithValue("@ProductId", itemId);
                DataTable tbl = await DataAccess.ExecuteReaderCommandAsync(cmd);
                List<Sales> sales = new List<Sales>();
                foreach (DataRow row in tbl.Rows)
                {
                    sales.Add(new Sales
                    {
                        Id = 0,
                        SalesQuantity = (int)row["SalesQuantity"],
                        PurchaseRate = 0,
                        Discount = 0,
                        ProductId = itemId,
                        BillNo = 0,
                        SalesRate = 0,
                        Bill = new Bill
                        {
                            BillDate = (DateTime)row["BillDate"]
                        }
                    });
                }
                return sales;
            });
        }

        public Task<List<ProfitLossReport>> GetProfitLoss(int year, int month)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GetSalesStat");
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@Month", month);
                DataTable tbl = await DataAccess.ExecuteReaderCommandAsync(cmd);
                List<ProfitLossReport> salesReport = new List<ProfitLossReport>();
                foreach (DataRow row in tbl.Rows)
                {
                    salesReport.Add(new ProfitLossReport
                    {
                        BillingDate = (DateTime)row["BillDate"],
                        Category = row["Category"].ToString(),
                        Brand = row["Brand"].ToString(),
                        ItemName = row["ItemName"].ToString(),
                        Color = row["Color"].ToString(),
                        ItemCode = row["Code"].ToString(),
                        Size = row["Size"].ToString(),
                        SalesQty = (int)row["SalesQuantity"],
                        PurchaseTotal = (decimal)row["PurchaseTotal"],
                        SalesTotal = (decimal)row["SalesTotal"],
                        Profit = (decimal)row["SalesTotal"] - (decimal)row["PurchaseTotal"]
                    });
                }
                return salesReport;
            });
        }
    }
}
