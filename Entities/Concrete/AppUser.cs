using Microsoft.AspNetCore.Identity;

namespace Entities.Concrete
{
   public class AppUser : IdentityUser<int>
   {

      public AppUser()
      {
         _Favourites = new HashSet<Favourite>();
      }
      public virtual ICollection<Favourite> _Favourites { get; set; }

      public string NameSurname { get; set; }
      public string? ImageUrl { get; set; }
      public string? Job { get; set; }
      public string? About { get; set; }
      public string? StoreAddress { get; set; }
      public int? StoreTaxNo { get; set; }
      public string? StoreTaxOffice { get; set; }
      public bool? Status { get; set; }
      public List<Wallet> Wallet { get; set; }
      public List<Order> Order { get; set; }
      public List<Product> Products { get; set; }
      public List<Comment> Comments { get; set; }
      public List<Address> Address { get; set; }
      public List<SupportRequest> SupportRequest { get; set; }
      public List<Basket> Baskets { get; set; }

      public List<RequestSeller> RequestSellers { get; set; }
      public List<UserBankAcco> UserBankAccos { get; set; }
      public List<Notification> Notifications { get; set; }

   }
}
