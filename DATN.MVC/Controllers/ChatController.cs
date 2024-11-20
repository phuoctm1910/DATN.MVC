using DATN.MVC.Helpers;
using DATN.MVC.Hubs;
using DATN.MVC.Models;
using DATN.MVC.Models.Request.Chat;
using DATN.MVC.Models.Response.Chat;
using DATN.MVC.Request.Friends;
using DATN.MVC.Respone.Chat;
using DATN.MVC.Respone.Friends;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.JSInterop;

namespace DATN.MVC.Controllers
{
    public class ChatController : Controller
    {
        private readonly IHubContext<ChatHub> _chatHubContext;

        public ChatController(IHubContext<ChatHub> chatHubContext)
        {
            _chatHubContext = chatHubContext;
        }
        [HttpGet]
        public JsonResult GetMessages(int chat_roomId)
        {
            var result = ApiHelpers.GetMethod<IEnumerable<Chat_MessageRes>>($"https://localhost:7296/api/chatroom/get-messages/{chat_roomId}");
            return Json(new { ApiData = result });
        }
        [HttpPost]
        public JsonResult CreateChatRoom([FromBody] Chat_CreateReq req)
        {
            try
            {
                var chatRoomId = ApiHelpers.PostMethodAsync<Chat_ChatRoomRes, Chat_CreateReq>("https://localhost:7296/api/chatroom/create-chat-room", req);

                if (chatRoomId != null)
                {

                    return Json(new { success = true, chatRoomId, message = "Chat room created successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to create chat room." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }

        public IActionResult Index()
        {

            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId != null)
            {
                ViewBag.UserId = userId;
                ViewBag.FullName = HttpContext.Session.GetString("FullName");
                ViewBag.Token = HttpContext.Session.GetString("Token");
                var viewSettings = new ViewSettings
                {
                    ShowSidebar = true, // Tắt sidebar
                    ShowHeader = true,   // Bật header
                    ShowFriendList = true // Tắt danh sách bạn bè
                };
                ViewBag.ViewSettings = viewSettings;

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
