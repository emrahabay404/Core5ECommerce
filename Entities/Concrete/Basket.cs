using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Basket
    {

        [Key]
        public int BasketID { get; set; }

        public int Quantity { get; set; }





        [ForeignKey("AppUser")]
        public int Id { get; set; }
        public AppUser AppUser { get; set; }



        //[ForeignKey("Product")]
        //public int ProductId { get; set; }
        //public Product Product { get; set; }




    }
}
