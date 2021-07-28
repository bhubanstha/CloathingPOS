using POS.Data;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.BusinessRule
{
    public class InventoryHistoryBO
    {

        public Task<List<InventoryHistory>> GetHistory(long id)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GetInventoryHistory");
                cmd.Parameters.AddWithValue("@ItemId", id);
                DataTable tbl = await DataAccess.ExecuteReaderCommandAsync(cmd);
                List<InventoryHistory> inventoryHistories = new List<InventoryHistory>();
                foreach (DataRow row in tbl.Rows)
                {
                    inventoryHistories.Add(new InventoryHistory
                    {
                        Id = (long)row["Id"],
                        Quantity = (int)row["Quantity"],
                        PurchaseRate = (decimal)row["PurchaseRate"],
                        RetailRate = (decimal)row["RetailRate"],
                        PurchaseDate = (DateTime)row["PurchaseDate"],
                        InventoryId = (long)row["InventoryId"]
                    });
                }
                return inventoryHistories;
            });
        }
    }
}
