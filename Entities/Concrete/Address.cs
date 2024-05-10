using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
   public class Address
    {
        [Key]
        public int AddressID { get; set; }

        public string Details { get; set; }

        public string Header { get; set; }

        [ForeignKey("AppUser")]
        public int Id { get; set; }
        public AppUser AppUser { get; set; }



        [ForeignKey("City")]
        public int CityID { get; set; }
        public City City { get; set; }



        public List<OrderDetail> OrderDetails { get; set; }

    }
}
