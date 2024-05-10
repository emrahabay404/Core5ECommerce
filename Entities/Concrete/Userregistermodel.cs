using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
   public class Userregistermodel
   {
      [Display(Name = "Ad soyad")]
      [Required(ErrorMessage = "Lütfen ad soyad")]
      public string NameSurname { get; set; }

      [Display(Name = "Şifre")]
      [Required(ErrorMessage = "Lütfen şifre")]
      public string Password { get; set; }

      [Display(Name = "Şifre Onay")]
      [Compare("Password", ErrorMessage = "Şifre tekrar")]
      public string ConfirmPassword { get; set; }

      [Display(Name = "Mail")]
      [Required(ErrorMessage = "Lütfen ad soyad")]
      public string Mail { get; set; }

      [Display(Name = "Kullanıcı")]
      [Required(ErrorMessage = "Lütfen girin")]
      public string Username { get; set; }

      public IFormFile ImgUrl { get; set; }

   }
}
