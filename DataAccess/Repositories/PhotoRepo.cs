using DataAccess.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
namespace DataAccess.Repositories
{
   public class PhotoRepo : GenericRepository<Photo>
    {
        public Context C = new();
        public List<Photo> GetListWith()
        {
            return C.Photos.Include(x => x.Product).ToList();
        }

 


    }
}
