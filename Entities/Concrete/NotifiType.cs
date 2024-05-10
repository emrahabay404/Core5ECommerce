using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class NotifiType
    {
        [Key]
        public int NotifiTypeID { get; set; }
        public string NotifiTypeName { get; set; }
        public string NotifiTypeImg { get; set; }

                public List<Notification> Notifications { get; set; }


    }
}
