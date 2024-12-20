using DATN.MVC.Areas.Admin.Models.AdminCategory.Request;
using DATN.MVC.Areas.Admin.Models.AdminCategory.Response;
using DATN.MVC.Areas.Employee.Models.EmployeProduct.Request;
using DATN.MVC.Areas.Employee.Models.EmployeProduct.Response;
using DATN.MVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DATN.MVC.Areas.Employee.Controllers.EmployeProduct
{
    [Area("Employee")]
    public class EmployeProductController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EmployeProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }




        public async Task<IActionResult> Index()
        {
            List<ProductGetAll_Res> Product = new List<ProductGetAll_Res>();

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://localhost:7296/EmployeProductGetAll");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();

                    // Deserialize vào ApiResponse thay vì List<ParentCategoriesReadAll_Res> trực tiếp
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<ProductGetAll_Res>>(jsonData);
                    if (apiResponse != null && apiResponse.Data != null)
                    {
                        Product = apiResponse.Data;
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

            return View(Product);
        }





        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7296/EmployeProductGetById/{id}"); // Kiểm tra lại URL chính xác của API

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var Product = JsonConvert.DeserializeObject<EditProductState_Req>(jsonResponse);

                // Kiểm tra nếu category có null
                if (Product == null)
                {
                    // Log hoặc xử lý khi category là null
                    Console.WriteLine("Product is null");
                    return RedirectToAction("Index");
                }

                return View(Product); // Trả về view Edit với model là category
            }
            else
            {
                Console.WriteLine("API response failed");
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public JsonResult UpdateProductState([FromBody] EditProductState_Req request)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient();
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Chèn ID vào URL
                var apiUrl = $"https://localhost:7296/Employe-update-product-status/";

                Console.WriteLine($"URL: {apiUrl}");
                Console.WriteLine($"Payload: {jsonContent}");

                try
                {
                    var response = client.PutAsync(apiUrl, content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { success = true, message = "Cập nhật sản phẩm thành công" });
                    }
                    else
                    {
                        var errorDetails = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine($"Error Status: {response.StatusCode}");
                        Console.WriteLine($"Error Details: {errorDetails}");

                        return Json(new { success = false, message = $"Có lỗi khi cập nhật sản phẩm. Mã lỗi: {response.StatusCode}. Chi tiết: {errorDetails}" });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception khi gửi yêu cầu PUT: " + ex.Message);
                    return Json(new { success = false, message = "Không thể kết nối đến API. Vui lòng kiểm tra lại kết nối." });
                }
            }

            return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
        }





    }
}
