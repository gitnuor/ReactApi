using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Web;
using System.Reflection;
using System.Collections;
using System.IO;
using System.ComponentModel;
using APIService.BO;
using Npgsql;
using System.Threading.Tasks;

namespace APIService.DAL.Helper
{
    public class DatabaseManager : IDisposable
    {

        private String conString;
        private SqlConnection _cn;
        private DataTable _dt;
        private DataSet _ds;
        private SqlCommand _cmd;
        private SqlDataReader _reader;

        private String conStringPostgreSql;


        #region Utilities
        public DatabaseManager()
        {
            TeracottaConnectionstring();
            
            //conString = "Data Source=HOFS2406101562A;Initial Catalog=RR;User ID=sa;Password=biTS@1234; Max Pool Size=100;Connect Timeout=200;Application Name=eOperations;";
        }
        public DatabaseManager(string strCon)
        {
            //conString = ConfigurationManager.ConnectionStrings[strCon].ConnectionString;
        }

        protected string TeracottaConnectionstring()
        {
            //Terakota.TerakotaReviled oTerakotaReviled = new Terakota.TerakotaReviled();
            //oTerakotaReviled.SetConfigPath(HttpContext.Current.Server.MapPath("~\\Terakota.config"));
            //TerakotaApp.Entities.ConnectionString oConnection = oTerakotaReviled.GetConnectionString("dbCon");
            //conString = oConnection.ConnectionDetails.Replace("\"", "").Trim();
            conString = "Data Source=10.42.65.115; Initial Catalog=APIService; User Id=sa; Password=biTS1234; Connect Timeout=200;Application Name=weTogether;";
          //  conStringPostgreSql = "User Id=postgres;Password=123456;Server=4.194.89.164;Port=5432;Database=postgres";
            //oTerakotaReviled = null;
            return conString;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_cn != null)
                {
                    _cn.Dispose();
                    _cn = null;
                }
                if (_dt != null)
                {
                    _dt.Dispose();
                    _dt = null;
                }
                if (_cmd != null)
                {
                    _cmd.Dispose();
                    _cmd = null;
                }
                if (_reader != null)
                {
                    _reader.Dispose();
                    _reader = null;
                }
            }
        }

        ~DatabaseManager()
        {
            Dispose(false);
        }

        private static readonly Lazy<DatabaseManager> VLazy = new Lazy<DatabaseManager>(() => new DatabaseManager());

        public static DatabaseManager Instance
        {
            get { return VLazy.Value; }
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
        #endregion

        //¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦Execute Procedure¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦

        public DataTable ExecuteDataTableSQL(string strSql)
        {
            try
            {
                using (_cn = new SqlConnection(conString))
                {
                    _cn.Open();
                    using (_cmd = new SqlCommand(strSql, _cn))
                    {
                        _cmd.CommandType = CommandType.Text;
                        using (_reader = _cmd.ExecuteReader())
                        {
                            _dt = new DataTable();
                            try
                            {
                                _dt.Load(_reader);
                            }
                            catch { }
                        }
                    }
                }

                return _dt;
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }
            finally
            {
                ClearParameters();
            }
        }

        public object ExecuteStoredProcedureScalar(string strSql, ArrayList arlParams)
        {
            try
            {
                object objResult = null;
                using (_cn = new SqlConnection(conString))
                {
                    _cn.Open();
                    using (_cmd = new SqlCommand(strSql, _cn))
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        for (int i = 0; i < arlParams.Count; i++)
                        {
                            _cmd.Parameters.Add(arlParams[i]);
                        }

                        objResult = _cmd.ExecuteScalar();
                    }
                }
                return objResult;
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));

            }
            finally
            {
                ClearParameters();
            }
        }

        public int ExecuteNonQueryStoredProcedure(string strSql, ArrayList arlParams)
        {
            try
            {
                int intResult;
                using (_cn = new SqlConnection(conString))
                {
                    _cn.Open();
                    using (_cmd = new SqlCommand(strSql, _cn))
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        for (int i = 0; i < arlParams.Count; i++)
                        {
                            _cmd.Parameters.Add(arlParams[i]);
                        }

                        intResult = _cmd.ExecuteNonQuery();
                    }
                }
                return intResult;
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));

            }
            finally
            {
                ClearParameters();
            }
        }

        public DataTable ExecuteStoredProcedureDataTable(string strSql, ArrayList arlParams)
        {
            try
            {
                using (_cn = new SqlConnection(conString))
                {
                    _cn.Open();
                    using (_cmd = new SqlCommand(strSql, _cn))
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        //foreach(object obj in arlParams)
                        //{
                        //    SqlParameter prm = (SqlParameter)obj;
                        //    prm.Value=DbType.
                        //}
                        for (int i = 0; i < arlParams.Count; i++)
                        {
                            _cmd.Parameters.Add(arlParams[i]);
                        }

                        using (_reader = _cmd.ExecuteReader())
                        {
                            _dt = new DataTable();
                            try
                            {
                                _dt.Load(_reader);
                            }
                            catch { }
                        }
                    }
                }

                return _dt;
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }
            finally
            {
                ClearParameters();
            }
        }

        public DataSet ExecuteStoredProcedureDataSet(string strSql, ArrayList arlParams)
        {
            try
            {
                using (_cn = new SqlConnection(conString))
                {
                    _cn.Open();
                    using (_cmd = new SqlCommand(strSql, _cn))
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        for (int i = 0; i < arlParams.Count; i++)
                        {
                            _cmd.Parameters.Add(arlParams[i]);
                        }

                        using (_reader = _cmd.ExecuteReader())
                        {
                            _dt = new DataTable();
                            _ds = new DataSet();
                            try
                            {
                                _dt.Load(_reader);
                                _ds.Tables.Add(_dt);
                            }
                            catch { }
                        }
                    }
                }

                return _ds;
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }
            finally
            {
                ClearParameters();
            }
        }

        public List<T> ExecuteStoredProcedureList<T>(string strSql, ArrayList arlParams)
        {
            List<T> list = new List<T>();
            try
            {
                using (_cn = new SqlConnection(conString))
                {
                    _cn.Open();
                    using (_cmd = new SqlCommand(strSql, _cn))
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        for (int i = 0; i < arlParams.Count; i++)
                        {
                            _cmd.Parameters.Add(arlParams[i]);
                        }

                        using (_reader = _cmd.ExecuteReader())
                        {
                            try
                            {
                                list = DataReaderMapToList<T>(_reader);
                            }
                            catch { }
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }
            finally
            {
                ClearParameters();
            }
        }

        public Response ExecuteStoredProcedureListAll<T>(string storeProcedoreName)
        {
            Response response = new Response();
            List<T> list = new List<T>();
            try
            {
                using (_cn = new SqlConnection(conString))
                {
                    _cn.Open();
                    using (_cmd = new SqlCommand(storeProcedoreName, _cn))
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;

                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Parameters.Clear();
                        using (_reader = _cmd.ExecuteReader())
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
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      

        //public T ExecuteStoredProcedureObject<T>(string strSql, ArrayList arlParams)
        //{
        //    T obj = default(T);
        //    try
        //    {
        //        using (_cn = new SqlConnection(conString))
        //        {
        //            _cn.Open();
        //            using (_cmd = new SqlCommand(strSql, _cn))
        //            {
        //                _cmd.CommandType = CommandType.StoredProcedure;
        //                for (int i = 0; i < arlParams.Count; i++)
        //                {
        //                    _cmd.Parameters.Add(arlParams[i]);
        //                }

        //                using (_reader = _cmd.ExecuteReader())
        //                {
        //                    try
        //                    {
        //                        obj = DataReaderMapToObject<T>(_reader);
        //                    }
        //                    catch { }
        //                }
        //            }
        //        }

        //        return obj;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        ClearParameters();
        //    }
        //}

        //¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦Mapping list or entity ¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦¦

        #region mapping

        public static T DataReaderMapToObject<T>(IDataReader dr)
        {
            T obj = default(T);
            while (dr.Read())
            {
                try
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
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

            return obj;
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

        #endregion

        #region Dynamic Object

        public Response ExecuteNonQueryStoredProcedure(string storeProcedoreName, object parameters)
        {
            Response response = new Response();
            try
            {
                int intResult;
                using (_cn = new SqlConnection(conString))
                {
                    _cn.Open();
                    using (_cmd = new SqlCommand(storeProcedoreName, _cn))
                    {
                        List<DbParameter> param = this.CreateDbParmeters(parameters);
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Parameters.Clear();
                        _cmd.Parameters.AddRange(param.ToArray());
                        intResult = _cmd.ExecuteNonQuery();
                        response.Success = true;
                        response.Code = StaticContext.Code.Success;
                        response.MessageType = StaticContext.MessageType.Success;
                        response.Message = StaticContext.Message.Success;
                        response.Data = intResult;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public Response ExecuteScalarStoredProcedure(string storeProcedoreName, object parameters)
        {
            Response response = new Response();
            try
            {
                using (_cn = new SqlConnection(conString))
                {
                    _cn.Open();
                    using (_cmd = new SqlCommand(storeProcedoreName, _cn))
                    {
                        List<DbParameter> param = this.CreateDbParmeters(parameters);
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Parameters.Clear();
                        _cmd.Parameters.AddRange(param.ToArray());
                        response.Success = true;
                        response.Code = StaticContext.Code.Success;
                        response.MessageType = StaticContext.MessageType.Success;
                        response.Message = StaticContext.Message.Success;
                        response.Data = _cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Code = StaticContext.Code.Error;
                response.MessageType = StaticContext.MessageType.Error;
                response.Message = ex.Message;
                response.Data = null;
            }
            return response;
        }
        public Response ExecuteStoredProcedureList<T>(string storeProcedoreName, object parameters)
        {
            Response response = new Response();
            List<T> list = new List<T>();
            try
            {
                using (_cn = new SqlConnection(conString))
                {
                    _cn.Open();
                    using (_cmd = new SqlCommand(storeProcedoreName, _cn))
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        List<DbParameter> param = this.CreateDbParmeters(parameters);
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Parameters.Clear();
                        _cmd.Parameters.AddRange(param.ToArray());
                        using (_reader = _cmd.ExecuteReader())
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
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Response ExecuteStoredProcedureObject<T>(string storeProcedoreName, object parameters)
        {
            Response response = new Response();
            T obj = default(T);
            try
            {
                using (_cn = new SqlConnection(conString))
                {
                    _cn.Open();
                    using (_cmd = new SqlCommand(storeProcedoreName, _cn))
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        List<DbParameter> param = this.CreateDbParmeters(parameters);
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Parameters.Clear();
                        _cmd.Parameters.AddRange(param.ToArray());
                        using (_reader = _cmd.ExecuteReader())
                        {
                            obj = DataReaderMapToObject<T>(_reader);
                        }
                        response.Success = true;
                        response.Code = StaticContext.Code.Success;
                        response.MessageType = StaticContext.MessageType.Success;
                        response.Message = StaticContext.Message.Success;
                        response.Data = obj;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response ExecuteStoredProcedureDataTable(string storeProcedoreName, object parameters)
        {
            Response response = new Response();
            try
            {
                using (_cn = new SqlConnection(conString))
                {
                    _cn.Open();
                    using (_cmd = new SqlCommand(storeProcedoreName, _cn))
                    {
                        List<DbParameter> param = this.CreateDbParmeters(parameters);
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Parameters.Clear();
                        _cmd.Parameters.AddRange(param.ToArray());

                        using (_reader = _cmd.ExecuteReader())
                        {
                            _dt = new DataTable();
                            _dt.Load(_reader);
                            response.Success = true;
                            response.Code = StaticContext.Code.Success;
                            response.MessageType = StaticContext.MessageType.Success;
                            response.Message = StaticContext.Message.Success;
                            response.Data = _dt;
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }
        }
        protected List<DbParameter> CreateDbParmeters<TTarget>(TTarget target)
        {
            try
            {
                List<DbParameter> parameters = new List<DbParameter>();
                List<PropertyInfo> props = new List<PropertyInfo>(GetSourceProperties(typeof(TTarget)));
                if (props.Count == 0)
                    props = new List<PropertyInfo>(GetSourceProperties(target.GetType()));

                foreach (PropertyInfo p in props)
                {
                    string columnName = p.Name;
                    object value = p.GetValue(target, null);

                    if (columnName == "OutMessageParam")
                    {
                        parameters.Add(CreateParameter(columnName, value, ParameterDirection.InputOutput, 5000));
                    }
                    else
                        parameters.Add(CreateParameter(columnName, value));
                }
                return parameters;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Type = '{0}', Method = '{1}', Message={2}", this.GetType().FullName, "CreateDbParmetersFromEntity()", ex.Message));
            }
        }
        public static PropertyInfo[] GetSourceProperties(Type sourceType)
        {
            List<PropertyInfo> result = new List<PropertyInfo>();
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(sourceType);
            foreach (PropertyDescriptor item in props)
                if (item.IsBrowsable)
                    result.Add(sourceType.GetProperty(item.Name));
            return result.ToArray();
        }
        protected SqlParameter CreateParameter(string parameterName, object parameterValue)
        {
            SqlParameter parameter = new SqlParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            return parameter;
        }
        protected SqlParameter CreateParameter(string parameterName, object parameterValue, ParameterDirection direction, int size)
        {
            SqlParameter parameter = new SqlParameter();
            parameter.Direction = direction;
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            parameter.Size = size;
            return parameter;
        }
        #endregion
    }
}
