using DATN.MVC.Helpers;
using DATN.MVC.Models;
using DATN.MVC.Models.Request;
using DATN.MVC.Models.Response;
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

        [HttpPost]
        public JsonResult LoginAccount([FromBody] LoginRequest loginRequest)
        {
            // Gọi API đăng nhập qua ApiHelpers và nhận về đối tượng LoginRes
            var result = ApiHelpers.PostMethodAsync<LoginResponse, LoginRequest>("https://localhost:7296/api/Auth/login", loginRequest);

            if (result != null)
            {
                // Lưu token vào session nếu đăng nhập thành công
                HttpContext.Session.SetInt32("UserID", result.ID);
                HttpContext.Session.SetString("FullName", result.FullName);
                HttpContext.Session.SetString("RoleName", result.RoleName);
                HttpContext.Session.SetString("Token", result.Token);
                HttpContext.Session.SetString("UserImage", result.UserImage);

                


                return Json(new { success = true, message = "Login successful." });
            }
            else
            {
                return Json(new { success = false, message = "Login failed." });
            }
        }
    }
}
