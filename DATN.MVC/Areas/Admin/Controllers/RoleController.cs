using DATN.MVC.Request.Role;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using DATN.MVC.Respone.Role;
using DATN.MVC.Request.User;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace DATN.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly IConfiguration _configuration;

        public RoleController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("https://localhost:7296/api/Role/get-roles");
                    if (response.IsSuccessStatusCode)
                    {
                        var roles = await System.Text.Json.JsonSerializer.DeserializeAsync<List<RoleResponse>>(await response.Content.ReadAsStreamAsync());
                        return View(roles);
                    }
                    else
                    {
                        TempData["Error"] = "An error occurred while retrieving roles from the API.";
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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNew([FromBody] AddRoleRequest request)
        {

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsJsonAsync("https://localhost:7296/api/Role/add-role", request);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Role added successfully.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = "Failed to add role using the API.";
                       
                        return View(request);
                    }
                }
            }
            catch (HttpRequestException httpEx)
            {
                TempData["Error"] = "An error occurred while making a request to the API: " + httpEx.Message;
             
                return View(request);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An unexpected error occurred: " + ex.Message;
              
                return View(request);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync($"https://localhost:7296/api/Role/get-role/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var role = await System.Text.Json.JsonSerializer.DeserializeAsync<UpdateRoleRequest>(await response.Content.ReadAsStreamAsync());
                        
                        return View(role);
                    }
                    else
                    {
                        TempData["Error"] = "An error occurred while retrieving the role details.";
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
        public async Task<IActionResult> Edit(int id, UpdateRoleRequest request)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PutAsJsonAsync($"https://localhost:7296/api/Role/update-role/{id}", request);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Role updated successfully.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = "Failed to update role using the API.";
                        return View(request);
                    }
                }
            }
            catch (HttpRequestException httpEx)
            {
                TempData["Error"] = "An error occurred while making a request to the API: " + httpEx.Message;
                return View(request);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An unexpected error occurred: " + ex.Message;
                return View(request);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.DeleteAsync($"https://localhost:7296/api/Role/delete-role/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Role deleted successfully.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = "Failed to delete Role using the API.";
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
}
