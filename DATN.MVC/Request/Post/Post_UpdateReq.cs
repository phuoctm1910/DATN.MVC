using DATN.MVC.Ultilities;

namespace DATN.MVC.Request.Post
{
    public class Post_UpdateReq
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public PostFor PostFor { get; set; }
        public string UpdatedDate { get; set; }
    }
}
