using DATN.MVC.Areas.Admin.Models.AdminCategory.Request;
using DATN.MVC.Areas.Admin.Models.AdminCategory.Response;
using DATN.MVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DATN.MVC.Areas.Admin.Controllers.AdminCategory
{
    [Area("Admin")]
    public class AdminCategoryController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminCategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }



        public async Task<IActionResult> Index()
        {
            List<ParentCategoriesReadAll_Res> parentCategories = new List<ParentCategoriesReadAll_Res>();

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://localhost:7296/api/ParentCategories/getAll");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();

                    // Deserialize vào ApiResponse thay vì List<ParentCategoriesReadAll_Res> trực tiếp
                    var apiResponse = JsonConvert.DeserializeObject<List<ParentCategoriesReadAll_Res>>(jsonData);
                    if (apiResponse != null)
                    {
                        parentCategories = apiResponse;
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

            return View(parentCategories);
        }



        // GET: AdminCategory/CreateParentCategory
        // GET: AdminCategory/CreateParentCategory
        public IActionResult CreateParentCategory()
        {
            return View();
        }


        // POST: AdminCategory/CreateParentCategory
        [HttpPost]
        public async Task<IActionResult> CreateParentCategory([FromBody] ParentCategories_Req model)
        {
            if (ModelState.IsValid)
            {
                // Tạo client HTTP để gửi yêu cầu đến API
                var client = _httpClientFactory.CreateClient();
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                try
                {
                    // Gửi yêu cầu POST tới API để tạo danh mục cha
                    var response = await client.PostAsync("https://localhost:7296/api/ParentCategories/create-parent-category", content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Trả về kết quả thành công dưới dạng JSON
                        return Json(new { success = true, message = "Thêm danh mục thành công!" });
                    }
                    else
                    {
                        // Nếu có lỗi khi gọi API, trả về thông báo lỗi
                        return Json(new { success = false, message = "Có lỗi xảy ra khi tạo danh mục. Vui lòng thử lại." });
                    }
                }
                catch (Exception ex)
                {
                    // Trường hợp xảy ra lỗi ngoài mong đợi (ví dụ: không thể kết nối API)
                    return Json(new { success = false, message = $"Lỗi hệ thống: {ex.Message}" });
                }
            }

            // Nếu Model không hợp lệ, trả về thông báo lỗi
            return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
        }





        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7296/api/ParentCategories/{id}"); // Kiểm tra lại URL chính xác của API

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var category = JsonConvert.DeserializeObject<ParentCategories_Res>(jsonResponse);

                // Kiểm tra nếu category có null
                if (category == null)
                {
                    // Log hoặc xử lý khi category là null
                    Console.WriteLine("Category is null");
                    return RedirectToAction("Index");
                }

                return View(category); // Trả về view Edit với model là category
            }
            else
            {
                Console.WriteLine("API response failed");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public JsonResult UpdateParentCategory([FromBody] ParentCategories_Req request)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient();
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                try
                {
                    var response = client.PutAsync($"https://localhost:7296/api/ParentCategories/update-parent-category", content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        // Nếu thành công, trả về success = true
                        return Json(new { success = true, message = "Cập nhật danh mục thành công" });
                    }
                    else
                    {
                        // Nếu có lỗi, trả về success = false và chi tiết lỗi
                        var errorDetails = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine($"Error Status: {response.StatusCode}");
                        Console.WriteLine($"Error Details: {errorDetails}");

                        return Json(new { success = false, message = $"Có lỗi khi cập nhật danh mục. Mã lỗi: {response.StatusCode}. Chi tiết: {errorDetails}" });
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



        [HttpPost]
        public JsonResult DeleteParentCategory(int id)
        {
            var client = _httpClientFactory.CreateClient();

            try
            {
                // Gửi yêu cầu DELETE đến API
                var response =  client.DeleteAsync($"https://localhost:7296/api/ParentCategories/delete-parent-category/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Nếu thành công, chuyển hướng về trang Index
                    return Json(new { success = true, message = "Cập nhật danh mục thành công" });
                }
                else
                {
                    // Ghi lại thông tin lỗi vào console hoặc log
                    var errorDetails =  response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error Status: {response.StatusCode}");
                    Console.WriteLine($"Error Details: {errorDetails}");

                    return Json(new { success = false, message = $"Có lỗi khi cập nhật danh mục. Mã lỗi: {response.StatusCode}. Chi tiết: {errorDetails}" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception khi gửi yêu cầu DELETE: " + ex.Message);
                    return Json(new { success = false, message = "Không thể kết nối đến API. Vui lòng kiểm tra lại kết nối." });
            }

        }

    }
}
