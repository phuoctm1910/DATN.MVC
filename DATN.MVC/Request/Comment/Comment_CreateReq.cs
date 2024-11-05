using Newtonsoft.Json;

namespace DATN.MVC.Models.Request
{
    public class Comment_CreateReq
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public int? CommentId { get; set; }
        public string CreatedDate { get; set; }

    }
}
