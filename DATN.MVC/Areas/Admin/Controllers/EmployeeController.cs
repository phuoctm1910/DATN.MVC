using DATN.MVC.Areas.Admin.Models.ManagerEmployee.Request;
using DATN.MVC.Areas.Admin.Models.ManagerEmployee.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DATN.MVC.Areas.Admin.Controllers.AdminEmployees
{
    [Area("Admin")]
    public class EmployeeAdminController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EmployeeAdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Hiển thị danh sách nhân viên
        public async Task<IActionResult> Index()
        {
            List<EmployeeReadAllRes> employees = new List<EmployeeReadAllRes>();

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://localhost:7296/api/Employee/GetAll");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    employees = JsonConvert.DeserializeObject<List<EmployeeReadAllRes>>(jsonData);
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

            return View(employees);
        }

        // Tạo nhân viên
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeReq model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    var jsonContent = JsonConvert.SerializeObject(model);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("https://localhost:7296/api/Employee/Create", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Thêm nhân viên thành công!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError("", $"API Error: {errorContent}");
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

        // Sửa thông tin nhân viên
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"https://localhost:7296/api/Employee/GetEmployeeById/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var employee = JsonConvert.DeserializeObject<EmployeeReadAllRes>(jsonData);

                    var model = new EmployeeReq
                    {
                        Id = employee.Id,
                        UserName = employee.UserName,
                        PasswordHash = employee.PasswordHash,
                        IsActived = employee.IsActived
                    };

                    return View(model);
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy nhân viên.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi hệ thống: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // Cập nhật thông tin nhân viên
        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeReq model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra dữ liệu đầu vào (như ID, trạng thái hoạt động)
                    if (model.Id <= 0)
                    {
                        ModelState.AddModelError("", "ID không hợp lệ.");
                        return View(model);
                    }

                    var client = _httpClientFactory.CreateClient();

                    // Serialize dữ liệu thành JSON
                    var jsonContent = JsonConvert.SerializeObject(model);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // Gửi PUT request đến API mà không cần ID trong URL
                    var response = await client.PutAsync("https://localhost:7296/api/Employee/EditStatus", content);

                    // Kiểm tra phản hồi từ API
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Cập nhật nhân viên thành công!";
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

            // Nếu model không hợp lệ hoặc có lỗi từ API, trả lại view với lỗi
            TempData["ErrorMessage"] = "Dữ liệu không hợp lệ!";
            return View(model);
        }


        // Xóa nhân viên
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.DeleteAsync($"https://localhost:7296/api/Employee/Delete/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Xóa nhân viên thành công!";
                    return Json(new { success = true });
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Lỗi khi xóa nhân viên: {errorContent}";
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
