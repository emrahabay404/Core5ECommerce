using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Brand
    {
        [Key]
        public int BrandID { get; set; }
        public string BrandName { get; set; }
        public bool Status { get; set; }

        public string Brandlogo { get; set; }
     

        public List<Product> Product { get; set; }

    }
}
