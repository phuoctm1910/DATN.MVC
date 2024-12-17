using DATN.MVC.Helpers;
using DATN.MVC.Models.Response;
using DATN.MVC.Request.Post;
using DATN.MVC.Respone.Post;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace DATN.MVC.Controllers
{
    public class PostController : BaseController
    {
        private readonly IMemoryCache _memoryCache;

        // Constructor để inject IMemoryCache
        public PostController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            // Nếu không có cache, gọi API để lấy dữ liệu
            var result = ApiHelpers.GetMethod<List<Post_ReadAllRes>>("https://localhost:7296/api/post/getall", null);

            // Kiểm tra cache trước khi gọi API
            if (!_memoryCache.TryGetValue("PostLikes", out Dictionary<int, int> postLikes))
            {

                // Giả sử mỗi bài viết có một ID và một số lượt like (reactCount), ta cần lưu vào cache
                postLikes = result.ToDictionary(p => p.Id, p => p.ReactCount);

                // Lưu kết quả vào cache với thời gian sống là 5 phút (có thể điều chỉnh)
                _memoryCache.Set("PostLikes", postLikes, TimeSpan.FromMinutes(5));

               
            }

            // Nếu đã có cache, chỉ cần trả về số lượt like từ cache
            return Json(new
            {
                ApiData = result,
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
                    var result = ApiHelpers.PostMethodWithFileAsync<bool, Post_CreateReq>("https://localhost:7296/api/post/create", newPost, newPost.ImageFile, fileKeyName: "ImageFile");

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
                    return Json(new { success = false, message = "No image file provided." });
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
