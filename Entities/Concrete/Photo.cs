using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Photo
    {
        [Key]
        public int PhotoID { get; set; }
        public string PhotoLink { get; set; }
        public string PhotoName { get; set; }



        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product  Product { get; set; }


        public bool Status { get; set; }

    }
}