using DATN.MVC.Areas.Admin.Models.AdminSalePlace.Response;
using DATN.MVC.Areas.Admin.Models.AdminSalePlace.Request;
using DATN.MVC.Areas.Admin.Models.AdminCategory.Response;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DATN.MVC.Areas.Admin.Models.AdminPost.Request;
using DATN.MVC.Areas.Admin.Models.AdminPost.Response;
using System.Text;
using DATN.MVC.Controllers;

namespace DATN.MVC.Areas.Admin.Controllers.AdminSalePlaces
{
    [Area("Admin")]

    public class AdminManageSalePlaceController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminManageSalePlaceController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IActionResult> Index()
        {
            List<ViewSalePlace_Res> post = new List<ViewSalePlace_Res>();

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://localhost:7296/api/ManageSalePlace/GetAll");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();

                    // Deserialize vào ApiResponse thay vì List<ParentCategoriesReadAll_Res> trực tiếp
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<ViewSalePlace_Res>>(jsonData);
                    if (apiResponse != null && apiResponse.Data != null)
                    {
                        post = apiResponse.Data;
                    }
                    else
                    {
                        ViewData["ErrorMessage"] = "Dữ liệu từ API không hợp lệ.";
                    }
                }
                else
                {
                    ViewData["ErrorMessage"] = "Không thể lấy dữ liệu từ API. Mã trạng thái: " + response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Đã xảy ra lỗi khi gọi API: " + ex.Message;
            }

            return View(post);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7296/api/ManageSalePlace/GetSalePlace/{Id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ViewSalePlace_Res>(jsonData);

                var model = new EditSalePlaceStatus_Req
                {
                    Id = apiResponse.Id, // Đảm bảo 'Id' không null
                    Status = apiResponse.Status
                };


                return View(model); // Trả về EditPostStatus_Req cho View
            }
            else
            {
                Console.WriteLine("API response failed");
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateStatusSalePlace(EditSalePlaceStatus_Req model)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient();
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                try
                {
                    var response = await client.PutAsync($"https://localhost:7296/api/ManageSalePlace/EditStatus/{model.Id}", content); // Đảm bảo URL đúng

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Cập nhật trạng thái gian hàng thành công!";
                        return RedirectToAction("Index", "AdminManageSalePlace");
                    }
                    else
                    {
                        var errorDetails = await response.Content.ReadAsStringAsync();
                        TempData["ErrorMessage"] = $"Có lỗi khi cập nhật gian hàng. Mã lỗi: {response.StatusCode}. Chi tiết: {errorDetails}";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Không thể kết nối đến API. Vui lòng kiểm tra lại kết nối. Lỗi: {ex.Message}";
                }
            }

            return View("Edit", model);
        }


        [HttpPost]
        public async Task<IActionResult> deleteSalePlace(int id)
        {
            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.DeleteAsync($"https://localhost:7296/api/ManageSalePlace/DeleteSalePlace/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Gian hàng đã được xóa thành công!" });
                }
                else
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = $"Không thể xóa Gian hàng. Chi tiết: {errorDetails}" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }

    }
}
