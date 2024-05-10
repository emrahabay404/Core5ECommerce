using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Cargo
    {
        [Key]
        public int CargoID { get; set; }
        public string CargoName { get; set; }
        public string CargoLogo { get; set; }

        public bool Status { get; set; }


 
        public List<OrderDetail> OrderDetails{ get; set; }



    }
}
