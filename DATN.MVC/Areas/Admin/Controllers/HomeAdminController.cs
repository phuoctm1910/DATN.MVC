using DATN.MVC.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DATN.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeAdminController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Chart() 
        { 
            return View();
        }

        public IActionResult StatisticalUsers()
        {
            return View();
        }

        public IActionResult StatisticalPosts()
        {
            return View();
        }

        public IActionResult StatisticalProducts()
        {
            return View();
        }
        public IActionResult StatisticalMessages()
        {
            return View();
        }
        public IActionResult Table()
        {

            return View();

        }
    }
}
