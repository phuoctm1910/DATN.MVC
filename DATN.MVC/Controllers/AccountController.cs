using DATN.MVC.Helpers;
using DATN.MVC.Models;
using DATN.MVC.Request.Registration;
using DATN.MVC.Models.Request;
using DATN.MVC.Models.Response;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.IdentityModel.Tokens.Jwt;

namespace DATN.MVC.Controllers
{
    public class AccountController : Controller
    {
        private Dictionary<string, string> DecodeJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(token))
                throw new ArgumentException("Invalid token");

            var jwtToken = handler.ReadJwtToken(token);

            // Lấy toàn bộ các claim từ token
            return jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
        }

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
            // Gọi API đăng nhập qua ApiHelpers và nhận về token JWT
            var result = ApiHelpers.PostMethodAsync<LoginResponse, LoginRequest>("https://localhost:7296/api/Auth/login", loginRequest);

            if (result != null)
            {
                // Lưu token vào Session
                HttpContext.Session.SetString("Token", result.Token);

                // Không cần giải mã hoặc lưu thêm dữ liệu, Middleware đã xử lý

                return Json(new { success = true, message = "Login successful." });
            }
            else
            {
                return Json(new { success = false, message = "Login failed." });
            }
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
