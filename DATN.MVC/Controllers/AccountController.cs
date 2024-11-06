using DATN.MVC.Helpers;
using DATN.MVC.Models;
using DATN.MVC.Request.Registration;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace DATN.MVC.Controllers
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class AccountController : Controller
    {
        private static List<User> users = new List<User>
        {
            new User { Id = 1, FullName = "Full Name 1", UserName = "phuoc", Password = "123456" },
            new User { Id = 2, FullName = "Full Name 2", UserName = "duy", Password = "123456" }  
        };
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

        // Xử lý POST yêu cầu đăng nhập
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Tìm người dùng dựa trên tên đăng nhập và mật khẩu
            var user = users.FirstOrDefault(u => u.UserName == username && u.Password == password);

            if (user != null)
            {
                // Nếu thông tin chính xác, set session
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("FullName", user.FullName);

                // Chuyển hướng tới trang chính sau khi đăng nhập thành công
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Nếu thông tin không chính xác, trả về lỗi
                ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng";
                return View();
            }
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
