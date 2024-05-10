using Microsoft.AspNetCore.Http;

namespace Entities.Concrete
{
   public class UrunEkle
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        //resim dosya formatı oldugu için gerekli tür
        public IFormFile Photo { get; set; }

        public int Stock { get; set; }
        public int SubCatID { get; set; }
        public int BrandID { get; set; }





    }
}
