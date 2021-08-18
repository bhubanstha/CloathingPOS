using POS.Data;
using POS.Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace POS.BusinessRule
{
    public class CustomerBO
    {
        public Task<List<Customer>> GetAll()
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GetAllCustomer");
                DataTable tbl = await DataAccess.ExecuteReaderCommandAsync(cmd);
                List<Customer> customers = new List<Customer>();
                foreach (DataRow row in tbl.Rows)
                {
                    customers.Add(new Customer
                    {
                        Id = (long)row["Id"],
                        Name = row["Name"].ToString(),
                        Address = row["Address"].ToString(),
                        GoogleMap = row["GoogleMap"].ToString(),
                        Mobile1 = row["Mobile1"].ToString(),
                        Mobile2 = row["Mobile2"].ToString()
                    });
                }
                return customers;
            });
        }

        public Task<Customer> GetCustomerByID(long id)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("GetCustomerById");
                cmd.Parameters.AddWithValue("@Id", id);
                DataTable tbl = await DataAccess.ExecuteReaderCommandAsync(cmd);
                if (tbl.Rows.Count > 0)
                {
                    Customer cust = new Customer
                    {
                        Id = (long)tbl.Rows[0]["Id"],
                        Name = tbl.Rows[0]["Name"].ToString(),
                        Address = tbl.Rows[0]["Address"].ToString(),
                        GoogleMap = tbl.Rows[0]["GoogleMap"].ToString(),
                        Mobile1 = tbl.Rows[0]["Mobile1"].ToString(),
                        Mobile2 = tbl.Rows[0]["Mobile2"].ToString()
                    };
                    return cust;
                }
                return null;
            });
        }


        public Task<List<Customer>> SearchCustomer(string searchText)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("SearchCustomer");
                cmd.Parameters.AddWithValue("@SearchText", searchText+"%");
                DataTable tbl = await DataAccess.ExecuteReaderCommandAsync(cmd);
                List<Customer> customers = new List<Customer>();
                foreach (DataRow row in tbl.Rows)
                {
                    customers.Add(new Customer
                    {
                        Id = (long)row["Id"],
                        Name = row["Name"].ToString(),
                        Address = row["Address"].ToString(),
                        GoogleMap = row["GoogleMap"].ToString(),
                        Mobile1 = row["Mobile1"].ToString(),
                        Mobile2 = row["Mobile2"].ToString()
                    });
                }
                return customers;
            });
        }

        public Task<long> Save(Customer customer)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("SaveCustomer");
                cmd.Parameters.AddWithValue("@Name", customer.Name);
                cmd.Parameters.AddWithValue("@Address", customer.Address);
                cmd.Parameters.AddWithValue("@GoogleMap", customer.GoogleMap);
                cmd.Parameters.AddWithValue("@Mobile1", customer.Mobile1);
                cmd.Parameters.AddWithValue("@Mobile2", customer.Mobile2);
                long i = await DataAccess.ExecuteScalarCommandAsync<long>(cmd);
                return i;
            });
        }

        public Task<int> Update(Customer customer)
        {
            return Task.Run(async () =>
            {
                SqlCommand cmd = DataAccess.CreateCommand("UpdateCustomer");
                cmd.Parameters.AddWithValue("@Id", customer.Id);
                cmd.Parameters.AddWithValue("@Name", customer.Name);
                cmd.Parameters.AddWithValue("@Address", customer.Address);
                cmd.Parameters.AddWithValue("@GoogleMap", customer.GoogleMap);
                cmd.Parameters.AddWithValue("@Mobile1", customer.Mobile1);
                cmd.Parameters.AddWithValue("@Mobile2", customer.Mobile2);
                int i = await DataAccess.ExecuteNonQueryAsync(cmd);
                return i;
            });
        }
    }
}
