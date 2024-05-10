using DataAccess.Concrete;
using DataAccess.Repositories;
using Entities.Concrete;
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
            //var orderwithuser = C.Orders.OrderByDescending(x=>x.OrderDate).Where(x => x.Id == Userid).ToList();
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
            //var vales = C.OrderDetails.Include(x => x.OrderProcess).Include(y => y.Product).Where(x => x.OrderId == id).ToList();
            var vales = ordersRepo.GetListDetails(id);
            ViewBag.total = vales.Sum(item => item.Price * item.Quantity);
            ViewBag.total = Math.Round(ViewBag.total, 2);
            ViewBag.Orderno = id;
            return View(vales);
        }



        [HttpGet]
        public IActionResult Orderpdfview(int id)
        {
            //var list = C.OrderDetails.Where(x => x.OrderId == id).Include(x => x.Product).ToList();
            var list = ordersRepo.GetListDetails(id);
            //var listdata = C.OrderDetails.Where(x => x.OrderId == id).Include(x => x.Product).FirstOrDefault();
            var listdata = ordersRepo.GetOrderId(id);
            ViewBag.orderid = "0000" + listdata.OrderId;
            ViewBag.orderdate = listdata.Deliverydate;

            return View(list);
        }





        //[HttpGet]
        //public FileResult Orderpdfgen(int id)
        //{
        //    #region NORMAL KLASİK ÇALIŞIYOR
        //    var ordate = C.Orders.Where(x => x.OrderID == id).Select(x => x.OrderDate.ToShortDateString()).FirstOrDefault().ToString();
        //    var mylist = C.OrderDetails.Include(x => x.Product).Where(x => x.OrderId == id).ToList();
        //    var myhtml = "";
        //    myhtml += "<!DOCTYPE html><html><body>";
        //    myhtml += "<head> <link rel=\"stylesheet\" href=\"https://cdn.jsdelivr.net/npm/bootstrap@4.4.1/dist/css/bootstrap.min.css\" integrity=\"sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh\" crossorigin=\"anonymous\"> </ head>";
        //    myhtml += "<p>Sipariş Numarası : <b>" + id + "</b></p>";
        //    myhtml += "<p>Sipariş Tarihi : <b>" + ordate + "</b></p>";
        //    myhtml += "<div class=\"container col-md-10\">";
        //    myhtml += "<br />";
        //    myhtml += "<hr />";
        //    myhtml += "<table class=\"table table-bordered\"><tr><th>Görsel</th><th>Ürün</th><th>Fiyat</th><th>Adet</th><th>Ara Toplam</th></tr><tbody>";
        //    foreach (var item in mylist)
        //    {
        //        myhtml += "<img src=\"~/images/solimg/gibfat.png\" width=\"100\" height=\"100\">";
        //        myhtml += "<tr><td><img src=\"~" + item.Product.PhotoUrl + "\"  width=\"30\" height=\"30\"></td><td>" + item.Product.Name + "</td><td>" + item.Price + "</td><td>" + item.Quantity + " </td> <td>" + (item.Price * item.Quantity + " TL") + "</td>  </tr> ";
        //    }
        //    myhtml += "</tbody></table></div>";
        //    myhtml += "<hr />";
        //    myhtml += "<br />";
        //    myhtml += "<p>Toplam Ödeme : <b> " + mylist.Sum(x => x.Price * x.Quantity) + " TL </b></p>";
        //    myhtml += "<p>Ödeme Kanalı : <b> " + "Kredi Kartı İle Ödeme" + "</b></p>";
        //    myhtml += "</body></html>";
        //    string fileName = id + "NoluSiparisDetay" + ".pdf";
        //    byte[] byteArray = Encoding.UTF8.GetBytes(myhtml);
        //    MemoryStream stream = new(byteArray);
        //    HtmlLoadOptions options = new();
        //    Document pdfDocument = new(stream, options);
        //    Stream outputStream = new MemoryStream();
        //    pdfDocument.Save(outputStream);
        //    return File(outputStream, System.Net.Mime.MediaTypeNames.Application.Pdf, fileName);
        //    #endregion
        //}






        [HttpPost]
        public IActionResult Orderagain(int id)
        {
            return Redirect("/Order/Orderlist");
        }




        [HttpGet]
        public IActionResult OrderCancel(int id)
        {
           var aaa= C.OrderDetails.Where(x => x.OrderDetailId == id).Select(x=>x.ProductId).FirstOrDefault();
            var bbb = C.Products.Where(x => x.ProductId == aaa).Select(x=>x.Id).FirstOrDefault();
            Notification nts = new();
            nts.Id = bbb;
            nts.NotifiDate = Convert.ToDateTime(DateTime.Now.ToLongTimeString());
            nts.NotifiTypeID = 9;
            nts.SeenStatus = false;
            C.Notifications.Add(nts);C.SaveChanges();

            var values = orderDetail.TGet(id);
            values.OrderProcessID = 13;
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
