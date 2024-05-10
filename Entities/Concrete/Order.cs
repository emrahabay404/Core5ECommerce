using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        public DateTime OrderDate { get; set; }

       

        public double Total { get; set; }


         



        [ForeignKey("AppUser")]
        public int Id { get; set; }
        public AppUser AppUser { get; set; }




        public List<OrderDetail> OrderDetails { get; set; }


    }
}
