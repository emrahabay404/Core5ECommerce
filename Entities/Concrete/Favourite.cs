using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
   public class Favourite
   {

      [Key]
      public int FavID { get; set; }
      public int? ProductId { get; set; }
      public int? Id { get; set; }
      public virtual Product Product { get; set; }
      public virtual AppUser AppUser { get; set; }

      //[ForeignKey("ProductId")]
      //public virtual Product Product { get; set; }
      //public int? ProductId { get; set; }
      //[ForeignKey("AppUser")]
      //public int Id { get; set; }
      //public AppUser AppUser { get; set; }
   }
}