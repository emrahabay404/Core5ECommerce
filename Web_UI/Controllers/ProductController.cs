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

    public class ProductController : Controller
    {
        public Context C = new();
        public PhotoRepo photoRepo = new();
        public ProductRepo productRepo = new();
        public Product Product = new();
        public Photo photo = new();
        public OrderDetailRepo orderDetailRepo = new();

        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public ProductController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public JsonResult AllProductList()
        {
            System.Threading.Thread.Sleep(1200);
            var list = JsonConvert.SerializeObject(productRepo.TList());
            return Json(list);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {

            //return View(productRepo.TList());
            return View(productRepo.TList().OrderBy(x => Guid.NewGuid()).ToList());
        }


        [HttpGet]
        public IActionResult Prdlistpricedown()
        {
            return View(productRepo.TList().OrderBy(x => x.Price).ToList());
        }

        [HttpGet]
        public IActionResult Prdlistpriceup()
        {
            return View(productRepo.TList().OrderByDescending(x => x.Price).ToList());
        }

        [HttpGet]
        public IActionResult Prdlistbuycount()
        {
            return View(productRepo.TList().OrderByDescending(x => x.BuyCount).ToList());
        }
        [HttpGet]
        public IActionResult PrdlistAtoZ()
        {
            return View(productRepo.TList().OrderBy(x => x.Name).ToList());
        }
        [HttpGet]
        public IActionResult PrdlistZtoA()
        {
            return View(productRepo.TList().OrderByDescending(x => x.Name).ToList());
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Detay(int id)
        {
            TempData["prdidforcomment"] = id;

            //var comments = C.Comments.OrderByDescending(x=>x.CommDate).Include(x => x.AppUser).Where(x => x.ProductId == id).ToList();
            var comments = productRepo.GetPrdComments(id);
            ViewBag.comments = comments.ToList();
            ViewBag.commentcount = comments.Count();

            //var prdimg = C.Photos.Where(x => x.ProductId == id).ToList();
            var prdimg = productRepo.GetPrdImgs(id);
            ViewBag.Prdimages = prdimg.ToList();

            var x = productRepo.TGet(id);
            int storeid = x.Id;
            TempData["Store"] = C.Users.Where(x => x.Id == storeid).Select(x => x.NameSurname).FirstOrDefault();

            Product p = new()
            {
                //FoodID = x.FoodID,
                ProductId = x.ProductId,
                Name = x.Name,
                Price = x.Price,
                Stock = x.Stock,
                Description = x.Description,
                PhotoUrl = x.PhotoUrl,
                BrandID = x.BrandID,
                Id = x.Id,
                Status = x.Status,
                Discount = x.Discount,
                BasketDiscount = x.BasketDiscount,
                BuyCount = x.BuyCount
            };
            //var srg = C.Brands.Where(x => x.BrandID == p.BrandID).FirstOrDefault();
            var srg = productRepo.GetBrandName(p.BrandID);
            TempData["Brand"] = srg.BrandName;
            return View(p);
        }

        [HttpGet]
        public IActionResult Addcomment(string msg, int productid)
        {
            Comment cmt = new();
            cmt.CommContent = msg;
            cmt.Id = (int)_userManager.GetUserAsync(User).Result.Id;
            cmt.CommDate = Convert.ToDateTime(DateTime.Now.ToLongTimeString());
            cmt.Status = true;
            cmt.ProductId = productid;
            C.Comments.Add(cmt);
            C.SaveChanges();
            return Json(true);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var getdel = productRepo.TGet(id);
            if (getdel.Status == true)
            {
                getdel.Status = false;
            }
            else
            {
                getdel.Status = true;
            }
            productRepo.TUpdate(getdel);
            //var val = productRepo.TGet(id);
            //productRepo.TDelete(val);
            return Redirect("/Product/List/");
        }


        [HttpGet]
        public IActionResult Addpro()
        {
            List<SelectListItem> degerler = (from i in C.SubCategories.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.SubCatName,
                                                 Value = i.SubCatID.ToString()
                                             }).ToList();
            List<SelectListItem> degerler2 = (from i in C.Brands.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.BrandName,
                                                  Value = i.BrandID.ToString()
                                              }).ToList();

            List<SelectListItem> degerler3 = (from i in C.Categories.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.CategoryName,
                                                  Value = i.CategoryID.ToString()
                                              }).ToList();


            ViewBag.subcat = degerler;
            ViewBag.brand = degerler2;
            ViewBag.category = degerler3;
            return View();
        }

        [HttpPost]
        public void Addpro(UrunEkle p)
        {
            //if (p.Photo != null)
            //{
            var extansion = Path.GetExtension(p.Photo.FileName);
            var imagename = Guid.NewGuid() + extansion;
            var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/productimg/", imagename);
            var stream = new FileStream(location, FileMode.Create);
            p.Photo.CopyTo(stream);
            Product.PhotoUrl = "/images/productimg/" + imagename;
            //}
            Product.Name = p.Name;
            Product.Price = p.Price;
            Product.Stock = p.Stock;
            Product.Description = p.Description;
            Product.BrandID = p.BrandID;
            Product.SubCatID = p.SubCatID;
            Product.Id = (int)_userManager.GetUserAsync(User).Result.Id;
            productRepo.TAdd(Product);


            //BURASI İSE ÜRÜN EKLEMEDE İLK RESMİ EKLER.
            //BURAYI KAPAK FOTOĞRAFI GİBİ KULLANACAGIMDAN DOLAYI İPTAL ETTİM.
            //photo.PhotoLink = Product.PhotoUrl;
            //photo.PhotoName = imagename;
            //photo.Status = true;
            //photo.ProductId = C.Products.OrderByDescending(x => x.ProductId).Take(1).FirstOrDefault().ProductId;
            //photoRepo.TAdd(photo);
            //return RedirectToAction("List", "Product");
        }

        [HttpGet]
        public IActionResult ProductEdit(int id)
        {
            List<SelectListItem> degerler = (from i in C.SubCategories.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.SubCatName,
                                                 Value = i.SubCatID.ToString()
                                             }).ToList();
            List<SelectListItem> degerler2 = (from i in C.Brands.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.BrandName,
                                                  Value = i.BrandID.ToString()
                                              }).ToList();
            ViewBag.subcat = degerler;
            ViewBag.brand = degerler2;
            var values = productRepo.TGet(id);
            return View(values);
        }

        [HttpPost]
        public IActionResult ProductEdit(Product p)
        {
            //int Userid = (int)_userManager.GetUserAsync(User).Result.Id;
            var dsd = C.Products.Where(x => x.ProductId == p.ProductId).FirstOrDefault();
            var prd = productRepo.TGet(p.ProductId);
            prd.Name = p.Name;
            //prd.Id = Userid;
            prd.Price = p.Price;
            prd.Description = p.Description;
            prd.Stock = p.Stock;
            prd.BrandID = p.BrandID;
            prd.SubCatID = p.SubCatID;
            prd.Discount = p.Discount;
            prd.BasketDiscount = p.BasketDiscount;
            productRepo.TUpdate(prd);

            return Redirect("/Product/List/");
        }

        [HttpPost]
        public IActionResult Coverimgadd(IFormFile photo, int productid)
        {
            var extansion = Path.GetExtension(photo.FileName);
            var imagename = Guid.NewGuid() + extansion;
            var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/productimg/", imagename);
            var stream = new FileStream(location, FileMode.Create);
            photo.CopyTo(stream);

            var getprd = productRepo.TGet(productid);
            getprd.PhotoUrl = "/images/productimg/" + imagename;
            productRepo.TUpdate(getprd);

            return Redirect("/Product/List/");
        }

        [HttpGet]
        public IActionResult Coverimgdel(int id)
        {
            try
            {
                var imgsrc = C.Products.Where(x => x.ProductId == id).Select(y => y.PhotoUrl).FirstOrDefault();
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), ("wwwroot" + imgsrc));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                var getprd = productRepo.TGet(id);
                getprd.PhotoUrl = "/images/productimg/nl.jpg";
                productRepo.TUpdate(getprd);
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [HttpGet]
        public IActionResult ProductPhotoEdit(int id)
        {
            var prd = productRepo.TGet(id);
            ViewBag.Prdname = prd.Name;
            TempData["ProductId"] = prd.ProductId;
            var values = C.Photos.Where(x => x.ProductId == id);
            return View(values);
        }

        [HttpPost]
        public IActionResult PhotoAdd(IFormFile photo, int productid)
        {
            var extansion = Path.GetExtension(photo.FileName);
            var imagename = Guid.NewGuid() + extansion;
            var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/productimg/", imagename);
            var stream = new FileStream(location, FileMode.Create);
            photo.CopyTo(stream);

            Photo pt = new();
            pt.PhotoName = "yok";
            pt.PhotoLink = "/images/productimg/" + imagename;
            pt.ProductId = productid;
            pt.Status = true;
            photoRepo.TAdd(pt);

            return Redirect("/Product/List");
        }

        [HttpGet]
        public void PhotoRemove(int id)
        {

            var imgsrc = C.Photos.Where(x => x.PhotoID == id).Select(y => y.PhotoLink).FirstOrDefault();
            //var fullPath = Path.Combine(Directory.GetCurrentDirectory(), ("wwwroot/images/productimg/" + imgsrc));
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), ("wwwroot" + imgsrc));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            var pht = photoRepo.TGet(id);
            photoRepo.TDelete(pht);
            //return Redirect("/Product/List/");
        }

        //BURASI SATICI ÜRÜN İŞLEMLERİ!!
        [Authorize(Roles = "admin,seller")]
        [HttpGet]
        public IActionResult List()
        {
            if (Globals.LoginStatus == 0)
            {
                return Redirect("/Account/Login/");
            }


            int Userid = (int)_userManager.GetUserAsync(User).Result.Id;

            return View(productRepo.GetWithStore(Userid));
        }

        //ALLTAKİ 2 KISIM LAYOUTTA KULLANILIYOR.
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Aramasonuc(string urunad)
        {
            var list = C.Products.Include(x => x.AppUser).Where(y => y.Name.Contains(urunad));
            ViewBag.adet = list.Count();
            ViewBag.filter = urunad;
            return View(list);
        }
        [HttpGet]
        [AllowAnonymous]
        public JsonResult Aramafilter(string urunad)
        {
            var list = C.Products.Where(x => x.Name.Contains(urunad)).ToList().Take(10).ToList();
            return Json(JsonConvert.SerializeObject(list));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult StoreProductist(int id)
        {
            TempData["Storename"] = C.Users.Where(x => x.Id == id).Select(y => y.NameSurname).FirstOrDefault();
            return View(C.Products.Where(x => x.Id == id).ToList());

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult BrandProductList(int id)
        {
            var list = productRepo.GetWithBrand(id);
            TempData["Brandname"] = C.Brands.Where(x => x.BrandID == id).Select(y => y.BrandName).FirstOrDefault();
            return View(list);
        }

        [Authorize(Roles = "admin,seller")]
        [HttpGet]
        public IActionResult StoreOrder()
        {
            int Userid = _userManager.GetUserAsync(User).Result.Id;
            //var order = C.OrderDetails.Where(x => x.Product.Id == Userid).Include(x => x.Product).Include(x => x.Address).Include(x => x.Cargo).Include(x => x.OrderProcess).ToList();
            var order = orderDetailRepo.StoreOrderList(Userid);
            return View(order);
        }

        [HttpGet]
        [Authorize(Roles = "admin,seller")]
        public IActionResult StoreOrderDetail(int id)
        {
            List<SelectListItem> prclist = (from i in C.OrderProcesses.ToList()
                                            select new SelectListItem
                                            {
                                                Text = i.OrderProcessType,
                                                Value = i.OrderProcessID.ToString()
                                            }).ToList();
            List<SelectListItem> cargolist = (from i in C.Cargos.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.CargoName,
                                                  Value = i.CargoID.ToString()
                                              }).ToList();

            TempData["process"] = prclist;
            TempData["cargos"] = cargolist;


            var order = orderDetailRepo.GetListWith(id).FirstOrDefault();
            return View(order);
        }

        [HttpPost]
        [Authorize(Roles = "admin,seller")]

        public void StoreOrderDetail(OrderDetail detail)
        {
            var values = orderDetailRepo.TGet(detail.OrderDetailId);
            values.CargoID = detail.CargoID;
            values.CargoID = detail.CargoID;
            values.OrderCargoNo = detail.OrderCargoNo;
            values.OrderProcessID = detail.OrderProcessID;
            orderDetailRepo.TUpdate(values);
            //return Redirect("/Product/StoreOrder/");
        }

        [HttpPost]
        [Authorize(Roles = "admin,seller")]
        public IActionResult SetOrderProcess(int orderid, int procid)
        {
            try
            {
                var getdel = orderDetailRepo.TGet(orderid);
                getdel.OrderProcessID = procid;
                orderDetailRepo.TUpdate(getdel);
                return Json(true);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}