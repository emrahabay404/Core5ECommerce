using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Passwordchange
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "mevcut şifre")]
        public string Currentpassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "yeni şifre")]
        public string Newpassword { get; set; }

      
        [DataType(DataType.Password)]
        [Display(Name = "yeni şifre onay")]
        [Compare("Newpassword", ErrorMessage ="yeni şifrelerin eşleşmedi.İşlem başarısız")]
        public string Confirmpassword { get; set; }


    }
}