using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class RequestSeller
    {

        [Key]
        public int RequestID { get; set; }

        public DateTime RequestDate { get; set; }


        [ForeignKey("AppUser")]
        public int Id { get; set; }
        public AppUser AppUser { get; set; }


    }
}
