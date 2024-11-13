using DATN.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DATN.MVC.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {

            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId != null)
            {
                ViewBag.UserId = userId;
                ViewBag.FullName = HttpContext.Session.GetString("FullName");
                ViewBag.Token = HttpContext.Session.GetString("Token");
                var viewSettings = new ViewSettings
                {
                    ShowSidebar = true, // Tắt sidebar
                    ShowHeader = true,   // Bật header
                    ShowFriendList = true // Tắt danh sách bạn bè
                };
                ViewBag.ViewSettings = viewSettings;

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
