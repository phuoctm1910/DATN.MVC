using DATN.MVC.Helpers;
using DATN.MVC.Models.Response;
using DATN.MVC.Request.Post;
using DATN.MVC.Respone.Post;
using Microsoft.AspNetCore.Mvc;

namespace DATN.MVC.Controllers
{
    public class PostController : Controller
    {
        [HttpGet]
        public JsonResult GetAll()
        {
            var result = ApiHelpers.GetMethod<List<Post_ReadAllRes>>("https://localhost:7296/api/post/getall", null);
            return Json(new
            {
                ApiData = result
            });
        }

        [HttpGet]
        public JsonResult GetReactionByPost(int postid)
        {
            // Use string interpolation to correctly insert the postid into the URL
            var result = ApiHelpers.GetMethod<List<Post_ReactReadByPostRes>>($"https://localhost:7296/api/post/getListReactByPostId/{postid}");

            return Json(new
            {
                ApiData = result
            });
        }


        [HttpPost]
        public JsonResult ShareAndAddNew([FromBody] Post_CreateReq newPost)
        {
            try
            {
                // Call to the Web API via ApiHelpers.
                var result = ApiHelpers.PostMethodAsync<bool, Post_CreateReq>("https://localhost:7296/api/post/create", newPost);

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
        public JsonResult ReactionPost([FromBody] Post_ReactReq newPost)
        {
            try
            {
                // Call to the Web API via ApiHelpers.
                var result = ApiHelpers.PostMethodAsync<bool, Post_ReactReq>("https://localhost:7296/api/post/reactPost", newPost);

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
