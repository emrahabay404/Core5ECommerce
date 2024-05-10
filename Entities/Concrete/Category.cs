using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        public bool Status { get; set; }
        public string IconImg{ get; set; }

        public List<SubCategory> SubCategory { get; set; }



    }
}
