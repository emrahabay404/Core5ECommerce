using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
   public class Product
   {

      public Product()
      {
         _Favourites = new HashSet<Favourite>();
      }

      public virtual ICollection<Favourite> _Favourites { get; set; }


      [Key]
      public int ProductId { get; set; }
      public string Name { get; set; }
      public double Price { get; set; }
      public string PhotoUrl { get; set; }
      public int Stock { get; set; }
      public string Description { get; set; }
      public int Discount { get; set; }
      public int BasketDiscount { get; set; }
      public int BuyCount { get; set; }


      public bool Status { get; set; }


      [ForeignKey("SubCategory")]
      public int SubCatID { get; set; }
      public SubCategory SubCategory { get; set; }


      [ForeignKey("Brand")]
      public int BrandID { get; set; }
      public Brand Brand { get; set; }


      [ForeignKey("AppUser")]
      public int Id { get; set; }
      public AppUser AppUser { get; set; }


      public List<Photo> Photo { get; set; }
      public List<Comment> Comments { get; set; }
      public List<OrderDetail> OrderDetails { get; set; }

   }
}