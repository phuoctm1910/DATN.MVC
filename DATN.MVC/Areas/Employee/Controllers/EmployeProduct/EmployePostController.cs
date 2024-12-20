using DATN.MVC.Areas.Admin.Models.AdminCategory.Response;
using DATN.MVC.Areas.Employee.Models.EmployePost.Request;
using DATN.MVC.Areas.Employee.Models.EmployePost.Respone;
using DATN.MVC.Areas.Employee.Models.EmployeProduct.Request;
using DATN.MVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DATN.MVC.Areas.Employee.Controllers.EmployeProduct
{
    [Area("Employee")]

    public class EmployePostController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EmployePostController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }



        public async Task<IActionResult> Index()
        {
            List<EmployeViewPostRespone_Res> Post = new List<EmployeViewPostRespone_Res>();

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://localhost:7296/EmpoyePostGetAll");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();

                    // Deserialize vào ApiResponse thay vì List<ParentCategoriesReadAll_Res> trực tiếp
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<EmployeViewPostRespone_Res>>(jsonData);
                    if (apiResponse != null && apiResponse.Data != null)
                    {
                        Post = apiResponse.Data;
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

            return View(Post);
        }


        // GET: Lấy thông tin bài đăng theo ID
        [HttpGet]
        public async Task<IActionResult> GetPostById(int postId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"https://localhost:7296/EmployePostGetById?postId={postId}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<EmployeViewPostRespone_Res>>(jsonData);

                    if (apiResponse != null && apiResponse.Data != null)
                    {
                        return Json(new { success = true, data = apiResponse.Data });
                    }
                }
                return Json(new { success = false, message = "Không tìm thấy bài đăng hoặc API trả về lỗi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi khi gọi API: " + ex.Message });
            }
        }




        [HttpPost]
        public JsonResult UpdatePostState([FromBody] EmployePostStatus_Req request)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient();
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Chèn ID vào URL
                var apiUrl = $"https://localhost:7296/Employe-edit-status/{request.PostId}";

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
