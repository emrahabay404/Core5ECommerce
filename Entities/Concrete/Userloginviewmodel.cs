using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
   public class Userloginviewmodel
    {
        [Required(ErrorMessage = "Lütfen kullanıcı girin")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Lütfen şifre girin")]
        public string Password { get; set; }
    }
}
