using APIService.DAL.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIService.BO;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace APIService.DAL
{
    public class UserDAL: DatabaseManager
    {
        public int InsertUser(UserBO model)
        {
            try
            {
                ArrayList arlParam = new ArrayList();
                arlParam.Add(new SqlParameter("@Id", model.user_id));
                arlParam.Add(new SqlParameter("@Name", model.name));
                arlParam.Add(new SqlParameter("@Email", model.email));
                arlParam.Add(new SqlParameter("@Status", model.status));
                arlParam.Add(new SqlParameter("@Gender", model.gender));
                DataTable dt = ExecuteStoredProcedureDataTable("User_Insert", arlParam);
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Response GetAll()
        {
            return ExecuteStoredProcedureListAll<UserBO>("User_GetAll");
        }

       
        public DataTable DeleteUser(int ID)
        {
            try
            {
                ArrayList arlParam = new ArrayList();
                arlParam.Add(new SqlParameter("@ID", ID));
                DataTable dt = ExecuteStoredProcedureDataTable("User_Delete", arlParam);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateUser(UserBO model)
        {
            try
            {
                ArrayList arlParam = new ArrayList();
                arlParam.Add(new SqlParameter("@ID", model.user_id));
                arlParam.Add(new SqlParameter("@Name", model.name));
                arlParam.Add(new SqlParameter("@Email", model.email));
                arlParam.Add(new SqlParameter("@Status", model.status));
                DataTable dt = ExecuteStoredProcedureDataTable("User_Update", arlParam);
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetbyId(int id)
        {
            ArrayList arlParam = new ArrayList();
            arlParam.Add(new SqlParameter("@ID", id));
            DataTable dt = ExecuteStoredProcedureDataTable("User_GetbyId", arlParam);
            return dt;
        }
    }
}
