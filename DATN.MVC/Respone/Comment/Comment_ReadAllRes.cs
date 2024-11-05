using DATN.MVC.Helpers;

namespace DATN.API.Models.Response
{
    public class Comment_ReadAllRes
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Content { get; set; }
        public int? CommentId { get; set; }
        public bool IsUpdated { get; set; }
        public bool IsActived { get; set; }
        public int CreatedDate { get; set; }
        public int UpdatedDate { get; set; }
        public int ReactCount { get; set; }

        // Định dạng DateTime từ Unix timestamp
        public DateTime CreatedDateTime
        {
            get => DateTimeHelper.UnixToDateTime(CreatedDate);
        }

        public DateTime UpdatedDateTime
        {
            get => DateTimeHelper.UnixToDateTime(UpdatedDate);
        }

        // Định dạng ngày giờ theo kiểu hiển thị
        public string CreatedDateFormatted
        {
            get => DateTimeHelper.FormatDateTimeVietnam(CreatedDateTime);
        }

        public string UpdatedDateFormatted
        {
            get => DateTimeHelper.FormatDateTimeVietnam(UpdatedDateTime);
        }

    }
}
