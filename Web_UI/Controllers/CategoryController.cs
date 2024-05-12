using DataAccess.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace Web_UI.Controllers
{
    public class CategoryController : Controller
    {

        public Context C = new();
        [AllowAnonymous]
        public JsonResult SubCatList(int id)
        {
            var values = JsonConvert.SerializeObject(C.SubCategories.Where(x => x.CategoryID == id).ToList());
            return Json(values);
        }

        [AllowAnonymous]
        public IActionResult SubCatListID(int id)
        {
            TempData["Subcatname"] = C.SubCategories.Where(x => x.SubCatID == id).Select(y => y.SubCatName).FirstOrDefault();
            var values = (C.Products.Where(x => x.SubCatID == id).ToList());
            return View(values);
        }



        //denemeeeeeeeeee



    }
}
