using DATN.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DATN.MVC.Controllers
{
    public class AccountController : Controller
    {

        public IActionResult Login()
        {
            var viewSettings = new ViewSettings
            {
                ShowSidebar = false, // Tắt sidebar
                ShowHeader = false,   // Bật header
                ShowFriendList = false // Tắt danh sách bạn bè
            };
            ViewBag.ViewSettings = viewSettings;
            return View();
        }
        public IActionResult Register()
        {
            var viewSettings = new ViewSettings
            {
                ShowSidebar = false, // Tắt sidebar
                ShowHeader = false,   // Bật header
                ShowFriendList = false // Tắt danh sách bạn bè
            };
            ViewBag.ViewSettings = viewSettings;
            return View();
        }
    }
}
