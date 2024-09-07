using APIService.BO;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace APIService.DAL.Helper
{
    public class DataBasePostgraSql : IDisposable
    {
        private String conStringPostgreSql;
        private NpgsqlConnection _cn;
        private NpgsqlCommand _cmd;
        private NpgsqlDataReader _reader;
        private DataTable _dt;
        public DataBasePostgraSql()
        {
            TeracottaConnectionstring();
        }

        protected string TeracottaConnectionstring()
        {
    
            conStringPostgreSql = "User Id=postgres;Password=123456;Server=4.194.89.164;Port=5432;Database=postgres";
            return conStringPostgreSql;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        //public async Task<tran_tech_pack_entity> GetAsync(long Id)
        //{
        //    try
        //    {
        //        using (NpgsqlConnection connection = new NpgsqlConnection(_configuration.GetConnectionString("PGConnectionString")))
        //        {
        //            connection.Open();

        //            string query = @"SELECT m.*  FROM tran_tech_pack m   where m.tran_techpack_id=@Id";

        //            var dataList = connection.Query<tran_tech_pack_entity>(query,
        //                new { @Id = Id }).ToList().FirstOrDefault();

        //            return dataList;//JsonConvert.DeserializeObject<List<tran_tech_pack_entity>>(JsonConvert.SerializeObject(dataList));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //}

        public Response ExecuteStoredProcedureListAllUser<T>(string functionName)
        {
            Response response = new Response();
            List<T> list = new List<T>();

            try
            {
                using (var _cn = new NpgsqlConnection(conStringPostgreSql))
                {
                    _cn.Open();
                    using (var _cmd = new NpgsqlCommand($"SELECT * FROM {functionName}()", _cn))
                    {
                       // _cmd.CommandType = CommandType.StoredProcedure;

                        using (var _reader = _cmd.ExecuteReader())
                        {
                            list = DataReaderMapToList<T>(_reader);
                        }

                        response.Success = true;
                        response.Code = StaticContext.Code.Success;
                        response.MessageType = StaticContext.MessageType.Success;
                        response.Message = StaticContext.Message.Success;
                        response.Data = list;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception properly, for example:
                response.Success = false;
                response.Code = StaticContext.Code.Error;
                response.MessageType = StaticContext.MessageType.Error;
                response.Message = ex.Message;
                response.Data = null; // Or some error details
                                      // Log the exception (optional)
            }

            return response;
        }

       

        public DataTable ExecuteStoredProcedureDataTable(string strSql, ArrayList arlParams)
        {
            try
            {
                using (_cn = new NpgsqlConnection(conStringPostgreSql))
                {
                    _cn.Open();
                    var query = $"SELECT * FROM {strSql}(@p_name, @p_email, @p_status, @p_gender)";
                    // Use CommandType.Text for PostgreSQL functions
                    using (_cmd = new NpgsqlCommand(query, _cn))
                    {
                        _cmd.CommandType = CommandType.Text;

                        // Add parameters
                        foreach (NpgsqlParameter param in arlParams)
                        {
                            _cmd.Parameters.Add(param);
                        }

                        using (_reader = _cmd.ExecuteReader())
                        {
                            _dt = new DataTable();
                            _dt.Load(_reader); // Load the result into a DataTable
                        }
                    }
                }

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                ClearParameters();
            }
        }

        public DataTable ExecuteStoredProcedureDataTableQuery(string storedProcedureName, ArrayList parameters)
        {
            DataTable dt = new DataTable();

            try
            {
                using (var _cn = new NpgsqlConnection(conStringPostgreSql))
                {
                    _cn.Open();
                    using (var _cmd = new NpgsqlCommand(storedProcedureName, _cn))
                    {
                        _cmd.CommandType = CommandType.Text;

                        // Add parameters to the command
                        foreach (NpgsqlParameter param in parameters)
                        {
                            _cmd.Parameters.AddWithValue(param.ParameterName, param.Value);
                        }

                        using (var adapter = new NpgsqlDataAdapter(_cmd))
                        {
                            adapter.Fill(dt); // Fill DataTable with result
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex; // Handle the exception as needed
            }

            return dt;
        }

        public int ExecuteUpdateQuery(string query, ArrayList parameters)
        {
            int rowsAffected = 0;

            try
            {
                using (var _cn = new NpgsqlConnection(conStringPostgreSql))
                {
                    _cn.Open();
                    using (var _cmd = new NpgsqlCommand(query, _cn))
                    {
                        _cmd.CommandType = CommandType.Text; // Set the command type to text for an inline SQL query

                        // Add parameters to the command
                        foreach (NpgsqlParameter param in parameters)
                        {
                            _cmd.Parameters.AddWithValue(param.ParameterName, param.Value);
                        }

                        // Execute the query and get the number of affected rows
                        rowsAffected = _cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw; // Handle the exception as needed
            }

            return rowsAffected;
        }



        public int ClearParameters()
        {

            if (_cmd != null)
            {
                if (_cmd.Parameters.Count > 0)
                    _cmd.Parameters.Clear();
            }
            return 1;
        }

        public static List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                int fieldCount = dr.FieldCount;
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (ColumnExists(dr, prop.Name))
                    {
                        if (!object.Equals(dr[prop.Name], DBNull.Value))
                        {
                            prop.SetValue(obj, dr[prop.Name], null);
                        }
                    }
                    else
                    {
                        prop.SetValue(obj, null, null);
                    }
                }
                list.Add(obj);
            }

            return list;
        }

        public static bool ColumnExists(IDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

    }
}
