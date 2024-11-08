using Microsoft.AspNetCore.Mvc;

namespace DATN.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Chart() 
        { 
            return View();
        }
        public IActionResult Table()
        {

            return View();

        }
    }
}
