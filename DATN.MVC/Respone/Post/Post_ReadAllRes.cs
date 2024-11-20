using DATN.MVC.Helpers;
using System;

namespace DATN.MVC.Models.Response
{
    public class Post_ReadAllRes
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string? PosterImage { get; set; }
        public string PostContent { get; set; }
        public int? PostShareId { get; set; }
        public int PostFor { get; set; }
        public bool IsActived { get; set; }
        public int CreatedDate { get; set; }
        public int UpdatedDate { get; set; }
        public int CommentCount { get; set; }
        public int ReactCount { get; set; }
        public int ShareCount { get; set; }
        public List<string>? ImageUrls { get; set; }


        // DateTime formatting helpers for the main post
        public DateTime CreatedDateTime
        {
            get => DateTimeHelper.UnixToDateTime(CreatedDate);
        }

        public DateTime UpdatedDateTime
        {
            get => DateTimeHelper.UnixToDateTime(UpdatedDate);
        }

        // Formatted date-time for display
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
