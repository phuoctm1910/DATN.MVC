using DATN.MVC.Helpers;
using DATN.MVC.Models;
using DATN.MVC.Models.Response;
using DATN.MVC.Request.Post;
using DATN.MVC.Respone.Post;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace DATN.MVC.Controllers
{
    public class PostController : BaseController
    {
        public PostController()
        {
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            // Gọi API để lấy dữ liệu nếu ConcurrentDictionary chưa có giá trị
            var result = ApiHelpers.GetMethod<List<Post_ReadAllRes>>("https://localhost:7296/api/post/getall", null);

            // Nếu ConcurrentDictionary chưa có dữ liệu, cập nhật từ API
            if (GlobalCache.PostLikes.IsEmpty)
            {
                foreach (var post in result)
                {
                    GlobalCache.PostLikes.AddOrUpdate(post.Id, post.ReactCount, (key, oldValue) => post.ReactCount);
                }
            }

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
        public JsonResult ShareAndAddNew([FromForm] Post_CreateReq newPost)
        {
            try
            {
                // Kiểm tra nếu có file ảnh để upload
                if (newPost.ImageFile != null)
                {
                    // Gọi API backend để lưu trữ thông tin bài đăng (bao gồm cả file ảnh)
                    var result = ApiHelpers.PostMethodWithFileAsync<bool, Post_CreateReq>("https://localhost:7296/api/post/create", newPost, newPost.ImageFile, "ImageFile");

                    if (result)
                    {
                        return Json(new { success = true, message = "Post created successfully." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Failed to create post." });
                    }
                }
                else
                {
                    // Gọi API backend để lưu trữ thông tin bài đăng (bao gồm cả file ảnh)
                    var result = ApiHelpers.PostMethodAsyncV2<bool, Post_CreateReq>("https://localhost:7296/api/post/create", newPost);

                    if (result)
                    {
                        return Json(new { success = true, message = "Post created successfully." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Failed to create post." });
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về phản hồi lỗi
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }
        [HttpPost]
        public JsonResult EditPost([FromForm] Post_UpdateReq editPost)
        {
            try
            {
                // Kiểm tra nếu có file ảnh để upload
                if (editPost.NewImageFile != null)
                {
                    // Gọi API backend để lưu trữ thông tin bài đăng (bao gồm cả file ảnh)
                    var result = ApiHelpers.PutMethodWithFileAsync<bool, Post_UpdateReq>("https://localhost:7296/api/post/edit", editPost, editPost.NewImageFile, "NewImageFile");

                    if (result)
                    {
                        return Json(new { success = true, message = "Post edited successfully." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Failed to edit post." });
                    }
                }
                else
                {
                    // Gọi API backend để lưu trữ thông tin bài đăng (bao gồm cả file ảnh)
                    var result = ApiHelpers.PutMethodAsyncV2<bool, Post_UpdateReq>("https://localhost:7296/api/post/edit", editPost);

                    if (result)
                    {
                        return Json(new { success = true, message = "Post edited successfully." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Failed to edit post." });
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về phản hồi lỗi
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }
        [HttpPost]
        public JsonResult DeletePost([FromBody] Post_DeleteReq req)
        {
            try
            {

                    // Gọi API backend để lưu trữ thông tin bài đăng (bao gồm cả file ảnh)
                    var result = ApiHelpers.DeleteMethodAsync<bool, Post_DeleteReq>("https://localhost:7296/api/post/delete/", req);

                    if (result)
                    {
                        return Json(new { success = true, message = "Post delete successfully." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Failed to delete post." });
                    }

            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về phản hồi lỗi
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
