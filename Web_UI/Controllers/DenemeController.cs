using Microsoft.AspNetCore.Mvc;

namespace Web_UI.Controllers
{
    public class DenemeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

    }
}
