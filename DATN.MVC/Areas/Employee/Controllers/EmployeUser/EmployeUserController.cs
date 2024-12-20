using DATN.MVC.Controllers;
using DATN.MVC.Respone.User;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DATN.MVC.Areas.Employee.Controllers.EmployeUser
{
    [Area("Employee")]
    public class EmployeUserController : BaseController
    {
        private readonly IConfiguration _configuration;

        public EmployeUserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // Gọi API
                    var response = await client.GetAsync("https://localhost:7296/api/EmployeeUser/get-employee-users");
                    if (response.IsSuccessStatusCode)
                    {
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        // Deserialize danh sách người dùng
                        var users = await response.Content.ReadFromJsonAsync<List<EmployeeUserResponse>>(options);

                        // Trả danh sách người dùng vào View
                        return View(users);
                    }
                    else
                    {
                        // Xử lý lỗi nếu API trả về lỗi
                        TempData["Error"] = $"API returned an error: {response.ReasonPhrase}";
                        return View(new List<EmployeeUserResponse>());
                    }
                }
            }
            catch (HttpRequestException httpEx)
            {
                TempData["Error"] = "An error occurred while making a request to the API: " + httpEx.Message;
                return View("Error");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An unexpected error occurred: " + ex.Message;
                return View("Error");
            }
        }

    }
}
