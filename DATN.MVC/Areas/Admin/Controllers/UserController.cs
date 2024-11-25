using DATN.MVC.Request.User;
using DATN.MVC.Respone.User;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;

namespace DATN.MVC.Areas.Admin.Controllers;

[Area("Admin")]
public class UserController : Controller
{
    private readonly IConfiguration _configuration;

    public UserController(IConfiguration configuration)
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
                var response = await client.GetAsync("https://localhost:7296/api/UserAdmin/get-users");
                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    // Deserialize danh sách người dùng
                    var users = await response.Content.ReadFromJsonAsync<List<UserResponse>>(options);

                    // Trả danh sách người dùng vào View
                    return View(users);
                }
                else
                {
                    // Xử lý lỗi nếu API trả về lỗi
                    TempData["Error"] = $"API returned an error: {response.ReasonPhrase}";
                    return View(new List<UserResponse>());
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

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AddUpdateUserRequest request)
    {
       
        try
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync("https://localhost:7296/api/UserAdmin/add-user", request);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "User added successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Failed to add user using the API.";
                    return RedirectToAction("Create");
                }
            }
        }
        catch (HttpRequestException httpEx)
        {
            TempData["Error"] = "An error occurred while making a request to the API: " + httpEx.Message;
            return RedirectToAction("Create");
        }
        catch (Exception ex)
        {
            TempData["Error"] = "An unexpected error occurred: " + ex.Message;
            return RedirectToAction("Create");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"https://localhost:7296/api/UserAdmin/get-user/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var user = await JsonSerializer.DeserializeAsync<AddUpdateUserRequest>(await response.Content.ReadAsStreamAsync(), options);
                    return View(user);
                }
                else
                {
                    TempData["Error"] = "An error occurred while retrieving the user details.";
                    return View("Error");
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

    [HttpPost]
    public async Task<IActionResult> Edit(int id, AddUpdateUserRequest model)
    {
        
        using (HttpClient client = new HttpClient())
        {
            var userJson = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            var content = new StringContent(userJson, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync($"https://localhost:7296/UserAdmin/update-user/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "User information updated successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update user information.";
            }
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"https://localhost:7296/api/UserAdmin/get-user/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var user = await JsonSerializer.DeserializeAsync<UserResponse>(await response.Content.ReadAsStreamAsync());
                    return View(user);
                }
                else
                {
                    TempData["Error"] = "An error occurred while retrieving the user details.";
                    return View("Error");
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



    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync($"https://localhost:7296/api/UserAdmin/delete-user/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "User deleted successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Failed to delete user using the API.";
                    return RedirectToAction("Index");
                }
            }
        }
        catch (HttpRequestException httpEx)
        {
            TempData["Error"] = "An error occurred while making a request to the API: " + httpEx.Message;
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Error"] = "An unexpected error occurred: " + ex.Message;
            return RedirectToAction("Index");
        }
    }
}
