using DATN.MVC.Models;
using DATN.MVC.Request.User;
using DATN.MVC.Respone.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DATN.MVC.Controllers
{
    public class UserController : Controller
    {
        // GET: UserController
        public ActionResult About()
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

        public ActionResult Notification()
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

        public ActionResult Friends()
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

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private readonly string _apiBaseUrl = "https://localhost:7296/api/User/update";

        public async Task<IActionResult> UpdateInformation()
        {
            using (var httpClient = new HttpClient())
            {
                int id = 1;
                HttpResponseMessage response = await httpClient.GetAsync($"https://localhost:7296/api/User/get/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var userInfo = JsonConvert.DeserializeObject<User_GetUserInfoRes>(jsonData);
                    return View(userInfo);
                }
                else
                {
                    ViewBag.Error = "Không thể tải thông tin người dùng. Vui lòng thử lại sau.";
                    return View();
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInformation(Update_UserReq request)
        {
            var file = Request.Form.Files["ImageFile"];
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/images", file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                request.Image = "/images/" + file.FileName;
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Không nhập đủ thông tin. Vui lòng thử lại.";
                return View();
            }

            using (var httpClient = new HttpClient())
            {
                string jsonData = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PutAsync(_apiBaseUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Cập nhật thông tin thành công.";
                    return Redirect("/");
                }
                else
                {
                    ViewBag.Error = "Đã xảy ra lỗi khi cập nhật thông tin. Vui lòng thử lại.";
                }
            }

            return View();
        }
    }
}
