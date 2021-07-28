using POS.Data;
using POS.Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace POS.BusinessRule
{
    public class BrandBO
    {
        public Task<List<Brand>> GetBrands()
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GetAllBrands");
                DataTable tbl = await DataAccess.ExecuteReaderCommandAsync(cmd);
                List<Brand> brands = new List<Brand>();
                foreach (DataRow row in tbl.Rows)
                {
                    brands.Add(new Brand
                    {
                        Id = (long)row["Id"],
                        Name = row["Name"].ToString()
                    });
                }
                return brands;
            });
        }

        public Task<Brand> GetBrand(long id)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GetBrandById");
                cmd.Parameters.AddWithValue("@Id", id);
                DataTable table = await DataAccess.ExecuteReaderCommandAsync(cmd);

                Brand c = new Brand
                {
                    Id = (long)table.Rows[0]["Id"],
                    Name = table.Rows[0]["Name"].ToString()
                };
                return c;
            });
        }

        public Task<long> Save(Brand brand)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("SaveBrand");
                cmd.Parameters.AddWithValue("@Name", brand.Name);
                long i = await DataAccess.ExecuteScalarCommandAsync<long>(cmd);
                return i;
            });
        }

        public Task<int> Update(Brand brand)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("UpdateBrand");
                cmd.Parameters.AddWithValue("@Id", brand.Id);
                cmd.Parameters.AddWithValue("@Name", brand.Name);
                int i = await DataAccess.ExecuteNonQueryAsync(cmd);
                return i;
            });
        }

        public Task<int> Delete(long id)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("RemoveBrand");
                cmd.Parameters.AddWithValue("@Id", id);
                int i = await DataAccess.ExecuteNonQueryAsync(cmd);
                return i;
            });
        }
    }
}
