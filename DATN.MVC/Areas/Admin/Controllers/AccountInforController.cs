using DATN.MVC.Areas.Admin.Models.AccountInfor.Request;
using DATN.MVC.Areas.Admin.Models.AccountInfor.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DATN.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountInforController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountInforController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            List<AccountInforReadAllRes> accounts = new List<AccountInforReadAllRes>();

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://localhost:7296/api/AccountInfor/GetAll");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    accounts = JsonConvert.DeserializeObject<List<AccountInforReadAllRes>>(jsonData);
                }
                else
                {
                    ViewData["ErrorMessage"] = "Không thể lấy dữ liệu từ API.";
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Lỗi khi gọi API: {ex.Message}";
            }

            return View(accounts); // Tự động trỏ đến Views/AccountInfor/Index.cshtml
           }


//// Tạo tài khoản
//[HttpGet]
//public IActionResult Create()
//{
//    return View();
//}

//[HttpPost]
//public async Task<IActionResult> Create(AccountInforReq model)
//{
//    if (ModelState.IsValid)
//    {
//        try
//        {
//            var client = _httpClientFactory.CreateClient();
//            var jsonContent = JsonConvert.SerializeObject(model);
//            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

//            var response = await client.PostAsync("https://localhost:7296/api/AccountInfor/Create", content);

//            if (response.IsSuccessStatusCode)
//            {
//                TempData["SuccessMessage"] = "Thêm tài khoản thành công!";
//                return RedirectToAction("Index");
//            }
//            else
//            {
//                var errorContent = await response.Content.ReadAsStringAsync();
//                ModelState.AddModelError("", $"API Error: {errorContent}");
//            }
//        }
//        catch (Exception ex)
//        {
//            ModelState.AddModelError("", $"Lỗi hệ thống: {ex.Message}");
//        }
//    }

//    TempData["ErrorMessage"] = "Dữ liệu không hợp lệ!";
//    return View(model);
//}

// Sửa thông tin tài khoản


[HttpGet]
        public async Task<IActionResult> GetId(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"https://localhost:7296/api/AccountInfor/GetById/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var account = JsonConvert.DeserializeObject<AccountInforReadAllRes>(jsonData);

                    var model = new AccountInforReq
                    {
                        Id = account.Id,
                        UserName = account.UserName,
                      
                        IsActived = account.IsActived
                    };

                    return View(model);
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy tài khoản.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi hệ thống: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // Cập nhật thông tin tài khoản
        [HttpPost]
        public async Task<IActionResult> Edit(AccountInforReq model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Id <= 0)
                    {
                        ModelState.AddModelError("", "ID không hợp lệ.");
                        return View(model);
                    }

                    var client = _httpClientFactory.CreateClient();
                    var jsonContent = JsonConvert.SerializeObject(model);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await client.PutAsync("https://localhost:7296/api/AccountInfor/EditStatus", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Cập nhật tài khoản thành công!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError("", $"Lỗi từ API: {errorContent}");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Lỗi hệ thống: {ex.Message}");
                }
            }

            TempData["ErrorMessage"] = "Dữ liệu không hợp lệ!";
            return View(model);
        }

        // Xóa tài khoản
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.DeleteAsync($"https://localhost:7296/api/AccountInfor/Delete/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Xóa tài khoản thành công!";
                    return Json(new { success = true });
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Lỗi khi xóa tài khoản: {errorContent}";
                    return Json(new { success = false });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi hệ thống: {ex.Message}";
                return Json(new { success = false });
            }
        }
    }
}
