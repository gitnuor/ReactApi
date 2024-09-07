using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIService.BO
{
    public class ProjectBO
    {
        public int ID { get; set; }
        public string PName { get; set; }
        public string POwner { get; set; }
        public string Desc { get; set; }
        public string PType { get; set; }
        public bool Status { get; set; }
    }
}
