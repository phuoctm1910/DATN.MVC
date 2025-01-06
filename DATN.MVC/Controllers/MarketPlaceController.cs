using DATN.MVC.Models;
using DATN.MVC.Request.Product;
using DATN.MVC.Respone.MarketPlace;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;

namespace DATN.MVC.Controllers
{
    public class MarketPlaceController : BaseController
    {
        private readonly HttpClient _httpClient;
      
        public MarketPlaceController()
        {
            _httpClient = new HttpClient();
        }


        // Action để lấy tất cả sản phẩm
        public async Task<ActionResult> Index(int page = 1, int pageSize = 10)
        {
            // Khai báo đối tượng chứa danh sách sản phẩm, danh mục và địa điểm bán hàng
            var data = new GetALLProductAndCategory
            {
                Categories = new List<CaterorisRes>(),
                Products = new List<GetAllProductRes>(),
                salePlace = new List<SalePlaceRes>()
            };

            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Gửi yêu cầu GET tới API lấy danh mục
                    HttpResponseMessage categoryResponse = await httpClient.GetAsync("https://localhost:7296/api/ParentCategories/getAll");
                    if (categoryResponse.IsSuccessStatusCode)
                    {
                        var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
                        data.Categories = JsonConvert.DeserializeObject<List<CaterorisRes>>(categoryJson);
                    }
                    else
                    {
                        TempData["Error"] = "Không thể tải danh sách danh mục.";
                    }

                    // Gửi yêu cầu GET tới API lấy sản phẩm
                    HttpResponseMessage productResponse = await httpClient.GetAsync("https://localhost:7296/EmployeProductGetAll");
                    if (productResponse.IsSuccessStatusCode)
                    {
                        var productJson = await productResponse.Content.ReadAsStringAsync();
                        data.Products = JsonConvert.DeserializeObject<List<GetAllProductRes>>(productJson);

                        // Phân trang
                        var totalProducts = data.Products.Count; // Tổng số sản phẩm
                        var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

                        // Lấy danh sách sản phẩm cho trang hiện tại
                        data.Products = data.Products
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

                        // Gán dữ liệu phân trang vào ViewBag
                        ViewBag.Page = page;
                        ViewBag.PageSize = pageSize;
                        ViewBag.TotalPages = totalPages;
                    }
                    else
                    {
                        TempData["Error"] = "Không thể tải danh sách sản phẩm.";
                    }

                    // Gửi yêu cầu GET tới API lấy danh sách địa điểm bán hàng
                    HttpResponseMessage saleplaceResponse = await httpClient.GetAsync("https://localhost:7296/api/ManageSalePlace/GetAll");
                    if (saleplaceResponse.IsSuccessStatusCode)
                    {
                        var saleplacesJson = await saleplaceResponse.Content.ReadAsStringAsync();
                        data.salePlace = JsonConvert.DeserializeObject<List<SalePlaceRes>>(saleplacesJson);
                    }
                    else
                    {
                        TempData["Error"] = "Không thể tải danh sách địa điểm bán hàng.";
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

            // Trả về dữ liệu vào View
            return View(data);
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



        public async Task<IActionResult> SalePlace(int saleplace)
        {
            // Đối tượng chứa thông tin địa điểm bán hàng và sản phẩm
            var data = new ByIdSalePlaceAndGetALLProduct
            {
                saleplace = new SalePlaceRes(),
                Products = new List<GetAllProductRes>()
            };

            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Lấy thông tin SalePlace
                    HttpResponseMessage saleplaceResponse = await httpClient.GetAsync($"https://localhost:7296/api/ManageSalePlace/GetSalePlace/{saleplace}");
                    if (saleplaceResponse.IsSuccessStatusCode)
                    {
                        var salePlaceJson = await saleplaceResponse.Content.ReadAsStringAsync();
                        data.saleplace = JsonConvert.DeserializeObject<SalePlaceRes>(salePlaceJson);
                    }
                    else
                    {
                        TempData["Error"] = "Không thể tải thông tin địa điểm bán hàng.";
                    }

                    // Lấy danh sách tất cả sản phẩm
                    HttpResponseMessage productResponse = await httpClient.GetAsync("https://localhost:7296/EmployeProductGetAll");
                    if (productResponse.IsSuccessStatusCode)
                    {
                        var productJson = await productResponse.Content.ReadAsStringAsync();
                        var allProducts = JsonConvert.DeserializeObject<List<GetAllProductRes>>(productJson);

                        // Lọc sản phẩm theo SalePlaceId
                        data.Products = allProducts
                            .Where(p => p.SalePlaceId == saleplace) // SalePlaceId là thuộc tính trong đối tượng sản phẩm
                            .ToList();

                        // Tính tổng số lượng sản phẩm
                        ViewBag.TotalProducts = data.Products.Count;
                    }
                    else
                    {
                        TempData["Error"] = "Không thể tải danh sách sản phẩm.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Đã xảy ra lỗi: {ex.Message}";
                }
            }

            // Trả về dữ liệu vào View
            return View(data);
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


        [HttpGet]
        public async Task<IActionResult> SortProducts(string sortOrder, string categoryFilter = null)
        {
            var data = new GetALLProductAndCategory
            {
                Categories = new List<CaterorisRes>(),
                Products = new List<GetAllProductRes>(),
                salePlace = new List<SalePlaceRes>()  // Thêm phần này để chứa danh sách gian hàng
            };

            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Lấy danh mục
                    HttpResponseMessage categoryResponse = await httpClient.GetAsync("https://localhost:7296/api/ParentCategories/getAll");
                    if (categoryResponse.IsSuccessStatusCode)
                    {
                        var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
                        data.Categories = JsonConvert.DeserializeObject<List<CaterorisRes>>(categoryJson);
                    }
                    else
                    {
                        TempData["Error"] = "Không thể tải danh sách danh mục.";
                    }

                    // Lấy danh sách sản phẩm
                    HttpResponseMessage productResponse = await httpClient.GetAsync("https://localhost:7296/EmployeProductGetAll");
                    if (productResponse.IsSuccessStatusCode)
                    {
                        var productJson = await productResponse.Content.ReadAsStringAsync();
                        data.Products = JsonConvert.DeserializeObject<List<GetAllProductRes>>(productJson);

                        // Lọc theo category nếu có filter
                        if (!string.IsNullOrEmpty(categoryFilter))
                        {
                            data.Products = data.Products.Where(p => p.CategoryName.Equals(categoryFilter, StringComparison.OrdinalIgnoreCase)).ToList();
                        }

                        // Lọc và sắp xếp sản phẩm theo yêu cầu
                        if (data.Products != null && data.Products.Count > 0)
                        {
                            switch (sortOrder?.ToLower())
                            {
                                case "asc":
                                    data.Products = data.Products.OrderBy(p => p.Price).ToList(); // Tăng dần theo giá
                                    break;
                                case "desc":
                                    data.Products = data.Products.OrderByDescending(p => p.Price).ToList(); // Giảm dần theo giá
                                    break;
                                case "newest":
                                    data.Products = data.Products
                                        .OrderByDescending(p => p.CreatedDateTime) // Sắp xếp theo `DateTime`
                                        .ToList();
                                    break;
                                default:
                                    TempData["Error"] = "Không xác định được kiểu sắp xếp.";
                                    break;
                            }
                        }
                        else
                        {
                            TempData["Error"] = "Không có sản phẩm phù hợp.";
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Không thể tải danh sách sản phẩm.";
                    }

                    // Lấy danh sách gian hàng
                    HttpResponseMessage saleplaceResponse = await httpClient.GetAsync("https://localhost:7296/api/ManageSalePlace/GetAll");
                    if (saleplaceResponse.IsSuccessStatusCode)
                    {
                        var saleplacesJson = await saleplaceResponse.Content.ReadAsStringAsync();
                        data.salePlace = JsonConvert.DeserializeObject<List<SalePlaceRes>>(saleplacesJson);

                        // Nếu cần, bạn có thể thêm lọc và sắp xếp cho salePlace tại đây nếu có
                    }
                    else
                    {
                        TempData["Error"] = "Không thể tải danh sách gian hàng.";
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

            // Trả về dữ liệu cho View
            return View("Index", data);
        }



        [HttpGet]
        public async Task<IActionResult> FilterByPrice(decimal? minPrice, decimal? maxPrice, decimal? minRentalPrice, decimal? maxRentalPrice)
        {
            var data = new GetALLProductAndCategory
            {
                Categories = new List<CaterorisRes>(),
                Products = new List<GetAllProductRes>(),
                salePlace = new List<SalePlaceRes>() // Thêm phần này để chứa gian hàng
            };

            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Lấy danh mục
                    HttpResponseMessage categoryResponse = await httpClient.GetAsync("https://localhost:7296/api/ParentCategories/getAll");
                    if (categoryResponse.IsSuccessStatusCode)
                    {
                        var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
                        data.Categories = JsonConvert.DeserializeObject<List<CaterorisRes>>(categoryJson);
                    }
                    else
                    {
                        TempData["Error"] = "Không thể tải danh sách danh mục.";
                    }

                    // Lấy danh sách sản phẩm
                    HttpResponseMessage productResponse = await httpClient.GetAsync("https://localhost:7296/EmployeProductGetAll");
                    if (productResponse.IsSuccessStatusCode)
                    {
                        var productJson = await productResponse.Content.ReadAsStringAsync();
                        data.Products = JsonConvert.DeserializeObject<List<GetAllProductRes>>(productJson);

                        // Lọc sản phẩm theo giá
                        if (minPrice.HasValue)
                        {
                            data.Products = data.Products.Where(p => p.Price >= minPrice.Value).ToList();
                        }
                        if (maxPrice.HasValue)
                        {
                            data.Products = data.Products.Where(p => p.Price <= maxPrice.Value).ToList();
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Không thể tải danh sách sản phẩm.";
                    }

                    // Lấy danh sách gian hàng
                    HttpResponseMessage saleplaceResponse = await httpClient.GetAsync("https://localhost:7296/api/ManageSalePlace/GetAll");
                    if (saleplaceResponse.IsSuccessStatusCode)
                    {
                        var saleplacesJson = await saleplaceResponse.Content.ReadAsStringAsync();
                        data.salePlace = JsonConvert.DeserializeObject<List<SalePlaceRes>>(saleplacesJson);

                      
                    }
                    else
                    {
                        TempData["Error"] = "Không thể tải danh sách gian hàng.";
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

            // Trả về dữ liệu cho View
            return View("Index", data);
        }



        [HttpGet]
        public async Task<IActionResult> FilterByCategory(int? categoryId)
        {
            var data = new GetALLProductAndCategory
            {
                Categories = new List<CaterorisRes>(),
                Products = new List<GetAllProductRes>(),
                salePlace = new List<SalePlaceRes>() // Thêm SalePlaceRes vào data
            };

            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Lấy danh mục
                    HttpResponseMessage categoryResponse = await httpClient.GetAsync("https://localhost:7296/api/ParentCategories/getAll");
                    if (categoryResponse.IsSuccessStatusCode)
                    {
                        var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
                        data.Categories = JsonConvert.DeserializeObject<List<CaterorisRes>>(categoryJson);
                    }
                    else
                    {
                        TempData["Error"] = "Không thể tải danh sách danh mục.";
                    }

                    // Lấy danh sách sản phẩm
                    HttpResponseMessage productResponse = await httpClient.GetAsync("https://localhost:7296/EmployeProductGetAll");
                    if (productResponse.IsSuccessStatusCode)
                    {
                        var productJson = await productResponse.Content.ReadAsStringAsync();
                        data.Products = JsonConvert.DeserializeObject<List<GetAllProductRes>>(productJson);

                        // Lọc sản phẩm theo danh mục
                        if (categoryId.HasValue)
                        {
                            data.Products = data.Products.Where(p => p.CategoryId == categoryId.Value).ToList();
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Không thể tải danh sách sản phẩm.";
                    }

                    // Lấy danh sách gian hàng
                    HttpResponseMessage saleplaceResponse = await httpClient.GetAsync("https://localhost:7296/api/ManageSalePlace/GetAll");
                    if (saleplaceResponse.IsSuccessStatusCode)
                    {
                        var saleplacesJson = await saleplaceResponse.Content.ReadAsStringAsync();
                        data.salePlace = JsonConvert.DeserializeObject<List<SalePlaceRes>>(saleplacesJson);

                    
                    }
                    else
                    {
                        TempData["Error"] = "Không thể tải danh sách gian hàng.";
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

            // Trả về dữ liệu cho View
            return View("Index", data);
        }

        [HttpGet]
        public async Task<IActionResult> SearchByName(string searchTerm, int page = 1, int pageSize = 10)
        {
            var data = new GetALLProductAndCategory
            {
                Categories = new List<CaterorisRes>(),
                Products = new List<GetAllProductRes>(),
                salePlace = new List<SalePlaceRes>()  // Thêm SalePlaceRes vào
            };

            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Lấy danh mục
                    HttpResponseMessage categoryResponse = await httpClient.GetAsync("https://localhost:7296/api/ParentCategories/getAll");
                    if (categoryResponse.IsSuccessStatusCode)
                    {
                        var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
                        data.Categories = JsonConvert.DeserializeObject<List<CaterorisRes>>(categoryJson);
                    }

                    // Lấy danh sách sản phẩm
                    HttpResponseMessage productResponse = await httpClient.GetAsync("https://localhost:7296/EmployeProductGetAll");
                    if (productResponse.IsSuccessStatusCode)
                    {
                        var productJson = await productResponse.Content.ReadAsStringAsync();
                        data.Products = JsonConvert.DeserializeObject<List<GetAllProductRes>>(productJson);

                        // Tìm kiếm sản phẩm theo tên nếu có searchTerm
                        if (!string.IsNullOrEmpty(searchTerm))
                        {
                            data.Products = data.Products
                                .Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                                .ToList();
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Không thể tải danh sách sản phẩm.";
                    }

                    // Lấy danh sách gian hàng
                    HttpResponseMessage saleplaceResponse = await httpClient.GetAsync("https://localhost:7296/api/ManageSalePlace/GetAll");
                    if (saleplaceResponse.IsSuccessStatusCode)
                    {
                        var saleplacesJson = await saleplaceResponse.Content.ReadAsStringAsync();
                        data.salePlace = JsonConvert.DeserializeObject<List<SalePlaceRes>>(saleplacesJson);

                 
                       
                    }
                    else
                    {
                        TempData["Error"] = "Không thể tải danh sách gian hàng.";
                    }

                    // Phân trang cho sản phẩm
                    var totalProducts = data.Products.Count;
                    var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);  // Số trang
                    ViewBag.Page = page;
                    ViewBag.PageSize = pageSize;
                    ViewBag.TotalPages = totalPages;

                    // Phân trang: Lấy 10 sản phẩm mỗi trang
                    var pagedProducts = data.Products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    data.Products = pagedProducts;

                    // Phân trang cho gian hàng (SalePlace)
                    var totalSalePlaces = data.salePlace.Count;
                    var totalSalePlacePages = (int)Math.Ceiling((double)totalSalePlaces / pageSize);
                    Console.WriteLine($"Total SalePlace Pages: {totalSalePlacePages}");  // In ra tổng số trang
                    ViewBag.TotalSalePlacePages = totalSalePlacePages;

                    // Phân trang: Lấy gian hàng mỗi trang
                    var pagedSalePlaces = data.salePlace.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    data.salePlace = pagedSalePlaces;

                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Đã xảy ra lỗi: {ex.Message}";
                }
            }

            return View("Index", data); // Trả dữ liệu về View
        }








    }
}
