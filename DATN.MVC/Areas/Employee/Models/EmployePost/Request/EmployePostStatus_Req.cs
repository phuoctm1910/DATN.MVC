namespace DATN.MVC.Areas.Employee.Models.EmployePost.Request
{
    public class EmployePostStatus_Req
    {
        public int PostId { get; set; }      // ID của bài đăng cần cập nhật
        public bool IsActived { get; set; }  // Trạng thái mới của bài đăng
    }
}
