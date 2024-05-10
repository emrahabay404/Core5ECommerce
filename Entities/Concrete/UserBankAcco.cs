using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class UserBankAcco
    {
        [Key]
        public int AccoID { get; set; }

        [MaxLength(40)]
        public string Bankname { get; set; }
        [MaxLength(70)]
        public string AccountNo { get; set; }
        [MaxLength(80)]
        public string IBANNo { get; set; }
        public bool Status{ get; set; }



        [ForeignKey("AppUser")]
        public int Id { get; set; }
        public AppUser AppUser { get; set; }



    }
}
