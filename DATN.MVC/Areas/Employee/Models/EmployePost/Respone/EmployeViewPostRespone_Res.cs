namespace DATN.MVC.Areas.Employee.Models.EmployePost.Respone
{
    public class EmployeViewPostRespone_Res
    {
        public int Id { get; set; }             // ID của bài đăng
        public int UserId { get; set; }         // ID người dùng tạo bài đăng
        public int? PostId { get; set; }
        public string? Image { get; set; }      // URL hình ảnh
        public string? Content { get; set; }    // Nội dung bài đăng
        public int PostFor { get; set; }        // Đối tượng của bài đăng
        public bool IsActived { get; set; }     // Trạng thái bài đăng
        public int CreatedDate { get; set; }    // Timestamp tạo bài đăng
        public int UpdatedDate { get; set; }    // Timestamp cập nhật bài đăng
        public string? FullName { get; set; }   // Tên người dùng tạo bài đăng

        // Chuyển đổi sang DateTime
        public DateTime CreatedDateTime => ConvertFromUnixTimestamp(CreatedDate);
        public DateTime UpdatedDateTime => ConvertFromUnixTimestamp(UpdatedDate);

        private DateTime ConvertFromUnixTimestamp(int unixTimestamp)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTimestamp);
        }
    }
}
