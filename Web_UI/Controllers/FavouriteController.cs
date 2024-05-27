using DataAccess.Concrete;
using DataAccess.Repositories;
using Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web_UI.Controllers
{
    public class FavouriteController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public FavouriteController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public Context C = new();
        public FavoriRepo favoriRepo = new();
        public bool Durum;

        [HttpGet]
        public IActionResult FavList()
        {
            int id = (int)_userManager.GetUserAsync(User).Result.Id;
            return View(favoriRepo.GetListWith(id));
        }

        public IActionResult FavDel(int id)
        {
            favoriRepo.TDelete(favoriRepo.TGet(id));
            return RedirectToAction("FavList", "Favourite");
        }

        public IActionResult Add(int prodid)
        {
            int id = (int)_userManager.GetUserAsync(User).Result.Id;
            //var cnt = C.Favourites.Where(x => x.ProductId == prodid && x.Id == id).Count();
            var cnt = favoriRepo.GetFavCount(prodid, id).Count();
            if (cnt < 1)
            {
                Favourite fv = new();
                fv.Id = (int)_userManager.GetUserAsync(User).Result.Id;
                fv.ProductId = (int)prodid;
                favoriRepo.TAdd(fv);
                Durum = true;
            }
            else
            {
                Durum = false;
            }
            return Json(Durum);
        }

    }
}