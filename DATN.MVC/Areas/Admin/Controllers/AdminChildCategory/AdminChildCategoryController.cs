using Microsoft.AspNetCore.Mvc;
using DATN.MVC.Areas.Admin.Models.AdminCategory.Response;
using Newtonsoft.Json;
using DATN.MVC.Areas.Admin.Models.AdminChildCategory.Response;
using DATN.MVC.Areas.Admin.Models.AdminCategory.Request;
using System.Text;
using DATN.MVC.Areas.Admin.Models.AdminChildCategory.Request;
using DATN.MVC.Areas.Admin.Models.AdminChildCategory.ViewModels;
using DATN.MVC.Controllers;


namespace DATN.MVC.Areas.Admin.Controllers.AdminChildCategory
{
    [Area("Admin")]
    public class AdminChildCategoryController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminChildCategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }



        public async Task<IActionResult> Index()
        {
            List<CategoriesChild_Res> parentCategories = new List<CategoriesChild_Res>();

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://localhost:7296/GetAllParentCategories");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();

                    // Deserialize vào ApiResponse thay vì List<ParentCategoriesReadAll_Res> trực tiếp
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<CategoriesChild_Res>>(jsonData);
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




        public async Task<IActionResult> CreateChildCategory()
        {
            List<ParentCategoryRespone> parentCategories = new List<ParentCategoryRespone>();

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://localhost:7296/GetAllParentCategories");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    parentCategories = JsonConvert.DeserializeObject<List<ParentCategoryRespone>>(jsonData);
                }
                else
                {
                    ViewData["ErrorMessage"] = "Không thể tải danh mục cha. Vui lòng thử lại.";
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Đã xảy ra lỗi khi gọi API: {ex.Message}";
            }

            // Truyền danh sách danh mục cha vào View
            ViewBag.ParentCategories = parentCategories;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateChild(CategoriesChild_Req model)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient();
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:7296/CreateChildCategory", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "AdminChildCategory");
                }
                else
                {
                    ModelState.AddModelError("", "Có lỗi khi tạo danh mục. Vui lòng thử lại.");
                }
            }

            // Nếu lỗi, tải lại danh mục cha để hiển thị trong dropdown
            await CreateChildCategory();
            return View(model);
        }




        public async Task<IActionResult> Edit(int id)
        {
            CategoriesChild_Res childCategory = null;
            List<ParentCategoryRespone> parentCategories = new List<ParentCategoryRespone>();

            try
            {
                var client = _httpClientFactory.CreateClient();

                // Gọi API lấy danh mục con
                var childResponse = await client.GetAsync($"https://localhost:7296/GetChildCategoryById/{id}");
                if (childResponse.IsSuccessStatusCode)
                {
                    var childData = await childResponse.Content.ReadAsStringAsync();
                    childCategory = JsonConvert.DeserializeObject<CategoriesChild_Res>(childData);
                }

                // Gọi API lấy danh mục cha
                var parentResponse = await client.GetAsync("https://localhost:7296/GetAllParentCategories");
                if (parentResponse.IsSuccessStatusCode)
                {
                    var parentData = await parentResponse.Content.ReadAsStringAsync();
                    parentCategories = JsonConvert.DeserializeObject<List<ParentCategoryRespone>>(parentData);
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Đã xảy ra lỗi khi gọi API: {ex.Message}";
            }

            if (childCategory == null)
            {
                return RedirectToAction("Index");
            }

            // Truyền dữ liệu vào ViewModel
            var viewModel = new EditChildCategoryViewModel
            {
                ChildCategory = childCategory,
                ParentCategories = parentCategories
            };

            return View(viewModel);
        }






        [HttpPost]
        public async Task<IActionResult> UpdateChildCategory(int id, CategoriesChild_Req model)
        {
            CategoriesChild_Res childCategoryRes = null; // Đảm bảo biến được khai báo ngoài try

            if (ModelState.IsValid)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();

                    // Chuyển từ CategoriesChild_Req sang CategoriesChild_Res (nếu cần thiết)
                    childCategoryRes = new CategoriesChild_Res
                    {
                        Name = model.CategoryName,
                        ParentCategoryId = model.ParentCategoryId
                    };

                    // Chuẩn bị nội dung JSON từ model (là CategoriesChild_Req)
                    var jsonContent = JsonConvert.SerializeObject(model);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // Gửi request tới API để cập nhật danh mục
                    var response = await client.PutAsync($"https://localhost:7296/UpdateChildCategory/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Cập nhật thành công, chuyển về trang danh sách
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Có lỗi khi cập nhật danh mục. Vui lòng thử lại.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Đã xảy ra lỗi khi gọi API: {ex.Message}");
                }
            }

            // Lấy lại danh mục cha để hiển thị dropdown khi có lỗi
            var parentCategories = new List<ParentCategoryRespone>();
            try
            {
                var clientRetry = _httpClientFactory.CreateClient();
                var parentResponse = await clientRetry.GetAsync("https://localhost:7296/GetAllParentCategories");
                if (parentResponse.IsSuccessStatusCode)
                {
                    var parentData = await parentResponse.Content.ReadAsStringAsync();
                    parentCategories = JsonConvert.DeserializeObject<List<ParentCategoryRespone>>(parentData);
                }
            }
            catch
            {
                ViewData["ErrorMessage"] = "Không thể tải danh mục cha.";
            }

            // Truyền biến childCategoryRes vào viewModel
            var viewModel = new EditChildCategoryViewModel
            {
                ChildCategory = childCategoryRes, // Truyền đối tượng CategoriesChild_Res vào viewModel
                ParentCategories = parentCategories
            };

            return View(viewModel);
        }




    }
}
