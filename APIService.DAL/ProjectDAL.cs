using APIService.BO;
using APIService.DAL.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIService.DAL
{
   public class ProjectDAL: DatabaseManager
    {
        public int InsertUser(ProjectBO model)
        {
            try
            {
                ArrayList arlParam = new ArrayList();
                arlParam.Add(new SqlParameter("@Id", model.ID));
                arlParam.Add(new SqlParameter("@PName", model.PName));
                arlParam.Add(new SqlParameter("@POwner", model.POwner));
                arlParam.Add(new SqlParameter("@Desc", model.Desc));
                arlParam.Add(new SqlParameter("@Type", model.PType));
                arlParam.Add(new SqlParameter("@Status", model.Status));
                DataTable dt = ExecuteStoredProcedureDataTable("Project_Insert", arlParam);
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Response GetAll()
        {
            return ExecuteStoredProcedureListAll<ProjectBO>("Project_GetAll");
        }

        public DataTable DeleteUser(int ID)
        {
            try
            {
                ArrayList arlParam = new ArrayList();
                arlParam.Add(new SqlParameter("@ID", ID));
                DataTable dt = ExecuteStoredProcedureDataTable("Project_Delete", arlParam);
                return dt;
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
            DataTable dt = ExecuteStoredProcedureDataTable("Project_GetbyId", arlParam);
            return dt;
        }
    }
}
