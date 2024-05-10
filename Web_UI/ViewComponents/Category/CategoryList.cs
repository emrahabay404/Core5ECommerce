using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Web_UI.ViewComponents.Category
{
   public class CategoryList : ViewComponent
    {
      public  CategoryRepo categoryRepo = new();
        public IViewComponentResult Invoke()
        {
            return View(categoryRepo.TList());
        }
    }
}
