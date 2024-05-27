using Business.Helpers;
using DataAccess.Concrete;
using DataAccess.Repositories;
using DataAccess.ViewModels;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Web_UI.Controllers
{
    [AllowAnonymous]
    public class CartController : Controller
    {

        public AddressRepo addressRepo = new();
        public WalletRepo walletRepo = new();
        public ProductRepo productRepo = new();

        private readonly UserManager<AppUser> _userManager;
        public CartController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public bool Durum;
        public Context C = new();

        public static int CartProductCount = 0;

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                TempData["CartCount"] = (int)CartProductCount;
                var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                ViewBag.cart = cart;

                ViewBag.total = cart.Sum(item => (item.Product.Price - (item.Product.Price * (item.Product.Discount + item.Product.BasketDiscount) / 100)) * item.Adet);
                ViewBag.total = Math.Round(ViewBag.total, 2);

                if (Globals.LoginStatus != 0)
                {

                    int userid = (int)_userManager.GetUserAsync(User).Result.Id;
                    var getwalletuser = C.Wallets.Where(x => x.Id == userid).Select(x => x.Balance).FirstOrDefault();
                    ViewBag.getwallet = getwalletuser;

                    List<SelectListItem> getaddressuser = (from i in C.Addresses.Where(x => x.Id == userid)
                                                           select new SelectListItem
                                                           {
                                                               Text = i.Header + " : (" + i.Details + ")",
                                                               Value = i.AddressID.ToString()
                                                           }).ToList();
                    ViewBag.getaddress = getaddressuser;
                }

                return View();
            }
            catch (Exception)
            {
                return View();
            }
        }

        public JsonResult CartList()
        {
            TempData["CartCount"] = (int)CartProductCount;
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            ViewBag.total = cart.Sum(item => item.Product.Price * item.Adet);
            ViewBag.total = Math.Round(ViewBag.total, 2);
            return Json(JsonConvert.SerializeObject(ViewBag.cart));
        }

        private int IsExist(int id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.ProductId.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

        [HttpPost]
        public IActionResult DetailsAdd(int id, int proadet)
        {
            ProductModel product = new();
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                List<Item> cart = new();
                cart.Add(new Item { Product = product.Find(id), Adet = proadet });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                int index = IsExist(id);
                if (index != -1)
                {
                    cart[index].Adet++;
                }
                else
                {
                    cart.Add(new Item { Product = product.Find(id), Adet = proadet });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            var say = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = say;
            CartProductCount = say.Sum(x => x.Adet);

            Durum = true;
            return Json(Durum);
        }

        [HttpPost]
        public IActionResult Ekle(int id)
        {
            ProductModel product = new();
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                List<Item> cart = new();
                cart.Add(new Item { Product = product.Find(id), Adet = 1 });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                int index = IsExist(id);
                if (index != -1)
                {
                    cart[index].Adet++;
                }
                else
                {
                    cart.Add(new Item { Product = product.Find(id), Adet = 1 });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }

            var say = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = say;
            CartProductCount = say.Sum(x => x.Adet);
            Durum = true;
            return Json(Durum);
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = IsExist(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            var say = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = say;
            if (say.Sum(x => x.Adet) == 0 || (say.Sum(x => x.Adet) == 0))
            {
                CartProductCount = 0;
            }
            CartProductCount = say.Sum(x => x.Adet);
            Durum = true;
            return Json(Durum);
        }

        [HttpPost]
        public IActionResult QuantityRemove(int id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = IsExist(id);
            if (index != -1 && index != 0)
            {
                cart[id].Adet = cart[id].Adet - 1;
            }
            else
            {
                cart.RemoveAt(id);
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            var say = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = say;
            CartProductCount = say.Sum(x => x.Adet);
            Durum = true;
            return Json(Durum);
        }

        public PartialViewResult CartCount()
        {
            TempData["CartCount"] = (int)CartProductCount;
            return PartialView(TempData["CartCount"]);
        }

        public IActionResult Pay(int adresid)
        {
            try
            {
                if (adresid < 1 && Globals.LoginStatus == 1)
                {
                    return Json("address");
                }
                if (Globals.LoginStatus == 0)
                {
                    return Json("userid");
                }
                if (adresid != 0)
                {
                    int userid = (int)_userManager.GetUserAsync(User).Result.Id;
                    var getwallet = C.Wallets.Where(x => x.Id == userid).FirstOrDefault();

                    var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                    ViewBag.cart = cart;
                    ViewBag.total = cart.Sum(item => item.Product.Price * item.Adet);
                    ViewBag.total = Math.Round(ViewBag.total, 2);
                    Order orde = new();
                    ViewBag.total = cart.Sum(item => (item.Product.Price - (item.Product.Price * (item.Product.Discount + item.Product.BasketDiscount) / 100)) * item.Adet);
                    orde.Total = ViewBag.total;
                    orde.OrderDate = Convert.ToDateTime(DateTime.Now.ToLongDateString());
                    orde.Id = (int)_userManager.GetUserAsync(User).Result.Id;
                    C.Orders.Add(orde);
                    C.SaveChanges();
                    var lastindex = C.Orders.OrderByDescending(x => x.OrderID).Select(y => y.OrderID).First();
                    foreach (var item in ViewBag.cart)
                    {
                        OrderDetail ordetay = new();
                        ordetay.OrderId = (int)lastindex;
                        //ordetay.Price = (double)item.Product.Price;
                        ordetay.Price = item.Product.Price - (((double)item.Product.Price * (item.Product.Discount + item.Product.BasketDiscount)) / 100);
                        ordetay.Quantity = (int)item.Adet;
                        ordetay.ProductId = (int)item.Product.ProductId;
                        ordetay.Status = 1;
                        ordetay.CargoID = 1;
                        ordetay.AddressID = adresid;
                        ordetay.OrderProcessID = 1;
                        ordetay.Deliverydate = Convert.ToDateTime(DateTime.Now.ToLongTimeString());
                        C.OrderDetails.Add(ordetay);
                        C.SaveChanges();
                        var plsproduct = productRepo.TGet((int)item.Product.ProductId);
                        plsproduct.BuyCount += (int)item.Adet;
                        plsproduct.Stock -= (int)item.Adet;
                        productRepo.TUpdate(plsproduct);

                        var aa = C.OrderDetails.Where(x => x.ProductId == ordetay.ProductId).Select(x => x.ProductId).FirstOrDefault();
                        var bbb = C.Products.Where(x => x.ProductId == aa).Select(x => x.Id).FirstOrDefault();
                        Notification nts = new(); nts.Id = bbb; nts.NotifiDate = Convert.ToDateTime(DateTime.Now.ToLongTimeString());
                        nts.NotifiTypeID = 7; nts.SeenStatus = false; C.Notifications.Add(nts); C.SaveChanges();

                    }
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", "");
                    CartProductCount = 0;
                    getwallet.Balance -= ViewBag.total;
                    C.Wallets.Update(getwallet);
                    C.SaveChanges();
                    return Json("succe");
                }
                return Json("");
            }
            catch (Exception)
            {
                if (Globals.LoginStatus == 0)
                {
                    return Json("userid");
                }
                return Json("processerror");
            }
        }

    }
}

