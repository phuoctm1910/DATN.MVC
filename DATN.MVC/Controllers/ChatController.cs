using DATN.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DATN.MVC.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            var viewSettings = new ViewSettings
            {
                ShowSidebar = false, // Tắt sidebar
                ShowHeader = true,   // Bật header
                ShowFriendList = false // Tắt danh sách bạn bè
            };
            ViewBag.ViewSettings = viewSettings;

            return View();
        }
    }
}
