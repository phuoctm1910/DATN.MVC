using DATN.MVC.Helpers;
using DATN.MVC.Ultilities;
using Newtonsoft.Json;
namespace DATN.MVC.Request.Post
{
    public class Post_CreateReq
    {
        public int UserId { get; set; }
        public int? PostId { get; set; }
        public string? Content { get; set; }
        public string CreatedDate { get; set; }
        public int? PostFor { get; set; }
    }


}