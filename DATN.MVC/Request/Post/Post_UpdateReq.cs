using DATN.MVC.Ultilities;

namespace DATN.MVC.Request.Post
{
    public class Post_UpdateReq
    {
        public int Id { get; set; }
        public PostFor? PostFor { get; set; }
        public string? Content { get; set; }
        public IEnumerable<IFormFile> NewImageFile { get; set; }
        public string? OldImageNameRemove { get; set; }
        public string? OldImageNameKeep { get; set; }
        public string? UpdatedDate { get; set; }
    }
}
