using DataAccess.Concrete;
using DataAccess.Repositories;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace Web_UI.Controllers
{
    public class LoginController : Controller
    {
        private bool _Status;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private Context C = new();
        private WalletRepo walletRepo = new();
        private AddressRepo _AddressRepo = new();

        public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [Route("/Account/Login")]
        [HttpGet]
        public IActionResult Index()
        {
            var sa = new AppUser
            {
                NameSurname = "emrah abay",
                UserName = "emrah40",
                Email = "deneme@email.com",
                ImageUrl = "base64000000000.com",
                About = "",
                EmailConfirmed = true,
            };
            var result = _userManager.CreateAsync(sa, "Ea123.*");
            if (result.Result.Succeeded)
            {

            }


            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(Userloginviewmodel p)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(p.Username, p.Password, false, true);
                if (result.Succeeded)
                {
                    Globals.LoginStatus = 1;
                    _Status = true;
                }
                else
                {
                    _Status = false;
                }
                //Globals.Userid = (int)_userManager.GetUserAsync(User).Result.Id;
            }
            return Json(_Status);
        }

        [HttpGet]
        public IActionResult Allnotifi()
        {
            var seen = C.Notifications.Where(x => x.SeenStatus == false).Count();
            ViewBag.newnotifi = seen;

            int userid = _userManager.GetUserAsync(User).Result.Id;
            var getnotifi = C.Notifications.OrderByDescending(x => x.NotifiDate).Include(x => x.NotifiType).Where(x => x.Id == userid).Take(5).ToList();
            return View(getnotifi);
        }

        [HttpGet]
        public IActionResult Allnotififull()
        {
            int userid = _userManager.GetUserAsync(User).Result.Id;
            var getnotifi = C.Notifications.OrderByDescending(x => x.NotifiDate).Include(x => x.NotifiType).Where(x => x.Id == userid).ToList();

            var query = from abc in C.Notifications
                        where abc.Id == userid
                        select abc;
            foreach (Notification ord in query)
            {
                ord.SeenStatus = true;
            }
            C.SaveChanges();
            ViewBag.allseen = true;
            return View(getnotifi);
        }

        [HttpGet]
        public IActionResult Notifiremove(int id)
        {
            var getrem = C.Notifications.Where(x => x.NotifiID == id).FirstOrDefault();
            C.Remove(getrem);
            C.SaveChanges();
            return Redirect("/Login/Allnotififull/");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            Globals.LoginStatus = 0;
            Globals.isStore = false;
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Signup() { return View(); }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Signup(Userregistermodel p)
        {
            //System.Threading.Thread.Sleep(1000);
            if (ModelState.IsValid)
            {
                AppUser user = new()
                {
                    Email = p.Mail,
                    UserName = p.Username,
                    NameSurname = p.NameSurname,
                    ImageUrl = "/images/userimg/nl.jpg",
                    Status = false
                };
                //var imagename = p.ImgUrl.FileName;
                //var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/userimg/", imagename);
                //var stream = new FileStream(location, FileMode.Create);
                //p.ImgUrl.CopyTo(stream);

                var result = await _userManager.CreateAsync(user, p.Password);
                if (result.Succeeded)
                {
                    _Status = true;
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            else
            {
                _Status = false;
            }
            return Json(_Status);
        }

        [AllowAnonymous]
        public IActionResult Page404()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("/Account/AccessDenied")]
        public IActionResult PageDenied()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult InfoHelp()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "admin,user,seller")]
        public IActionResult Account()
        {
            int userid = (int)_userManager.GetUserAsync(User).Result.Id;

            int reqcont = C.RequestSellers.Where(x => x.Id == userid).Count();
            int sellercont = C.UserRoles.Where(x => x.UserId == userid && x.RoleId == 2).Count();
            if (sellercont > 0)
            {
                ViewBag.sellerstatus = "accept";
            }
            if (reqcont > 0)
            {
                ViewBag.sellerstatus = "noaccept";
            }
            if (reqcont < 1 && sellercont < 1)
            {
                ViewBag.sellerstatus = "formshow";
            }

            double getwallet = C.Wallets.Where(x => x.Id == userid).Select(x => x.Balance).FirstOrDefault();
            int getwalletno = C.Wallets.Where(x => x.Id == userid).Select(x => x.WalletID).FirstOrDefault();
            ViewBag.walletno = getwalletno;
            ViewBag.walletbalance = getwallet;
            ViewBag.addresslist = _AddressRepo.TGet(userid);
            var getuser = C.Users.Where(x => x.Id == userid).FirstOrDefault();
            return View(getuser);
        }

        //string subject, string mailadres
        [HttpPost]
        public IActionResult Sendmail()
        {
            try
            {
                SmtpClient client = new("smtp.live.com", 587); //Burası aynı kalacak
                client.Credentials = new NetworkCredential("deneme@hotmail.com", "şifre");
                client.EnableSsl = true;
                MailMessage msj = new(); //Yeni bir MailMesajı oluşturuyoruz
                msj.From = new MailAddress("senramwil@hotmail.com", "Emrah" + " " + "ABAY"); //iletişim kısmında girilecek mail buaraya gelecektir
                msj.To.Add("ali_abay_01@hotmail.com"); //Buraya kendi mail adresimizi yazıyoruz
                msj.Subject = "Deneme konu"; //Buraya iletişim sayfasında gelecek konu ve mail adresini mail içeriğine yazacaktır
                msj.Body = "BURASI BODYY MESAJJJ GÖVDEEE"; //Mail içeriği burada aktarılacakır
                client.Send(msj); //Clien sent kısmında gönderme işlemi gerçeklecektir.
                                  //Bu kısımdan itibaren sizden kullanıcıya gidecek mail bilgisidir 
                MailMessage msj1 = new();
                msj1.From = new MailAddress("Kendimailadresiniz", "İstediğini bir ad yazabilirsiniz");
                msj1.To.Add("deneme@hotmail.com"); //Buraua iletişim sayfasında gelecek mail adresi gelecktir.
                msj1.Subject = "Mail'inize Cevap";
                msj1.Body = "Size En kısa zamanda Döneceğiz. Teşekkür Ederiz Bizi tercih ettiğiniz için";
                client.Send(msj1);
                ViewBag.Succes = "teşekkürler Mailniz başarı bir şekilde gönderildi"; //Bu kısımlarda ise kullanıcıya bilgi vermek amacı ile olur
                return Redirect("/Login/Account/");
            }
            catch (Exception)
            {
                ViewBag.Error = "Mesaj Gönderilken hata olıuştu"; //Bu kısımlarda ise kullanıcıya bilgi vermek amacı ile olur
                return Redirect("/Login/Account/");
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin,user,seller")]
        public async Task<IActionResult> Userupdate(AppUser apu)
        {
            try
            {
                var values = await _userManager.FindByNameAsync(apu.UserName);
                values.NameSurname = apu.NameSurname;
                //values.UserName = apu.UserName;
                values.PhoneNumber = apu.PhoneNumber;
                values.Job = apu.Job;
                values.About = apu.About;
                var result = await _userManager.UpdateAsync(values);
                return Redirect("/Login/Account/");
            }
            catch (Exception)
            {
                return Redirect("/Login/Account/");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Newpassword(Passwordchange pwd)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Redirect("/Account/Login/");
                }
                var result = await _userManager.ChangePasswordAsync(user, pwd.Currentpassword, pwd.Newpassword);
                if (!result.Succeeded)
                {
                    foreach (var errors in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, errors.Description);
                    }
                    return Json("");
                }
                await _signInManager.RefreshSignInAsync(user);
                return Json(true);
            }
            return Json("");
        }

        [HttpPost]
        //[Authorize(Roles = "user")]
        public async Task<IActionResult> Sellerrequest(AppUser apu, UserBankAcco pusb)
        {
            //Var olan kullanıcının Fullname ini Mağaza adı olarak alma
            var values = await _userManager.FindByNameAsync(apu.UserName);
            values.NameSurname = apu.NameSurname;
            values.StoreAddress = apu.StoreAddress;
            values.StoreTaxNo = apu.StoreTaxNo;
            values.StoreTaxOffice = apu.StoreTaxOffice;
            await _userManager.UpdateAsync(values);
            //Satıcı Olma isteği ile beraber hesap ekleme
            UserBankAcco usb = new();
            usb.Bankname = pusb.Bankname;
            usb.AccountNo = pusb.AccountNo;
            usb.IBANNo = pusb.IBANNo;
            usb.Id = _userManager.GetUserAsync(User).Result.Id;
            C.UserBankAccos.Add(usb); C.SaveChanges();
            //Satıcı Olma taleplerini moderatorun görüp onay verebilmesi için 
            RequestSeller rqs = new();
            rqs.RequestDate = Convert.ToDateTime(DateTime.Now.ToLongTimeString());
            rqs.Id = _userManager.GetUserAsync(User).Result.Id;
            C.RequestSellers.Add(rqs); C.SaveChanges();
            return Redirect("/Login/Account/");
        }

        [HttpGet]
        public IActionResult CancelSellerRequest()
        {
            int uid = _userManager.GetUserAsync(User).Result.Id;
            var getrequ = C.RequestSellers.Where(x => x.Id == uid).FirstOrDefault();
            C.RequestSellers.Remove(getrequ);
            C.SaveChanges();
            return Redirect("/Login/Account/");
        }

        [HttpGet]
        public IActionResult CancelSeller()
        {
            int userid = _userManager.GetUserAsync(User).Result.Id;
            var control = C.UserRoles.Where(x => x.UserId == userid && x.RoleId == 2).FirstOrDefault();
            C.UserRoles.Remove(control);
            C.SaveChanges();
            return Redirect("/Login/Account/");
        }

        [HttpGet]
        public IActionResult AddressList()
        {
            int Userid = (int)_userManager.GetUserAsync(User).Result.Id;
            return View(_AddressRepo.TGet(Userid));

        }

        [HttpPost]
        public IActionResult AddressAdd(Address adr)
        {
            adr.Id = (int)_userManager.GetUserAsync(User).Result.Id;
            _AddressRepo.TAdd(adr);
            return Redirect("/Login/Account/");
        }

        [HttpPost]
        [Authorize(Roles = "user,seller")]
        public IActionResult Walletadd(Wallet wt)
        {
            wt.Id = (int)_userManager.GetUserAsync(User).Result.Id;
            wt.Status = true;
            wt.Balance = 0;
            walletRepo.TAdd(wt);
            return Redirect("/Login/Account/");
        }

        [HttpGet]
        [Authorize(Roles = "user,seller")]
        public IActionResult Walletdelete()
        {
            int userid = _userManager.GetUserAsync(User).Result.Id;
            var getir = C.Wallets.Where(x => x.Id == userid).FirstOrDefault();
            C.Wallets.Remove(getir);
            C.SaveChanges();
            return Redirect("/Login/Account/");
        }

        [HttpPost]
        public IActionResult Walletbalanceadd(int money)
        {
            int userid = _userManager.GetUserAsync(User).Result.Id;
            var getir = C.Wallets.Where(x => x.Id == userid).FirstOrDefault();
            getir.Balance += money;
            walletRepo.TUpdate(getir);
            return Redirect("/Login/Account/");
        }

    }
}