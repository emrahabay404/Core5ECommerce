using DataAccess.Concrete;
using DataAccess.Repositories;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Web_UI.Controllers
{

   [Authorize(Roles = "admin,moderator")]

    public class ModeratorController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        public ModeratorController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }


        #region Tanimlamalar1
        public Context C = new();
        public BrandRepo brandRepo = new();
        public UserRepository userRepository = new();
        public SubCategoryRepo subCategoryRepo = new();
        public CategoryRepo categoryRepo = new();
        public OrdersRepo ordersRepo = new();
        public SupportRepo supportRepo = new();
        public WalletRepo walletRepo = new();
        public CargoRepo cargoRepo = new();
        public PhotoRepo photoRepo = new();
        public Cargo cargo = new();
        #endregion



        public IActionResult Index()
        {
            return View();
        }




        public IActionResult SellerRequest()
        {
            var list = C.RequestSellers.Include(x => x.AppUser).ToList();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> RequestReply(int id, int d)
        {
            Notification ntsf = new();
            if (d == 1)
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
                await _userManager.AddToRoleAsync(user, "seller");
                ntsf.NotifiTypeID = 4;
            }
            else
            {
                ntsf.NotifiTypeID = 11;
            }

            ntsf.SeenStatus = false;
            ntsf.NotifiDate = Convert.ToDateTime(DateTime.Now.ToLongTimeString());
            ntsf.Id = id;
            C.Notifications.Add(ntsf);
            C.SaveChanges();


            var complete = C.RequestSellers.Where(x => x.Id == id).FirstOrDefault();
            C.RequestSellers.Remove(complete);
            C.SaveChanges();



            return Redirect("/Moderator/SellerRequest/");
        }


        [HttpGet]
        public IActionResult GetRequestUser(int id)
        {
            return View(C.Users.Where(x => x.Id == id).FirstOrDefault());
        }




        [HttpGet]
        public IActionResult Brand()
        {
            return View(brandRepo.TList());
        }
        public IActionResult BrandDelete(int id)
        {
            var getdel = brandRepo.TGet(id);
            if (getdel.Status == true)
            {
                getdel.Status = false;
            }
            else
            {
                getdel.Status = true;
            }
            brandRepo.TUpdate(getdel);
            return RedirectToAction("Brand", "Moderator");
        }

        [HttpPost]
        public IActionResult Brandadd(Brand p)
        {
            p.Status = true;
            brandRepo.TAdd(p);
            return RedirectToAction("Brand", "Moderator");
        }

        [HttpGet]
        public IActionResult Brandedit(int id)
        {
            return View(brandRepo.TGet(id));
        }
        [HttpPost]
        public IActionResult Brandedit(Brand p)
        {
            var badd = brandRepo.TGet(p.BrandID);
            badd.BrandName = p.BrandName;
            badd.Status = p.Status;
            brandRepo.TUpdate(badd);
            return RedirectToAction("Brand", "Moderator");
        }



        [HttpGet]
        public IActionResult SubCatg()
        {
            List<SelectListItem> degerler = (from i in C.Categories.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.CategoryName,
                                                 Value = i.CategoryID.ToString()
                                             }).ToList();
            ViewBag.catg = degerler;

            return View(subCategoryRepo.TList());
        }
        public IActionResult SubCatgDelete(int id)
        {
            var getdel = subCategoryRepo.TGet(id);
            if (getdel.Status == true)
            {
                getdel.Status = false;
            }
            else
            {
                getdel.Status = true;
            }
            subCategoryRepo.TUpdate(getdel);
            return RedirectToAction("SubCatg", "Moderator");
        }
        [HttpPost]
        public IActionResult SubCatgadd(SubCategory p)
        {
            subCategoryRepo.TAdd(p);
            return RedirectToAction("SubCatg", "Moderator");
        }
        [HttpGet]
        public IActionResult SubCatedit(int id)
        {
            return View(subCategoryRepo.TGet(id));
        }
        [HttpPost]
        public IActionResult SubCatedit(SubCategory p)
        {
            var badd = subCategoryRepo.TGet(p.SubCatID);
            badd.SubCatName = p.SubCatName;
            badd.Status = p.Status;
            subCategoryRepo.TUpdate(badd);
            return RedirectToAction("SubCatg", "Moderator");
        }





        [HttpGet]
        public IActionResult Catg()
        {
            return View(categoryRepo.TList());
        }
        public IActionResult CatgDelete(int id)
        {
            var getdel = categoryRepo.TGet(id);
            if (getdel.Status == true)
            {
                getdel.Status = false;
            }
            else
            {
                getdel.Status = true;
            }
            categoryRepo.TUpdate(getdel);
            return RedirectToAction("Catg", "Moderator");
        }
        [HttpPost]
        public IActionResult Catgadd(Category p)
        {
            categoryRepo.TAdd(p);
            return RedirectToAction("Catg", "Moderator");
        }
        [HttpGet]
        public IActionResult Catedit(int id)
        {
            return View(categoryRepo.TGet(id));
        }
        [HttpPost]
        public IActionResult Catedit(Category p)
        {
            var badd = categoryRepo.TGet(p.CategoryID);
            badd.CategoryName = p.CategoryName;
            badd.Status = p.Status;
            categoryRepo.TUpdate(badd);
            return RedirectToAction("Catg", "Moderator");
        }




        [HttpGet]
        public IActionResult Cargo()
        {
            return View(cargoRepo.TList());
        }
        public IActionResult CargoDelete(int id)
        {
            var getdel = cargoRepo.TGet(id);
            if (getdel.Status == true)
            {
                getdel.Status = false;
            }
            else
            {
                getdel.Status = true;
            }
            cargoRepo.TUpdate(getdel);
            return RedirectToAction("Cargo", "Moderator");
        }
        [HttpPost]
        public IActionResult Cargoadd(CargoAddModel p)
        {
            if (p.CargoLogo != null)
            {
                var extansion = Path.GetExtension(p.CargoLogo.FileName);
                //benzersiz bir isim tanımlıyor guid. zorunlu değil direk dosya ismide verilebilir.
                var imagename = Guid.NewGuid() + extansion;
                var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/cargologo/", imagename);
                var stream = new FileStream(location, FileMode.Create);
                p.CargoLogo.CopyTo(stream);
                cargo.CargoLogo = "/images/cargologo/" + imagename;
            }
            cargo.CargoName = p.CargoName;
            //cargo.CargoLogo = p.CargoLogo.FileName;
            cargoRepo.TAdd(cargo);
            return RedirectToAction("Cargo", "Moderator");
        }
        [HttpGet]
        public IActionResult Cargoedit(int id)
        {
            return View(cargoRepo.TGet(id));
        }
        [HttpPost]
        public IActionResult Cargoedit(Cargo p)
        {
            var badd = cargoRepo.TGet(p.CargoID);
            badd.CargoName = p.CargoName;
            badd.Status = p.Status;
            cargoRepo.TUpdate(badd);
            return RedirectToAction("Cargo", "Moderator");
        }




        [HttpGet]
        public JsonResult Usersearch(string username)
        {
            var getus = JsonConvert.SerializeObject(C.Users.Where(x => x.UserName.Contains(username)).Take(5).ToList());
            return Json(getus);
        }




        [HttpGet]
        public IActionResult Wallet()
        {
            List<SelectListItem> values = (from i in C.Wallets.ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.Id.ToString(),
                                               Value = i.WalletID.ToString()
                                           }).ToList();
            ViewBag.walletlist = values;
            return View(walletRepo.GetListWith());
        }
        [HttpPost]
        public IActionResult Walletadd(Wallet p)
        {
            walletRepo.TAdd(p);
            return RedirectToAction("Wallet", "Moderator");
        }
        public IActionResult WalletDelete(int id)
        {
            var getdel = walletRepo.TGet(id);
            if (getdel.Status == true)
            {
                getdel.Status = false;
            }
            else
            {
                getdel.Status = true;
            }
            walletRepo.TUpdate(getdel);
            return RedirectToAction("Wallet", "Moderator");
        }

        [HttpGet]
        public IActionResult Walletedit(int id)
        {
            var ımgurl = walletRepo.TGet(id);
            TempData["userimg"] = C.Wallets.Where(x => x.WalletID == id).Select(x => x.AppUser.ImageUrl).FirstOrDefault();
            TempData["userfullname"] = C.Wallets.Where(x => x.WalletID == id).Select(x => x.AppUser.NameSurname).FirstOrDefault();

            return View(walletRepo.TGet(id));
        }
        [HttpPost]
        public IActionResult Walletedit(Wallet p)
        {
            var badd = walletRepo.TGet(p.WalletID);
            badd.Id = p.Id;
            badd.Balance = p.Balance;
            badd.Status = p.Status;
            walletRepo.TUpdate(badd);
            return RedirectToAction("Wallet", "Moderator");
        }






        [HttpGet]
        public IActionResult Photos()
        {
            return View(photoRepo.GetListWith());
        }
        public IActionResult PhotosDelete(int id)
        {
            var getdel = photoRepo.TGet(id);
            if (getdel.Status == true)
            {
                getdel.Status = false;
            }
            else
            {
                getdel.Status = true;
            }
            photoRepo.TUpdate(getdel);
            return RedirectToAction("Photos", "Moderator");
        }
        public IActionResult PhotosRemove(int id)
        {
            try
            {
                var blogval = photoRepo.TGet(id);
                photoRepo.TDelete(blogval);
                //
                var imgsrc = C.Photos.Where(x => x.PhotoID == id).Select(y => y.PhotoLink).FirstOrDefault();
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), ("wwwroot" + imgsrc));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                return RedirectToAction("Photos", "Moderator");
            }
            catch (Exception)
            {
                throw;
            }
        }





        [HttpGet]
        public IActionResult SupportRequests()
        {
            return View(supportRepo.GetListWith());
        }
        [AllowAnonymous]
        [Authorize(Roles = "seller,user")]
        public IActionResult Supportadd(SupportRequest sprequest)
        {
            supportRepo.TAdd(sprequest);
            return View();
        }
        public IActionResult SupportClose(int id)
        {
            var getdel = supportRepo.TGet(id);
            if (getdel.Status == true)
            {
                getdel.Status = false;
            }
            else
            {
                getdel.Status = true;
            }
            supportRepo.TUpdate(getdel);
            return RedirectToAction("SupportRequests", "Moderator");
        }

        public IActionResult SupportRemove(int id)
        {
            supportRepo.TDelete(supportRepo.TGet(id));
            return RedirectToAction("SupportRequests", "Moderator");
        }




    }
}
