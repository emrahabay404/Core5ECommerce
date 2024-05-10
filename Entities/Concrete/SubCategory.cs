using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class SubCategory
    {
        [Key]
        public int SubCatID { get; set; }
        public string SubCatName { get; set; }

        public bool Status { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public Category Category{ get; set; }

        //public Product  Product { get; set; }
        public List<Product> Product { get; set; }
        

    }
}
