namespace DATN.MVC.Areas.Admin.Models.AdminPost.Request
{
    public class EditPostStatus_Req
    {
        public int PostId { get; set; }      // ID của bài đăng cần cập nhật
        public bool IsActived { get; set; }  // Trạng thái mới của bài đăng
    }
}
