using APIService.BO;
using APIService.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIService.BLL
{
    public class UserBLL
    {
        UserDAL userdal = new UserDAL();
        UserPostgreSqlDAL userdalpostgresql = new UserPostgreSqlDAL();
        public int Insert(UserBO model)
        {
            try
            {
                //return userdal.InsertUser(model);
                return userdalpostgresql.InsertUser(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response GetAll()
        {
            return userdal.GetAll();
        }

        public Response GetAllUser()
        {
            return userdalpostgresql.GetAllUser();
        }


        public DataTable Delete(int ID)
        {
            try
            {
                return userdalpostgresql.DeleteUser(Convert.ToInt32(ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(UserBO model)
        {
            try
            {
                //return userdal.UpdateUser(model);
                return userdalpostgresql.UpdateUser(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetbyId(int id)
        {
            return userdalpostgresql.GetbyId(id);
        }
    }
}
