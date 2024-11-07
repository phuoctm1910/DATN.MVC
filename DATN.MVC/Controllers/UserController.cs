using DATN.MVC.Models;
using DATN.MVC.Request.User;
using DATN.MVC.Respone.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DATN.MVC.Controllers;

public class UserController : Controller
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

    public ActionResult Friends()
    {
        using (var httpClient = new HttpClient())
        {
            HttpResponseMessage response = httpClient.GetAsync("https://localhost:7296/api/friend/get-sent-friend-requests").Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var users = JsonConvert.DeserializeObject<List<FriendRequestResponse>>(jsonData);

                // Lọc chỉ lấy những người có status = 0
                var pendingRequests = users.Where(user => user.Status == 0).ToList();

                var viewSettings = new ViewSettings
                {
                    ShowSidebar = false, // Tắt sidebar
                    ShowHeader = true,   // Bật header
                    ShowFriendList = false // Tắt danh sách bạn bè
                };
                ViewBag.ViewSettings = viewSettings;
                return View(pendingRequests);
            }
            else
            {
                TempData["Error"] = "Không có lời mời nào.";
                return View(new List<FriendRequestResponse>());
            }
        }
    }

    [HttpPost]
    public IActionResult HandleFriend(Manage_FriendReq req)
    {
        using (var httpClient = new HttpClient())
        {
            var json = JsonConvert.SerializeObject(req);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = httpClient.PutAsync("https://localhost:7296/api/friend/manage-friend-request", content).Result;

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
                return RedirectToAction("Friends");
            }
        }
    }

    public ActionResult AllFriends()
    {
        using (var httpClient = new HttpClient())
        {
            HttpResponseMessage response = httpClient.GetAsync("https://localhost:7296/api/friend/get-all-friend").Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var users = JsonConvert.DeserializeObject<List<FriendRequestResponse>>(jsonData);

                // Lọc bạn bè có status = 1
                var friendsWithStatusOne = users.Where(friend => friend.Status == 1).ToList();

                var viewSettings = new ViewSettings
                {
                    ShowSidebar = false, // Tắt sidebar
                    ShowHeader = true,   // Bật header
                    ShowFriendList = false // Tắt danh sách bạn bè
                };
                ViewBag.ViewSettings = viewSettings;
                return View(friendsWithStatusOne);
            }
            else
            {
                return View(new List<FriendRequestResponse>());
            }
        }
    }

    // GET: UserController/Details/5
    public ActionResult Details(int id)
    {
        return View();
    }

    // GET: UserController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: UserController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: UserController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: UserController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: UserController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: UserController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
    private readonly string _apiBaseUrl = "https://localhost:7296/api/User/update";

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

            HttpResponseMessage response = await httpClient.PutAsync(_apiBaseUrl, content);

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
    public async Task<IActionResult> SendRequestFriend()
    {
        using (var httpClient = new HttpClient())
        {
            HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7296/api/User/getAll");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var userInfoList = JsonConvert.DeserializeObject<List<User_GetUserInfoRes>>(jsonData);
                return View(userInfoList);
            }
            else
            {
                TempData["Error"] = "Không thể tải thông tin người dùng. Vui lòng thử lại sau.";
                return View(new List<User_GetUserInfoRes>());
            }
        }
    }
    [HttpPost]
    public async Task<IActionResult> SendFriend(FriendReq request)
    {
        using (var httpClient = new HttpClient())
        {
            request.User1Id = 1;
            string jsonData = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync("https://localhost:7296/api/Friend/send-request", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Yêu cầu kết bạn đã được gửi thành công.";
            }
            else
            {
                TempData["Error"] = "Không thể gửi yêu cầu kết bạn. Vui lòng thử lại sau.";
            }
            return Redirect("/User/Friends");
        }
    }
}
