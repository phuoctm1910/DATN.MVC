using DATN.MVC.Areas.Admin.Models.AdminCategory.Request;
using DATN.MVC.Areas.Admin.Models.AdminCategory.Response;
using DATN.MVC.Areas.Admin.Models.AdminPost.Request;
using DATN.MVC.Areas.Admin.Models.AdminPost.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DATN.MVC.Areas.Admin.Controllers.AdminPost
{
    [Area("Admin")]
    public class AdminPostController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminPostController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

       
        public async Task<IActionResult> Index()
        {
            List<ViewPostResponse_Res> post = new List<ViewPostResponse_Res>();

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://localhost:7296/api/Posts/getAll");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();

                    // Deserialize vào ApiResponse thay vì List<ParentCategoriesReadAll_Res> trực tiếp
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<ViewPostResponse_Res>>(jsonData);
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
            var response = await client.GetAsync($"https://localhost:7296/api/Posts/getById/{Id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ViewPostResponse_Res>(jsonData);

                var model = new EditPostStatus_Req
                {
                    PostId = apiResponse.Id, // Đảm bảo 'Id' không null
                    IsActived = apiResponse.IsActived
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
        public async Task<IActionResult> UpdateStatusPost(EditPostStatus_Req model )
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient();
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                try
                {
                    var response = await client.PutAsync($"https://localhost:7296/api/Posts/edit-status/{model.PostId}", content); // Đảm bảo URL đúng

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Cập nhật trạng thái bài đăng thành công!";
                        return RedirectToAction("Index", "AdminPost");
                    }
                    else
                    {
                        var errorDetails = await response.Content.ReadAsStringAsync();
                        TempData["ErrorMessage"] = $"Có lỗi khi cập nhật bài viết. Mã lỗi: {response.StatusCode}. Chi tiết: {errorDetails}";
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
        public async Task<IActionResult> deletePost(int id)
        {
            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.DeleteAsync($"https://localhost:7296/api/Posts/delete/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Bài đăng đã được xóa thành công!" });
                }
                else
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = $"Không thể xóa bài đăng. Chi tiết: {errorDetails}" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }



    }
}
