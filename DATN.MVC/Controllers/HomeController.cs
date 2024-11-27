using DATN.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DATN.MVC.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
           
                var viewSettings = new ViewSettings
                {
                    ShowSidebar = true, // Tắt sidebar
                    ShowHeader = true,   // Bật header
                    ShowFriendList = true // Tắt danh sách bạn bè
                };
                ViewBag.ViewSettings = viewSettings;

                return View();

        }
    
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
