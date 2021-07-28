using POS.Data;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace POS.BusinessRule
{
    public class InventoryBO
    {
        public Task<int> Save(Inventory inventory)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("SaveInventory");
                cmd.Parameters.AddWithValue("@PurchaseRate", inventory.PurchaseRate);
                cmd.Parameters.AddWithValue("@RetailRate", inventory.RetailRate);
                cmd.Parameters.AddWithValue("@Quantity", inventory.Quantity);
                cmd.Parameters.AddWithValue("@FirstPurchaseDate", inventory.FirstPurchaseDate);
                cmd.Parameters.AddWithValue("@IsDeleted", inventory.IsDeleted);
                cmd.Parameters.AddWithValue("@BranchId", inventory.BranchId);
                cmd.Parameters.AddWithValue("@UserId", inventory.UserId);
                cmd.Parameters.AddWithValue("@Code", inventory.Code);
                cmd.Parameters.AddWithValue("@BarCode", inventory.BarCode);
                cmd.Parameters.AddWithValue("@Name", inventory.Name);
                cmd.Parameters.AddWithValue("@Size", inventory.Size);
                cmd.Parameters.AddWithValue("@Color", inventory.Color);
                cmd.Parameters.AddWithValue("@ColorName", inventory.ColorName);
                cmd.Parameters.AddWithValue("@CategoryId", inventory.CategoryId);
                cmd.Parameters.AddWithValue("@BrandId", inventory.BrandId);
                int i = await DataAccess.ExecuteNonQueryAsync(cmd);
                return i;
            });
        }

        public Task<List<Inventory>> GetAllActiveProducts(long branchId, string productName)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GetAllActiveInventory");
                cmd.Parameters.AddWithValue("@BranchId", branchId);
                cmd.Parameters.AddWithValue("@Name", productName);
                DataTable tbl = await DataAccess.ExecuteReaderCommandAsync(cmd);
                List<Inventory> inventories = new List<Inventory>();
                foreach (DataRow row in tbl.Rows)
                {
                    inventories.Add(new Inventory
                    {
                        Id = (long)row["Id"],
                        FirstPurchaseDate = (DateTime)row["FirstPurchaseDate"],
                        PurchaseRate = (decimal)row["PurchaseRate"],
                        RetailRate = (decimal)row["RetailRate"],
                        Quantity = (int)row["Quantity"],
                        IsDeleted = (bool)row["IsDeleted"],
                        BranchId = row.IsNull("BranchId") ? 0 : (long)row["BranchId"],
                        UserId = row.IsNull("UserId") ? 0 : (long)row["UserId"],
                        Code = row["Code"].ToString(),
                        BarCode = row["BarCode"].ToString(),
                        Name = row["Name"].ToString(),
                        Size = row["Size"].ToString(),
                        Color = row["Color"].ToString(),
                        ColorName = row["ColorName"].ToString(),
                        CategoryId = (long)row["CategoryId"],
                        BrandId = (long)row["BrandId"],
                        Category = await new CategoryBO().GetCategory((long)row["CategoryId"]),
                        Brand = await new BrandBO().GetBrand((long)row["BrandId"])
                    });
                }
                return inventories;
            });
        }

        public Task<Inventory> GetById(long id)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GetInventoryById");
                cmd.Parameters.AddWithValue("@Id", id);
                DataTable tbl = await DataAccess.ExecuteReaderCommandAsync(cmd);
                Inventory inv = new Inventory
                {
                    Id = (long)tbl.Rows[0]["Id"],
                    FirstPurchaseDate = (DateTime)tbl.Rows[0]["FirstPurchaseDate"],
                    PurchaseRate = (decimal)tbl.Rows[0]["PurchaseRate"],
                    RetailRate = (decimal)tbl.Rows[0]["RetailRate"],
                    Quantity = (int)tbl.Rows[0]["Quantity"],
                    IsDeleted = (bool)tbl.Rows[0]["IsDeleted"],
                    BranchId = tbl.Rows[0].IsNull("BranchId") ? 0 : (long)tbl.Rows[0]["BranchId"],
                    UserId = tbl.Rows[0].IsNull("UserId") ? 0 : (long)tbl.Rows[0]["UserId"],
                    Code = tbl.Rows[0]["Code"].ToString(),
                    BarCode = tbl.Rows[0]["BarCode"].ToString(),
                    Name = tbl.Rows[0]["Name"].ToString(),
                    Size = tbl.Rows[0]["Size"].ToString(),
                    Color = tbl.Rows[0]["Color"].ToString(),
                    ColorName = tbl.Rows[0]["ColorName"].ToString(),
                    CategoryId = (long)tbl.Rows[0]["CategoryId"],
                    BrandId = (long)tbl.Rows[0]["BrandId"],
                    Category = await new CategoryBO().GetCategory((long)tbl.Rows[0]["CategoryId"]),
                    Brand = await new BrandBO().GetBrand((long)tbl.Rows[0]["BrandId"])
                };
                return inv;
            });
        }

        public Task<int> RemoveItem(Inventory item)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("RemoveItem");
                cmd.Parameters.AddWithValue("@Id", item.Id);

                int i = await DataAccess.ExecuteNonQueryAsync(cmd);
                return i;
            });
        }


        public Task<int> DeductQuantity(Int64 productId, int deductionQty)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("DeductInventoryQuantity");
                cmd.Parameters.AddWithValue("@Id", productId);
                cmd.Parameters.AddWithValue("@Quantity", deductionQty);

                int i = await DataAccess.ExecuteNonQueryAsync(cmd);
                return i;
            });
        }

        public Task<int> UpdateInventory(Inventory inventory, InventoryHistory history = null)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("UpdateInventory");
                cmd.Parameters.AddWithValue("@Id", inventory.Id);
                cmd.Parameters.AddWithValue("@PurchaseRate", inventory.PurchaseRate);
                cmd.Parameters.AddWithValue("@RetailRate", inventory.RetailRate);
                cmd.Parameters.AddWithValue("@Quantity", inventory.Quantity);
                cmd.Parameters.AddWithValue("@FirstPurchaseDate", inventory.FirstPurchaseDate);
                cmd.Parameters.AddWithValue("@IsDeleted", inventory.IsDeleted);
                cmd.Parameters.AddWithValue("@BranchId", inventory.BranchId);
                cmd.Parameters.AddWithValue("@UserId", inventory.UserId);
                cmd.Parameters.AddWithValue("@Code", inventory.Code);
                cmd.Parameters.AddWithValue("@BarCode", inventory.BarCode);
                cmd.Parameters.AddWithValue("@Name", inventory.Name);
                cmd.Parameters.AddWithValue("@Size", inventory.Size);
                cmd.Parameters.AddWithValue("@Color", inventory.Color);
                cmd.Parameters.AddWithValue("@ColorName", inventory.ColorName);
                cmd.Parameters.AddWithValue("@BrandId", inventory.BrandId);
                cmd.Parameters.AddWithValue("@CategoryId", inventory.CategoryId);
                cmd.Parameters.AddWithValue("@MaintainHistory", history != null);

                int i = await DataAccess.ExecuteNonQueryAsync(cmd);
                return i;
            });
        }

        public Task<int> Restock(Inventory inventory, int salesReturn)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("RestockInventory");
                cmd.Parameters.AddWithValue("@Id", inventory.Id);
                cmd.Parameters.AddWithValue("@Quantity", salesReturn);

                int i = await DataAccess.ExecuteNonQueryAsync(cmd);
                return i;
            });
        }
    }
}
