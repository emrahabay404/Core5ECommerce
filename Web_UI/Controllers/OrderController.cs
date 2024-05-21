using DataAccess.Concrete;
using DataAccess.Repositories;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Web_UI.Controllers
{
    public class OrderController : Controller
    {
        public Context C = new();
        public OrdersRepo ordersRepo = new();
        public ProductRepo productRepo = new();
        public Product Product = new();
        public Photo photo = new();

        public OrderDetailRepo orderDetail = new();

        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Orderlist()
        {

            int Userid = (int)_userManager.GetUserAsync(User).Result.Id;
            var orderwithuser = ordersRepo.GetListWith(Userid);
            return View(orderwithuser);
        }

        [HttpGet]
        public IActionResult Prdimglist(int id)
        {
            var vales = C.OrderDetails.Include(x => x.Order).Include(y => y.Product).Where(x => x.OrderId == id).ToList();
            return View(vales);
        }

        [HttpGet]
        public IActionResult OrderDetail(int id)
        {
            var vales = ordersRepo.GetListDetails(id);
            ViewBag.total = vales.Sum(item => item.Price * item.Quantity);
            ViewBag.total = Math.Round(ViewBag.total, 2);
            ViewBag.Orderno = id;
            return View(vales);
        }

        [HttpGet]
        public IActionResult Orderpdfview(int id)
        {
            var list = ordersRepo.GetListDetails(id);
            var listdata = ordersRepo.GetOrderId(id);
            ViewBag.orderid = "0000" + listdata.OrderId;
            ViewBag.orderdate = listdata.Deliverydate;
            return View(list);
        }

        [HttpPost]
        public IActionResult Orderagain(int id)
        {
            return Redirect("/Order/Orderlist");
        }

        [HttpGet]
        public IActionResult OrderCancel(int id)
        {
            var aaa = C.OrderDetails.Where(x => x.OrderDetailId == id).Select(x => x.ProductId).FirstOrDefault();
            var bbb = C.Products.Where(x => x.ProductId == aaa).Select(x => x.Id).FirstOrDefault();
            Notification nts = new();
            nts.Id = bbb;
            nts.NotifiDate = Convert.ToDateTime(DateTime.Now.ToLongTimeString());
            nts.NotifiTypeID = 10;
            nts.SeenStatus = false;
            C.Notifications.Add(nts); C.SaveChanges();
            var values = orderDetail.TGet(id);
            values.OrderProcessID = 5;
            orderDetail.TUpdate(values);
            return Redirect("/Order/Orderlist/");
        }

        [HttpGet]
        public JsonResult Ordercompletecontrol(int id)
        {
            var getcontrol = C.OrderDetails.Where(x => x.OrderId == id).Select(x => x.OrderProcessID == 12).Count();
            if (getcontrol != 0)
            {
                return Json("orderok");
            }
            else
            {
                return Json("");
            }
        }

    }
}
