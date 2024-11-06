using DATN.MVC.Helpers;
using DATN.MVC.Models;
using DATN.MVC.Request.Registration;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

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

        [HttpPost]
        public JsonResult RegistrationAccount([FromBody] RegistrationRequest RegistrationRequest)
        {
            // Gọi API đăng ký qua ApiHelpers và nhận về đối tượng LoginRes
            var result = ApiHelpers.PostMethodAsync<bool,RegistrationRequest>("https://localhost:7296/api/Registration/create", RegistrationRequest);

            if (result)
            {
              
                return Json(new { success = true, message = "Registration successful." });
            }
            else
            {
                return Json(new { success = false, message = "Registration failed." });
            }
        }
    }
}
