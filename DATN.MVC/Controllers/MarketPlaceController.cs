using DATN.MVC.Models;
using DATN.MVC.Request.MarketPlaces;
using DATN.MVC.Request.Product;
using DATN.MVC.Respone.MarketPlace;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
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
        public async Task<IActionResult> DisplayProduct()
        {
            // Lấy danh sách Category từ API hoặc từ cơ sở dữ liệu
            var categories = await GetCategoriesFromApi();
            ViewBag.Categories = categories;

            return View();
        }

        // Hàm giả định lấy dữ liệu từ API
        private async Task<List<CaterorisRes>> GetCategoriesFromApi()
        {
            var response = await _httpClient.GetAsync("https://localhost:7296/api/ParentCategories/getAll");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<CaterorisRes>>(json);
            }
            return new List<CaterorisRes>();
        }

        [HttpGet("{saleplace:int}")]
        public async Task<IActionResult> SalePlace(int saleplace, decimal? minPrice, decimal? maxPrice, string sortOrder)
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
                        data.Products = allProducts.Where(p => p.SalePlaceId == saleplace).ToList();

                        // Lọc theo mức giá nếu có
                        if (minPrice.HasValue)
                        {
                            data.Products = data.Products.Where(p => p.Price >= minPrice.Value).ToList();
                        }
                        if (maxPrice.HasValue)
                        {
                            data.Products = data.Products.Where(p => p.Price <= maxPrice.Value).ToList();
                        }

                        // Sắp xếp sản phẩm theo yêu cầu
                        switch (sortOrder?.ToLower())
                        {
                            case "newest":
                                data.Products = data.Products.OrderByDescending(p => p.CreatedDateTime).ToList();
                                break;
                            case "asc":
                                data.Products = data.Products.OrderBy(p => p.Price).ToList();
                                break;
                            case "desc":
                                data.Products = data.Products.OrderByDescending(p => p.Price).ToList();
                                break;
                        }

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


        // create saleplace
        [HttpPost]
        public async Task<IActionResult> DisplayProduct(DisplayProduct product)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage)
                                               .ToList();
                TempData["ErrorList"] = errors;
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ. Vui lòng kiểm tra chi tiết lỗi.";
                ViewBag.Categories = await GetCategoriesFromApi();
                return View(product);
            }

            try
            {
                // Xử lý file ảnh
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                        Directory.CreateDirectory(uploadPath);

                        var filePath = Path.Combine(uploadPath, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        product.Image = $"{fileName}";
                        //product.Image = $"/uploads/{fileName}";
                    }
                }
                else
                {
                    product.Image = null; // Nếu không tải lên ảnh, để null
                }

                // Gửi dữ liệu đến API
                var client = new HttpClient();
                var apiUrl = "https://localhost:7296/api/Product/add";
                var jsonContent = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Sản phẩm đã được thêm thành công!";
                    return RedirectToAction("DisplayProduct");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Lỗi từ API: {error}";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
            }

            ViewBag.Categories = await GetCategoriesFromApi();
            return View(product);
        }



        [HttpGet]
        public async Task<IActionResult> CreateSalePlace()
        {
                        
            var viewSettings = new ViewSettings
            {
                ShowSidebar = false,
                ShowHeader = false,
                ShowFriendList = false
            };
            ViewBag.ViewSettings = viewSettings;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateSalePlace(CreateSalceplaces model)
        {
            // Kiểm tra tính hợp lệ của ModelState
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại!";
                return View(model);
            }

            var formData = new MultipartFormDataContent();
            var userId = HttpContext.Items["userId"]?.ToString();

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin người dùng!";
                return View(model);
            }

            formData.Add(new StringContent(model.Name), "Name");
            formData.Add(new StringContent(userId), "UserId");

            if (model.ImageFile != null && model.ImageFile.Any())
            {
                foreach (var file in model.ImageFile)
                {
                    var fileContent = new StreamContent(file.OpenReadStream());
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                    formData.Add(fileContent, "ImageFile", file.FileName);
                }
            }

            try
            {
                // Gửi request đến API tạo gian hàng
                var response = await _httpClient.PostAsync("https://localhost:7296/api/ManageSalePlace/CreateSalePlace", formData);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Gian hàng đã được tạo thành công!";
                    return RedirectToAction("SalePlace", "MarketPlace"); // Chuyển hướng sau khi thành công
                }
                else
                {
                    TempData["ErrorMessage"] = "Bạn chỉ được tạo duy nhất 1 gian hàng, không thể tạo thêm.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                // Bắt lỗi và hiển thị thông báo
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi khi tạo gian hàng: {ex.Message}";
                return View(model);
            }
        }
       




        [HttpGet]
        public IActionResult SalePlaceRedirect(int saleplace)
        {
            // Chuyển hướng tới action SalePlace với route parameter
            return RedirectToAction("SalePlace", new { saleplace });
        }







    }
}