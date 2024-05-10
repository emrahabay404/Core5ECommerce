using DataAccess.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
   public class ProductRepo : GenericRepository<Product>
    {
        public Context C = new();

        public List<Product> GetWithStore(int id)
        {
            return C.Products.Where(x => x.Id == id).ToList();
        }

        public List<Product> GetWithBrand(int id)
        {
            return C.Products.Where(x => x.BrandID == id).ToList();
        }


        public List<Comment> GetPrdComments(int id)
        {
                       
            return C.Comments.OrderByDescending(x => x.CommDate).Include(x => x.AppUser).Where(x => x.ProductId == id).ToList();
        }



        public List<Photo> GetPrdImgs(int id)
        {
            return C.Photos.Where(x => x.ProductId == id).ToList();
        }


        public Brand GetBrandName(int id)
        {
            return C.Brands.Where(x => x.BrandID == id).FirstOrDefault();
        }



    }
}
