using DataAccess.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
   public class FavoriRepo : GenericRepository<Favourite>
   {

      public Context C = new();
      public List<Favourite> GetListWith(int id)
      {
         return C.Favourites.Where(x => x.Id == id).Include(y => y.Product).ToList();
      }

      public List<Favourite> GetFavCount(int prdid, int usid)
      {
         return C.Favourites.Where(x => x.ProductId == prdid && x.Id == usid).ToList();
      }

   }
}
