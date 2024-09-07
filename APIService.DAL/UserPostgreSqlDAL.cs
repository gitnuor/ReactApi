using APIService.BO;
using APIService.DAL.Helper;
using Npgsql;
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
    public class UserPostgreSqlDAL:DataBasePostgraSql
    {

        public Response GetAllUser()
        {
            return ExecuteStoredProcedureListAllUser<UserBO>("public.user_getall");
        }

        public int InsertUser(UserBO model)
        {
            try
            {
                ArrayList arlParam = new ArrayList();
               // arlParam.Add(new NpgsqlParameter("@p_name", model.user_id));
                arlParam.Add(new NpgsqlParameter("@p_name", model.name));
                arlParam.Add(new NpgsqlParameter("@p_email", model.email));
                arlParam.Add(new NpgsqlParameter("@p_status", model.status));
                arlParam.Add(new NpgsqlParameter("@p_gender", model.gender));
                DataTable dt = ExecuteStoredProcedureDataTable("public.user_insert", arlParam);

                if (dt.Rows.Count > 0)
                {
                    return Convert.ToInt32(dt.Rows[0][0]); // Return the inserted user_id
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable DeleteUser(int ID)
        {
            try
            {
                string query = "DELETE FROM public.test_user WHERE user_id = @ID RETURNING *;";

                
                ArrayList arlParam = new ArrayList();
                arlParam.Add(new NpgsqlParameter("@ID", ID));

             
                DataTable dt = ExecuteStoredProcedureDataTableQuery(query, arlParam);
                return dt;

                //ArrayList arlParam = new ArrayList();
                //arlParam.Add(new SqlParameter("@ID", ID));
                //DataTable dt = ExecuteStoredProcedureDataTableDelete("public.User_Delete", arlParam);
                //return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetbyId(int id)
        {
            try
            {
                // Inline SQL query to select a user by ID
                string query = "SELECT * FROM public.test_user WHERE user_id = @ID;";

                // Create an ArrayList of parameters
                ArrayList arlParam = new ArrayList();
                arlParam.Add(new NpgsqlParameter("@ID", id));

                // Call method to execute the query
                DataTable dt = ExecuteStoredProcedureDataTableQuery(query, arlParam);
                return dt;
            }
            catch (Exception ex)
            {
                throw; // Re-throwing exception for further handling
            }
        }

        public int UpdateUser(UserBO model)
        {
            try
            {
                // Inline SQL query to update user details
                string query = "UPDATE public.test_user SET name = @Name, email = @Email, status = @Status, gender = @Gender WHERE user_id = @ID;";

                // Create an ArrayList of parameters
                ArrayList arlParam = new ArrayList();
                arlParam.Add(new NpgsqlParameter("@ID", model.user_id));
                arlParam.Add(new NpgsqlParameter("@Name", model.name));
                arlParam.Add(new NpgsqlParameter("@Email", model.email));
                arlParam.Add(new NpgsqlParameter("@Status", model.status));
                arlParam.Add(new NpgsqlParameter("@Gender", model.gender));

                // Execute the inline update query
                int result = ExecuteUpdateQuery(query, arlParam);
                return result;
            }
            catch (Exception ex)
            {
                throw; // Handle the exception as needed
            }
        }


    }
}
