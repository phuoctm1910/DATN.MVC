using DATN.MVC.Areas.Admin.Models;
using DATN.MVC.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DATN.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly string _url = "https://localhost:7296/api/Admin";
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // 1. List all admin
        [HttpGet]   
      public async Task<IActionResult> Index()
{
    List<UserRegistration> categories = new List<UserRegistration>();
    using (var client = _httpClientFactory.CreateClient())
    {
        var response = await client.GetAsync($"{_url}/getByClassId");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(jsonData))
            {
                ViewData["ErrorMessage"] = "No data returned from the server.";
            }
            else
            {
                categories = JsonConvert.DeserializeObject<List<UserRegistration>>(jsonData);
                if (categories.Count == 0)
                {
                    ViewData["ErrorMessage"] = "No users found.";
                }
            }
        }
        else
        {
            ViewData["ErrorMessage"] = "Failed to retrieve data.";
        }
    }
    return View(categories);
}


        //// 2. Get details of a category by ID
        //[HttpGet("Details/{id}")]
        //public async Task<IActionResult> Details(int id)
        //{
        //    UserRegistration category = null;
        //    using (var client = _httpClientFactory.CreateClient())
        //    {
        //        var response = await client.GetAsync($"{_url}/getById/{id}");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var jsonResponse = await response.Content.ReadAsStringAsync();
        //            category = JsonConvert.DeserializeObject<UserRegistration>(jsonResponse);
        //        }
        //        else
        //        {
        //            ViewData["ErrorMessage"] = "Failed to retrieve category details.";
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    return View(category);
        //}

         //3. Delete a category - GET confirmation page
        //[HttpGet("delete/{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    UserRegistration category = null;
        //    using (var client = _httpClientFactory.CreateClient())
        //    {
        //        var response = await client.GetAsync($"{_url}/getById/{id}");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var jsonData = await response.Content.ReadAsStringAsync();
        //            category = JsonConvert.DeserializeObject<UserRegistration>(jsonData);
        //        }
        //    }
        //    return View(category);
        //}

        //// 4. Delete a category - POST to confirm deletion
        //[HttpPost, ActionName("delete")]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    using (var client = _httpClientFactory.CreateClient())
        //    {
        //        var response = await client.DeleteAsync($"{_url}/delete/{id}");
        //        if (!response.IsSuccessStatusCode)
        //        {
        //            TempData["ErrorMessage"] = "Failed to delete category.";
        //        }
        //    }
        //    return RedirectToAction("Index");
        //}

        // 5. Update category - GET
        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            try
            {
                var user = ApiHelpers.GetMethod<UserRegistration>($"{_url}/getByClassId/{id}");
                if (user == null)
                {
                    return NotFound("User not found.");
                }
                return View(user);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error retrieving user: {ex.Message}");
                return View();
            }
        }


        // 6. Update category - POST
        [HttpPost("Edit/{id}")]
        public IActionResult Edit(int id, UserRegistration model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Fetch existing user để giữ CreatedDate
                var existingUser = ApiHelpers.GetMethod<UserRegistration>($"{_url}/getByClassId/{id}");
                if (existingUser == null)
                {
                    ModelState.AddModelError("", "User not found.");
                    return View(model);
                }

                // Preserve CreatedDate và cập nhật UpdatedDate
                //model.CreatedDate = existingUser.CreatedDate;
                //model.UpdatedDate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                // Update user qua API
                var result = ApiHelpers.PutMethod<UserRegistration>($"{_url}/update", model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating user: {ex.Message}");
                return View(model);
            }
        }

    }
}
