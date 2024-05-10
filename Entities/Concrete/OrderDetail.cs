using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{

   public class OrderDetail
    {

        [Key]
        public int OrderDetailId { get; set; }


        [ForeignKey("OrderId")]
        public int? OrderId { get; set; }
        public virtual Order Order { get; set; }
        //[ForeignKey("Order")]
        //public int OrderId { get; set; }
        //public Order Order { get; set; }



        public double Price { get; set; }
        public int Quantity { get; set; }


        public int OrderCargoNo { get; set; }


        public DateTime Deliverydate { get; set; }


        public int Status { get; set; }


        //[ForeignKey("ProductId")]
        //public int? ProductId { get; set; }
        //public virtual Product Product { get; set; }


        [ForeignKey("AddressID")]
        public int? AddressID { get; set; }
        public virtual Address Address { get; set; }



        [ForeignKey("OrderProcessID")]
        public int OrderProcessID { get; set; }
        public OrderProcess OrderProcess { get; set; }



        [ForeignKey("CargoID")]
        public int CargoID { get; set; }
        public Cargo Cargo { get; set; }



        public int? ProductId { get; set; }
        public virtual Product Product { get; set; }





    }
}
