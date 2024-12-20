using DATN.MVC.Models;
using DATN.MVC.Request.Friends;
using DATN.MVC.Request.User;
using DATN.MVC.Respone.Friends;
using DATN.MVC.Respone.MarketPlace;
using DATN.MVC.Respone.User;
using DATN.MVC.Ultilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DATN.MVC.Controllers
{
    public class UserController : BaseController
    {
        // GET: UserController
        public ActionResult About()
        {
            var viewSettings = new ViewSettings
            {
                ShowSidebar = true, // Tắt sidebar
                ShowHeader = true,   // Bật header
                ShowFriendList = true // Tắt danh sách bạn bè
            };
            ViewBag.ViewSettings = viewSettings;

            return View();
        }

        public ActionResult Notification()
        {
            var viewSettings = new ViewSettings
            {
                ShowSidebar = true, // Tắt sidebar
                ShowHeader = true,   // Bật header
                ShowFriendList = true // Tắt danh sách bạn bè
            };
            ViewBag.ViewSettings = viewSettings;

            return View();
        }

        public IActionResult SuggestFriends()
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


        public IActionResult UserProfile() 
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

        public IActionResult RequestFriends()
        {
            using (var httpClient = new HttpClient())
            {
                // Extract UserId from HttpContext
                var userId = HttpContext.Items["userId"]?.ToString();
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User ID is missing.");
                }

                // Prepare the request
                var req = new FriendListReq
                {
                    UserId = int.Parse(userId), // Convert UserId to int
                    Status = FriendStatus.Pending
                };
                // Serialize the request to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");

                HttpResponseMessage response = httpClient.PostAsync("https://localhost:7296/api/friends/get-friend-list", jsonContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    var users = JsonConvert.DeserializeObject<List<FriendListRes>>(jsonData);

                    var viewSettings = new ViewSettings
                    {
                        ShowSidebar = false, // Tắt sidebar
                        ShowHeader = true,   // Bật header
                        ShowFriendList = false // Tắt danh sách bạn bè
                    };
                    ViewBag.ViewSettings = viewSettings;
                    return View(users);
                }
                else
                {
                    TempData["Error"] = "Không có lời mời nào.";
                    return View(new List<FriendListRes>());
                }
            }
        }
        [HttpPost]
        public IActionResult Unfriend(Friend_Manage req)
        {
            using (var httpClient = new HttpClient())
            {
                req.Status = FriendStatus.Unfriend;
                var json = JsonConvert.SerializeObject(req);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = httpClient.PostAsync("https://localhost:7296/api/friends/manage-friend-request", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Nếu cập nhật thành công, chuyển hướng đến AllFriends
                    TempData["Success"] = "Friend request handled successfully.";
                    return Redirect("/User/AllFriends");
                }
                else
                {
                    // Nếu có lỗi, hiển thị lỗi và ở lại trang hiện tại
                    TempData["Error"] = "Failed to handle friend request. Please try again later.";
                    return Ok();
                }
            }
        }

        [HttpPost]
        public IActionResult AcceptFriend(Friend_Manage req)
        {
            using (var httpClient = new HttpClient())
            {
                req.Status = FriendStatus.Accepted;
                var json = JsonConvert.SerializeObject(req);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = httpClient.PostAsync("https://localhost:7296/api/friends/manage-friend-request", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Nếu cập nhật thành công, chuyển hướng đến AllFriends
                    TempData["Success"] = "Friend request handled successfully.";
                    return Redirect("/User/AllFriends");
                }
                else
                {
                    // Nếu có lỗi, hiển thị lỗi và ở lại trang hiện tại
                    TempData["Error"] = "Failed to handle friend request. Please try again later.";
                    return Ok();
                }
            }
        }

        [HttpPost]
        public IActionResult CancelFriend(Friend_Manage req)
        {
            using (var httpClient = new HttpClient())
            {
                req.Status = FriendStatus.Cancel;
                var json = JsonConvert.SerializeObject(req);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = httpClient.PostAsync("https://localhost:7296/api/friends/manage-friend-request", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Nếu cập nhật thành công, chuyển hướng đến AllFriends
                    TempData["Success"] = "Friend request handled successfully.";
                    return Redirect("/User/AllFriends");
                }
                else
                {
                    // Nếu có lỗi, hiển thị lỗi và ở lại trang hiện tại
                    TempData["Error"] = "Failed to handle friend request. Please try again later.";
                    return Ok();
                }
            }
        }

        [HttpPost]
        public IActionResult RemoveSuggest(Friend_Manage req)
        {
            using (var httpClient = new HttpClient())
            {
                req.Status = FriendStatus.RemoveSuggest;
                var json = JsonConvert.SerializeObject(req);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = httpClient.PostAsync("https://localhost:7296/api/friends/manage-friend-request", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Nếu cập nhật thành công, chuyển hướng đến AllFriends
                    TempData["Success"] = "Friend request handled successfully.";
                    return Redirect("/User/SuggestFriend");
                }
                else
                {
                    // Nếu có lỗi, hiển thị lỗi và ở lại trang hiện tại
                    TempData["Error"] = "Failed to handle friend request. Please try again later.";
                    return Ok();
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInformation(Update_UserReq request)
        {
            var file = Request.Form.Files["ImageFile"];
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/uploads", file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                request.Image = "/uploads/" + file.FileName;
            }
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Không nhập đủ thông tin. Vui lòng thử lại.";
                return View();
            }

            using (var httpClient = new HttpClient())
            {
                string jsonData = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PutAsync("https://localhost:7296/api/User/update", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Cập nhật thông tin thành công.";
                    return Redirect("/");
                }
                else
                {
                    TempData["Error"] = "Đã xảy ra lỗi khi cập nhật thông tin. Vui lòng thử lại.";
                }
            }

            return View();
        }
        public async Task<IActionResult> SuggestFriend()
        {
            using (var httpClient = new HttpClient())
            {
                var userId = int.Parse(HttpContext.Items["userId"].ToString());
                HttpResponseMessage response = await httpClient.GetAsync($"https://localhost:7296/api/Friends/get-suggested-list/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var userInfoList = JsonConvert.DeserializeObject<List<Friend_SuggestUserRes>>(jsonData);
                    var viewSettings = new ViewSettings
                    {
                        ShowSidebar = false, // Tắt sidebar
                        ShowHeader = true,   // Bật header
                        ShowFriendList = false // Tắt danh sách bạn bè
                    };
                    ViewBag.ViewSettings = viewSettings;

                    return View(userInfoList);
                }
                else
                {
                    TempData["Error"] = "Không thể tải thông tin người dùng. Vui lòng thử lại sau.";
                    return View(new List<Friend_SuggestUserRes>());
                }
            }
        }
        [HttpPost]
        public IActionResult SendFriend(FriendReq request)
        {
            using (var httpClient = new HttpClient())
            {

                request.User1Id = int.Parse(HttpContext.Items["userId"].ToString());
                string jsonData = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = httpClient.PostAsync("https://localhost:7296/api/Friend/send-request", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Yêu cầu kết bạn đã được gửi thành công.";
                }
                else
                {
                    TempData["Error"] = "Không thể gửi yêu cầu kết bạn. Vui lòng thử lại sau.";
                }
                return Redirect("/User/SuggestFriend");

            }
        }
        public async Task<IActionResult> UpdateInformation()
        {
            using (var httpClient = new HttpClient())
            {
                int id = 1;
                HttpResponseMessage response = await httpClient.GetAsync($"https://localhost:7296/api/User/get/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var userInfo = JsonConvert.DeserializeObject<User_GetUserInfoRes>(jsonData);
                    return View(userInfo);
                }
                else
                {
                    TempData["Error"] = "Không thể tải thông tin người dùng. Vui lòng thử lại sau.";
                    return View();
                }
            }
        }
        public ActionResult AllFriends()
        {
            using (var httpClient = new HttpClient())
            {
                // Extract UserId from HttpContext
                var userId = HttpContext.Items["userId"]?.ToString();
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User ID is missing.");
                }

                // Prepare the request
                var req = new FriendListReq
                {
                    UserId = int.Parse(userId), // Convert UserId to int
                    Status = FriendStatus.Accepted
                };

                try
                {
                    // Serialize the request to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");

                    // Send POST request
                    HttpResponseMessage response = httpClient.PostAsync("https://localhost:7296/api/friends/get-friend-list", jsonContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        // Read and deserialize response
                        var jsonData = response.Content.ReadAsStringAsync().Result;
                        var users = JsonConvert.DeserializeObject<List<FriendListRes>>(jsonData);

                        // Prepare view settings
                        var viewSettings = new ViewSettings
                        {
                            ShowSidebar = false, // Disable sidebar
                            ShowHeader = true,   // Enable header
                            ShowFriendList = false // Disable friend list
                        };
                        ViewBag.ViewSettings = viewSettings;

                        // Pass the filtered list to the view
                        return View(users);
                    }
                    else
                    {
                        // Return empty list in case of unsuccessful response
                        return View(new List<FriendListRes>());
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle exceptions appropriately
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }
    }
}


