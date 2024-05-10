using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class SupportRequest
    {

        [Key]
        public int SupportID { get; set; }
        public string Object { get; set; }
        public string Description { get; set; }


        public DateTime SupportDate { get; set; }
        public bool Status { get; set; }

    
        public string Reply { get; set; }
        public DateTime ReplyDate { get; set; }



        [ForeignKey("AppUser")]
        public int Id { get; set; }
        public AppUser AppUser { get; set; }






    }
}
