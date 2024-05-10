using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Wallet
    {

        [Key]
        public int WalletID { get; set; }
        public double Balance { get; set; }




        [ForeignKey("AppUser")]
        public int Id { get; set; }
        public AppUser AppUser { get; set; }


        public bool Status { get; set; }


    }
}
