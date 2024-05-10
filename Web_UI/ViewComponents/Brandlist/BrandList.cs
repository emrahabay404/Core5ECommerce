using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Web_UI.ViewComponents.Brandlist
{
   public class BrandList : ViewComponent
    {
        public BrandRepo brandRepo = new();
        public IViewComponentResult Invoke()
        {
            //bu komutumuz ile listeden rastgele 5 veriyi sıraladık.
            //return View(brandRepo.TList().OrderBy(x => Guid.NewGuid()).Take(5).ToList());

            return View(brandRepo.TList());
        }
    }
}
