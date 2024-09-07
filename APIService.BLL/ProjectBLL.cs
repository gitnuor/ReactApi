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
    public class ProjectBLL
    {
        ProjectDAL pdal = new ProjectDAL();
        public int Insert(ProjectBO model)
        {
            try
            {
                return pdal.InsertUser(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response GetAll()
        {
            return pdal.GetAll();
        }

        public DataTable Delete(int ID)
        {
            try
            {
                return pdal.DeleteUser(ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetbyId(int id)
        {
            return pdal.GetbyId(id);
        }
    }
}
