using POS.Data;
using POS.Model;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace POS.BusinessRule.ADO
{
    public class ShopBO 
    {
        public Task<Shop> GetShop()
        {
            return Task.Run(async () =>
            {

                SqlCommand cmd = DataAccess.CreateCommand("GetAllSales");
                DataTable tbl = await DataAccess.ExecuteReaderCommandAsync(cmd);
                Shop shop = new Shop
                {
                    Id = (long)tbl.Rows[0]["Id"],
                    Name = tbl.Rows[0]["Name"].ToString(),
                    PANNumber = tbl.Rows[0]["PANNumber"].ToString(),
                    LogoPath = tbl.Rows[0]["LogoPath"].ToString(),
                    CalculateVATOnSales = (bool)tbl.Rows[0]["CalculateVATOnSales"],
                    PrintInvoice = (bool)tbl.Rows[0]["PrintInvoice"],
                    PdfPassword = tbl.Rows[0]["PdfPassword"].ToString()
                };
                return shop;
            });
        }

        public Task<int> SaveShop(Shop shop)
        {
            return Task.Run(async () =>
            {

                SqlCommand cmd = DataAccess.CreateCommand("GetAllSales");
                cmd.Parameters.AddWithValue("@Name", shop.Name);
                cmd.Parameters.AddWithValue("@PANNumber", shop.PANNumber);
                cmd.Parameters.AddWithValue("@LogoPath", shop.LogoPath);
                cmd.Parameters.AddWithValue("@CalculateVATOnSales", shop.CalculateVATOnSales);
                cmd.Parameters.AddWithValue("@PrintInvoice", shop.PrintInvoice);
                cmd.Parameters.AddWithValue("@PdfPassword", shop.PdfPassword);
                int i = await DataAccess.ExecuteNonQueryAsync(cmd);
                return i;
            });
        }

        public Task<int> UpdateShop(Shop shop)
        {
            return Task.Run(async () =>
            {

                SqlCommand cmd = DataAccess.CreateCommand("GetAllSales");
                cmd.Parameters.AddWithValue("@Id", shop.Id);
                cmd.Parameters.AddWithValue("@Name", shop.Name);
                cmd.Parameters.AddWithValue("@PANNumber", shop.PANNumber);
                cmd.Parameters.AddWithValue("@LogoPath", shop.LogoPath);
                cmd.Parameters.AddWithValue("@CalculateVATOnSales", shop.CalculateVATOnSales);
                cmd.Parameters.AddWithValue("@PrintInvoice", shop.PrintInvoice);
                cmd.Parameters.AddWithValue("@PdfPassword", shop.PdfPassword);
                int i = await DataAccess.ExecuteNonQueryAsync(cmd);
                return i;
            });
        }
    }
}
