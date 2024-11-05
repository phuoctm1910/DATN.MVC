using DATN.MVC.Ultilities;

namespace DATN.MVC.Request.Post
{
    public class Post_ReactReq
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
        public PostReact Type { get; set; }
    }
}
