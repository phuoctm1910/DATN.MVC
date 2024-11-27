using DATN.MVC.Helpers;
using DATN.MVC.Request.Friends;
using DATN.MVC.Respone.Friends;
using Microsoft.AspNetCore.Mvc;

namespace DATN.MVC.Controllers
{
    public class FriendController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetListFriendOfUser([FromBody] FriendListReq req)
        {
            var result = ApiHelpers.PostMethodAsync<IEnumerable<FriendListRes>, FriendListReq>("https://localhost:7296/api/Friends/get-friend-list", req);
            return Json(new { ApiData = result });
        }
    }
}
