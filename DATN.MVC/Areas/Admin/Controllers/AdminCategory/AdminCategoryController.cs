using DATN.MVC.Areas.Admin.Models.AdminCategory.Request;
using DATN.MVC.Areas.Admin.Models.AdminCategory.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DATN.MVC.Areas.Admin.Controllers.AdminCategory
{
    [Area("Admin")]
    public class AdminCategoryController : Controller
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
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<ParentCategoriesReadAll_Res>>(jsonData);
                    if (apiResponse != null && apiResponse.Data != null)
                    {
                        parentCategories = apiResponse.Data;
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
        public IActionResult CreateParentCategory()
        {
            return View();
        }


        // POST: AdminCategory/CreateParentCategory
        [HttpPost]
        public async Task<IActionResult> CreateParentCategory(ParentCategories_Req model)
        {
            if (ModelState.IsValid)
            {
                // Tạo client HTTP để gửi yêu cầu đến API
                var client = _httpClientFactory.CreateClient();
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Gửi yêu cầu POST tới API để tạo danh mục cha
                var response = await client.PostAsync("https://localhost:7296/api/ParentCategories/create-parent-category", content);

                if (response.IsSuccessStatusCode)
                {
                    // Nếu thành công, chuyển hướng về trang danh sách danh mục hoặc trang khác
                    return RedirectToAction("Edit", "HomeAdmin");
                }
                else
                {
                    // Nếu có lỗi khi tạo danh mục, bạn có thể thêm thông báo lỗi cho người dùng
                    ModelState.AddModelError("", "Có lỗi khi tạo danh mục. Vui lòng thử lại.");
                }
            }

            // Nếu Model không hợp lệ hoặc có lỗi, hiển thị lại form tạo danh mục
            return View(model);
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
        public async Task<IActionResult> UpdateParentCategory(int id, ParentCategories_Req model)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient();
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                try
                {
                    var response = await client.PutAsync($"https://localhost:7296/api/ParentCategories/update-parent-category/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Chuyển hướng về trang Index nếu thành công
                        return RedirectToAction("Index", "AdminCategory");
                    }
                    else
                    {
                        // Ghi lại thông tin lỗi vào console hoặc log
                        var errorDetails = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error Status: {response.StatusCode}");
                        Console.WriteLine($"Error Details: {errorDetails}");

                        ModelState.AddModelError("", $"Có lỗi khi cập nhật danh mục. Mã lỗi: {response.StatusCode}. Chi tiết: {errorDetails}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception khi gửi yêu cầu PUT: " + ex.Message);
                    ModelState.AddModelError("", "Không thể kết nối đến API. Vui lòng kiểm tra lại kết nối.");
                }
            }

            return View("Edit", model);
        }



        [HttpPost]
        public async Task<IActionResult> DeleteParentCategory(int id)
        {
            var client = _httpClientFactory.CreateClient();

            try
            {
                // Gửi yêu cầu DELETE đến API
                var response = await client.DeleteAsync($"https://localhost:7296/api/ParentCategories/delete-parent-category/{id}");

                if (response.IsSuccessStatusCode)
                {
                    // Nếu thành công, chuyển hướng về trang Index
                    return RedirectToAction("Index");
                }
                else
                {
                    // Ghi lại thông tin lỗi vào console hoặc log
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error Status: {response.StatusCode}");
                    Console.WriteLine($"Error Details: {errorDetails}");

                    TempData["ErrorMessage"] = $"Có lỗi khi xóa danh mục. Mã lỗi: {response.StatusCode}. Chi tiết: {errorDetails}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception khi gửi yêu cầu DELETE: " + ex.Message);
                TempData["ErrorMessage"] = "Không thể kết nối đến API. Vui lòng kiểm tra lại kết nối.";
            }

            // Nếu có lỗi, quay lại Index và hiển thị thông báo lỗi
            return RedirectToAction("Index");
        }

    }
}
