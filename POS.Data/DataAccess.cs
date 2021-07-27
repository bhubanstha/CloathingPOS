using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class DataAccess
    {
        static string _connectionString;

        static DataAccess()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["CloathingPOS"].ConnectionString;
        }

        /// <summary>
        /// Create a <see cref="SqlCommand"/> for a given store procedure
        /// </summary>
        /// <param name="storeProcedureName">Name of store procedure that needs to be executed</param>
        /// <returns><see cref="SqlCommand"/> that needs to be executed</returns>
        public static SqlCommand CreateCommand(string storeProcedureName)
        {
            SqlConnection con = new SqlConnection(_connectionString);
            SqlCommand cmd = con.CreateCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storeProcedureName;

            return cmd;
        }

        /// <summary>
        /// Return a first selection result from a <see cref="SqlCommand"/> executed in given connection context
        /// </summary>
        /// <param name="cmd">A <see cref="SqlCommand"/> to be executed</param>
        /// <returns><see cref="DataTable"/>containing result of first select query</returns>

        public static DataTable ExecuteReaderCommand(SqlCommand cmd)
        {
            DataTable tbl = new DataTable();
            SqlDataReader rdr = null;
            try
            {
                cmd.Connection.Open();
                rdr = cmd.ExecuteReader();
                tbl.Load(rdr);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (rdr != null && !rdr.IsClosed)
                {
                    rdr.Close();
                }
                cmd.Connection.Close();
            }
            return tbl;
        }

        /// <summary>
        /// Return a first selection result from a <see cref="SqlCommand"/> executed in given connection context in async mode
        /// </summary>
        /// <param name="cmd">A <see cref="SqlCommand"/> to be executed</param>
        /// <returns><see cref="DataTable"/>containing result of first select query</returns>
        public static Task<DataTable> ExecuteReaderCommandAsync(SqlCommand cmd)
        {
            return Task.Run(async () =>
            {
                DataTable tbl = new DataTable();
                SqlDataReader rdr = null;
                try
                {
                    await cmd.Connection.OpenAsync();
                    rdr = await cmd.ExecuteReaderAsync();
                    tbl.Load(rdr);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (rdr != null && !rdr.IsClosed)
                    {
                        rdr.Close();
                    }
                    cmd.Connection.Close();
                }
                return tbl;
            });
        }


        /// <summary>
        /// Returns a number of rows affected by <see cref="SqlCommand"/> when executed
        /// </summary>
        /// <param name="cmd"><see cref="SqlCommand"/> to be executed</param>
        /// <returns><see cref="int"/> number of rows affected by <see cref="SqlCommand"/></returns>
        public static int ExecuteNonQuery(SqlCommand cmd)
        {
            int i = -1;
            try
            {
                cmd.Connection.Open();
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return i;
        }

        /// <summary>
        /// Returns a number of rows affected by <see cref="SqlCommand"/> when executed in async mode
        /// </summary>
        /// <param name="cmd"><see cref="SqlCommand"/> to be executed</param>
        /// <returns><see cref="int"/> number of rows affected by <see cref="SqlCommand"/></returns>
        public static Task<int> ExecuteNonQueryAsync(SqlCommand cmd)
        {
            return Task.Run(async () =>
            {
                int i = -1;
                try
                {
                    await cmd.Connection.OpenAsync();
                    i = await cmd.ExecuteNonQueryAsync();
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    cmd.Connection.Close();
                }
                return i;
            });
        }


        /// <summary>
        /// Execute a <see cref="SqlCommand"/> containing more than 1 select queries and all queries result set is required
        /// </summary>
        /// <param name="cmd"><see cref="SqlCommand"/> to be executed</param>
        /// <returns><see cref="DataSet"/> containing at least one <see cref="DataTable"/> having values return by select query in <see cref="SqlCommand"/></returns>
        public static DataSet ExecuteDataSet(SqlCommand cmd)
        {
            DataSet ds = null;            
            try
            {
                cmd.Connection.Open();
                SqlDataAdapter adap  = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adap.Fill(ds);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return ds;
        }


        /// <summary>
        /// Execute a <see cref="SqlCommand"/> containing more than 1 select queries and all queries result set is required
        /// </summary>
        /// <param name="cmd"><see cref="SqlCommand"/> to be executed</param>
        /// <returns><see cref="DataSet"/> containing at least one <see cref="DataTable"/> having values return by select query in <see cref="SqlCommand"/></returns>
        public static Task<DataSet> ExecuteDataSetAsync(SqlCommand cmd)
        {
            return Task.Run(async () =>
            {

                DataSet ds = null;
                try
                {
                    await cmd.Connection.OpenAsync();
                    SqlDataAdapter adap = new SqlDataAdapter(cmd);
                    ds = new DataSet();
                    adap.Fill(ds);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    cmd.Connection.Close();
                }
                return ds;
            });
        }


        /// <summary>
        /// Execute a scalar command to get specific value from a <see cref="SqlCommand"/>
        /// </summary>
        /// <typeparam name="T">Return data type of a data to be returned. Should be a <see cref="struct"/></typeparam>
        /// <param name="cmd"></param>
        /// <returns>A value from a <see cref="SqlCommand"/> query of type <see cref="Type"/> T</returns>
        public static T ExecuteScalarCommand<T>(SqlCommand cmd) where T : struct
        {
            T data;
            try
            {
                cmd.Connection.Open();

                data = (T)Convert.ChangeType(cmd.ExecuteScalar(), typeof(T));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return data;
        }


        /// <summary>
        /// Execute a scalar command to get specific value from a <see cref="SqlCommand"/>
        /// </summary>
        /// <typeparam name="T">Return data type of a data to be returned. Should be a <see cref="struct"/></typeparam>
        /// <param name="cmd"></param>
        /// <returns>A value from a <see cref="SqlCommand"/> query of type <see cref="Type"/> T</returns>
        public static Task<T> ExecuteScalarCommandAsync<T>(SqlCommand cmd) where T: struct
        {
            return Task.Run(async () =>
            {
                T data;
                try
                {
                    await cmd.Connection.OpenAsync();
                    data = (T)Convert.ChangeType(cmd.ExecuteScalar(), typeof(T));
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    cmd.Connection.Close();
                }
                return data;
            });
        }


        /// <summary>
        /// Execute a scalar command to get specific value from a <see cref="SqlCommand"/>
        /// </summary>
        /// <param name="cmd"><see cref="SqlCommand"/> to be executed</param>
        /// <returns>A string value from a <see cref="SqlCommand"/> query</returns>
        public static Task<string> ExecuteScalarCommandAsync(SqlCommand cmd) 
        {
            return Task.Run(async () =>
            {
                string data;
                try
                {
                    await cmd.Connection.OpenAsync();
                    data = cmd.ExecuteScalar().ToString();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    cmd.Connection.Close();
                }
                return data;
            });
        }

        ~DataAccess()
        {
            GC.Collect();
        }
    }
}
