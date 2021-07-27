using POS.Data;
using POS.Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace POS.BusinessRule.ADO
{
    public class CategoryBO
    {
        public Task<List<Category>> GetCategories()
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GetAllCategory");
                DataTable tbl = await DataAccess.ExecuteReaderCommandAsync(cmd);
                List<Category> categories = new List<Category>();
                foreach (DataRow row in tbl.Rows)
                {
                    categories.Add(new Category
                    {
                        Id = (long)row["Id"],
                        Name = row["Name"].ToString()
                    });
                }
                return categories;
            });
        }

        public Task<Category> GetCategory(long id)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GetCategoryById");
                cmd.Parameters.AddWithValue("@Id", id);
                DataTable table = await DataAccess.ExecuteReaderCommandAsync(cmd);

                Category c = new Category
                {
                    Id = (long)table.Rows[0]["Id"],
                    Name = table.Rows[0]["Name"].ToString()
                };
                return c;
            });
        }

        public Task<long> Save(Category category)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("SaveCategory");
                cmd.Parameters.AddWithValue("@Name", category.Name);
                long i = await DataAccess.ExecuteScalarCommandAsync<long>(cmd);
                return i;
            });
        }

        public Task<int> Update(Category category)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("UpdateCategory");
                cmd.Parameters.AddWithValue("@Id", category.Id);
                cmd.Parameters.AddWithValue("@Name", category.Name);
                int i = await DataAccess.ExecuteNonQueryAsync(cmd);
                return i;
            });
        }

        public Task<int> Delete(long id)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("RemoveCategory");
                cmd.Parameters.AddWithValue("@Id", id);
                int i = await DataAccess.ExecuteNonQueryAsync(cmd);
                return i;
            });
        }
    }
}
