using DataAccess.Concrete;
using Entities.Concrete;

namespace DataAccess.ViewModels
{
   public class ProductModel
    {
        public Context C = new();

       public List<Product> Products { get; set; }

        public List<Product> FindAll()
        {
            return  C.Products.ToList();
        }

        public Product Find(int id)
        {
            List<Product> _products = FindAll();
            var prod = _products.Where(a => a.ProductId == id).FirstOrDefault();
            return prod;
        }



    }
}
