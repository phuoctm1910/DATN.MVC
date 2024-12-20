using DATN.MVC.Areas.Admin.Models.AdminCategory.Response;
using DATN.MVC.Areas.Admin.Models.AdminSalePlace.Request;
using DATN.MVC.Areas.Admin.Models.AdminSalePlace.Response;
using DATN.MVC.Request.SalePlaceApprove;
using DATN.MVC.Respone.SalePlaceApprove;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DATN.MVC.Areas.Admin.Controllers.AdminSalePlaceApprove
{
    [Area("Admin")]

    public class AdminSalePlaceApproveController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminSalePlaceApproveController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            List<ApproveSalePlace_Res> ApproveList = new List<ApproveSalePlace_Res>();

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://localhost:7296/api/ApproveSalePlace/GetAll");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();

                    // Deserialize vào ApiResponse thay vì List<ParentCategoriesReadAll_Res> trực tiếp
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<ApproveSalePlace_Res>>(jsonData);
                    if (apiResponse != null && apiResponse.Data != null)
                    {
                        ApproveList = apiResponse.Data;
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

            return View(ApproveList);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7296/api/ApproveSalePlace/GetSalePlace/{Id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApproveSalePlace_Res>(jsonData);

                var model = new GetSalePlaceByID_Req
                {
                    Id = apiResponse.Id // Đảm bảo 'Id' không null
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
        public async Task<IActionResult> ApproveSalePlaceStatus([FromBody] ApproveSalePlace_Req model)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient();
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                try
                {
                    var response = await client.PutAsync($"https://localhost:7296/api/ApproveSalePlace/Approve/", content);

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
    }
}
