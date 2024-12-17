using DATN.MVC.Areas.Admin.Models.ManagerProduct.Request;
using DATN.MVC.Areas.Admin.Models.ManagerProduct.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DATN.MVC.Areas.Admin.Controllers.AdminProducts
{
    [Area("Admin")]
    public class ProductAdminController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductAdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            List<ProductReadAllRes> products = new List<ProductReadAllRes>();
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://localhost:7296/api/Product/GetAll");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    products = JsonConvert.DeserializeObject<List<ProductReadAllRes>>(jsonData) ?? new List<ProductReadAllRes>();
                }
                else
                {
                    ViewData["ErrorMessage"] = $"Không thể lấy danh sách sản phẩm. Mã lỗi: {response.StatusCode}, Nội dung: {await response.Content.ReadAsStringAsync()}";
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Lỗi hệ thống: {ex.Message}";
            }
            return View(products);
        }

        // Hiển thị danh sách sản phẩm theo ID
        [HttpGet]
        public async Task<IActionResult> GetId(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"https://localhost:7296/api/Product/Get/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var product = JsonConvert.DeserializeObject<ProductReadAllRes>(jsonData);

                    var model = new ProductReq
                    {
                        Id = product.Id,
                        Name = product.Name,
                        IsActived = product.IsActived
                    };

                    return View(model);
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy sản phẩm.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi hệ thống: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // Cập nhật trạng thái sản phẩm (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(ProductReq model)
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
                    var response = await client.PutAsync("https://localhost:7296/api/Product/EditStatus", content);

                    // Kiểm tra phản hồi từ API
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Cập nhật trạng thái sản phẩm thành công!";
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

        // Xóa sản phẩm
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { success = false, message = "ID sản phẩm không hợp lệ." });
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.DeleteAsync($"https://localhost:7296/api/Product/Delete/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Sản phẩm đã được xóa thành công!" });
                }
                else
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = $"Lỗi khi xóa sản phẩm: {errorDetails}" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }
    }
}
