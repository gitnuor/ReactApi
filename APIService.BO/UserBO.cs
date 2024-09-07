using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIService.BO
{
    public class UserBO
    {
        public Int64 user_id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string email { get; set; }

        public bool status { get; set; }
        [Required]
        public string gender { get; set; }
    }
}
