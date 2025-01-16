using DATN.API.Models.Response;
using DATN.MVC.Helpers;
using DATN.MVC.Models;
using DATN.MVC.Models.Request;
using DATN.MVC.Models.Response.Comment;
using DATN.MVC.Request.Comment;
using DATN.MVC.Request.Post;
using DATN.MVC.Respone.Post;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;

namespace DATN.MVC.Controllers
{
    public class CommentController : BaseController
    {
        private readonly IMemoryCache _memoryCache;

        // Constructor để inject IMemoryCache
        public CommentController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        [HttpGet]
        public JsonResult GetCommentByPostId(int Id)
        {
            // Gọi API để lấy danh sách comment theo PostId
            var result = ApiHelpers.GetMethod<List<Comment_ReadAllRes>>(
                "https://localhost:7296/api/Comment/getListCommentByPostId/" + Id
            );

            // Nếu result = null hoặc rỗng thì ta return ngay một list rỗng.
            // Điều này đảm bảo khi bài post không có comment,
            // ta không hiển thị comment của bài cũ.
            if (result == null || !result.Any())
            {
                return Json(new { ApiData = new List<Comment_ReadAllRes>() });
            }

            // Nếu có comment, chỉ lúc này mới cập nhật vào GlobalCache
            // (để lưu số lượng reactCount cho comment).
            if (GlobalCache.CommentLikes.IsEmpty)
            {
                foreach (var comment in result)
                {
                    GlobalCache.CommentLikes.AddOrUpdate(
                        comment.Id,
                        comment.ReactCount,
                        (key, oldValue) => comment.ReactCount
                    );
                }
            }

            return Json(new { ApiData = result });
        }

        [HttpGet]
        public JsonResult GetReactionByComment(int commentid)
        {
            // Use string interpolation to correctly insert the postid into the URL
            var result = ApiHelpers.GetMethod<List<Comment_ReactReadByCommentRes>>($"https://localhost:7296/api/Comment/getListReactByComment/{commentid}");

            return Json(new
            {
                ApiData = result
            });
        }

        [HttpPost]
        public JsonResult AddNew([FromBody] Comment_CreateReq newComment)
        {
            try
            {
                // Call to the Web API via ApiHelpers.
                var result = ApiHelpers.PostMethodAsync<Comment_ReadAllRes, Comment_CreateReq>("https://localhost:7296/api/Comment/create", newComment);

                // Return success or failure response
                if (result != null)
                {
                    GlobalCache.CommentLikes.AddOrUpdate(result.Id, result.ReactCount, (key, oldValue) => result.ReactCount);

                    return Json(new { success = true, message = "Post created successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to create post." });
                }
            }
            catch (Exception ex)
            {
                // Handle the exception and return an error response.
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }

    }
}
