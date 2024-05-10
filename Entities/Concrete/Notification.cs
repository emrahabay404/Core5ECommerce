using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Notification
    {


        [Key]
        public int NotifiID { get; set; }

        public DateTime NotifiDate { get; set; }


        public bool SeenStatus { get; set; }



        [ForeignKey("AppUser")]
        public int Id { get; set; }
        public AppUser AppUser { get; set; }



        [ForeignKey("NotifiType")]
        public int NotifiTypeID { get; set; }
        public NotifiType NotifiType{ get; set; }






    }
}
