using DATN.API.Models.Response;
using DATN.MVC.Helpers;
using DATN.MVC.Models.Request;
using DATN.MVC.Models.Response.Comment;
using DATN.MVC.Request.Comment;
using DATN.MVC.Request.Post;
using DATN.MVC.Respone.Post;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;

namespace DATN.MVC.Controllers
{
    public class CommentController : Controller
    {
        [HttpGet]
        public JsonResult GetCommentByPostId(int Id)
        {
            var result = ApiHelpers.GetMethod<List<Comment_ReadAllRes>>("https://localhost:7296/api/Comment/getListCommentByPostId/" + Id);
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
                var result = ApiHelpers.PostMethodAsync<bool, Comment_CreateReq>("https://localhost:7296/api/Comment/create", newComment);

                // Return success or failure response
                if (result)
                {
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
        [HttpPost]
        public JsonResult ReactionComment([FromBody] Comment_ReactReq newPost)
        {
            try
            {
                // Call to the Web API via ApiHelpers.
                var result = ApiHelpers.PostMethodAsync<bool, Comment_ReactReq>("https://localhost:7296/api/comment/reactComment", newPost);

                // Return success or failure response
                if (result)
                {
                    return Json(new { success = true, message = "React post successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to react post." });
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
