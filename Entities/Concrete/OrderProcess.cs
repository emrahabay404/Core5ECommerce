using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class OrderProcess
    {
        [Key]
        public int OrderProcessID { get; set; }
        public string OrderProcessType { get; set; }
        public string OrderProcessImg { get; set; }



        public List<OrderDetail> OrderDetails { get; set; }


    }
}
