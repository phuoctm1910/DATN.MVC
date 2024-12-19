using DATN.MVC.Models;
using DATN.MVC.Respone.MarketPlace;
using DATN.MVC.Ultilities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using DATN.MVC.Request.Product;

namespace DATN.MVC.Controllers
{
    public class MarketPlaceController : BaseController
    {
        private readonly HttpClient _httpClient;
      
        public MarketPlaceController()
        {
            _httpClient = new HttpClient();
        }

        public class SaleManProductViewModel
        {
            public List<SaleManRes>? SaleMans { get; set; }
            public List<GetAllProductRes>? Products { get; set; }
        }
        // Action để lấy tất cả sản phẩm
        public async Task<ActionResult> Index()
        {
            // Khai báo đối tượng danh sách sản phẩm
            List<GetAllProductRes> products = new List<GetAllProductRes>();

            // Gửi yêu cầu GET tới API để lấy danh sách sản phẩm
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Gửi GET request tới API
                    HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7296/EmployeProductGetAll");

                    if (response.IsSuccessStatusCode)
                    {
                        // Đọc dữ liệu JSON từ API
                        var jsonData = await response.Content.ReadAsStringAsync();

                        // Deserialize thành danh sách sản phẩm
                        products = JsonConvert.DeserializeObject<List<GetAllProductRes>>(jsonData);

                        // Kiểm tra nếu dữ liệu trả về rỗng
                        if (products == null || products.Count == 0)
                        {
                            TempData["Error"] = "Không có sản phẩm nào.";
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Không thể tải danh sách sản phẩm. Vui lòng thử lại sau.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Đã xảy ra lỗi: {ex.Message}";
                }
            }

            // Cấu hình hiển thị cho View
            var viewSettings = new ViewSettings
            {
                ShowSidebar = false, // Tắt sidebar
                ShowHeader = true,   // Bật header
                ShowFriendList = false // Tắt danh sách bạn bè
            };
            ViewBag.ViewSettings = viewSettings;

            // Trả về danh sách sản phẩm vào View
            return View(products);
        }



        [HttpGet]
        public IActionResult DisplayProduct()
        {

            var viewSettings = new ViewSettings
            {
                ShowSidebar = false, // Tắt sidebar
                ShowHeader = true,   // Bật header
                ShowFriendList = false // Tắt danh sách bạn bè
            };
            ViewBag.ViewSettings = viewSettings;
            return View(); ;
        }

        [HttpPost]
        public async Task<IActionResult> DisplayProduct(DisplayProduct model)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = "https://localhost:7296/api/Product/add";
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Product created successfully!";
                    return RedirectToAction("DisplayProduct");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to create product. Please try again.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid data. Please check the form.";
            }

            // Nếu lỗi, trả về model hiện tại để tránh mất dữ liệu đã nhập
            return View(model);
        }



        public async Task<IActionResult> SaleMans()
        {
            List<GetAllProductRes> products = new List<GetAllProductRes>();

            // Gửi yêu cầu GET tới API để lấy danh sách sản phẩm
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Gửi GET request tới API
                    HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7296/EmployeProductGetAll");

                    if (response.IsSuccessStatusCode)
                    {
                        // Đọc dữ liệu JSON từ API
                        var jsonData = await response.Content.ReadAsStringAsync();

                        // Deserialize thành danh sách sản phẩm
                        products = JsonConvert.DeserializeObject<List<GetAllProductRes>>(jsonData);

                        // Kiểm tra nếu dữ liệu trả về rỗng
                        if (products == null || products.Count == 0)
                        {
                            TempData["Error"] = "Không có sản phẩm nào.";
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Không thể tải danh sách sản phẩm. Vui lòng thử lại sau.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Đã xảy ra lỗi: {ex.Message}";
                }
            }

            // Cấu hình hiển thị cho View
            var viewSettings = new ViewSettings
            {
                ShowSidebar = false, // Tắt sidebar
                ShowHeader = true,   // Bật header
                ShowFriendList = false // Tắt danh sách bạn bè
            };
            ViewBag.ViewSettings = viewSettings;

            // Trả về danh sách sản phẩm vào View
            return View(products);
        }



        public IActionResult DetailProduct()
        {
            var viewSettings = new ViewSettings
            {
                ShowSidebar = false, // Tắt sidebar
                ShowHeader = true,   // Bật header
                ShowFriendList = false // Tắt danh sách bạn bè
            };
            ViewBag.ViewSettings = viewSettings;
            return View();
        }

    }
}
