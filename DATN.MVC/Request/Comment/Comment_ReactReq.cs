using DATN.MVC.Ultilities;

namespace DATN.MVC.Request.Comment
{
    public class Comment_ReactReq
    {
        public int UserId { get; set; }
        public int CommentId { get; set; }
        public PostReact Type { get; set; }
    }
}
