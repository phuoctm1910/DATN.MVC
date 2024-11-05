using DATN.MVC.Helpers;
using System;

namespace DATN.MVC.Models.Response
{
    public class Post_ReadAllRes
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string PostContent { get; set; }
        public int? PostShareId { get; set; }
        public int PostFor { get; set; }
        public bool IsActived { get; set; }
        public int CreatedDate { get; set; }
        public int UpdatedDate { get; set; }
        public int CommentCount { get; set; }
        public int ReactCount { get; set; }
        public int ShareCount { get; set; }

        public int? OriginalIdPost { get; set; } 
        public string? OriginalPostOwner { get; set; }
        public int? OriginalPostCreatedDate { get; set; }
        public int? OriginalPostUpdatedDate { get; set; }

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

        // DateTime formatting helpers for the original post if it's shared
        public DateTime? OriginalPostCreatedDateTime
        {
            get => OriginalPostCreatedDate.HasValue
                ? DateTimeHelper.UnixToDateTime(OriginalPostCreatedDate.Value)
                : (DateTime?)null;
        }

        public DateTime? OriginalPostUpdatedDateTime
        {
            get => OriginalPostUpdatedDate.HasValue
                ? DateTimeHelper.UnixToDateTime(OriginalPostUpdatedDate.Value)
                : (DateTime?)null;
        }

        // Formatted date-time for the original shared post
        public string OriginalPostCreatedDateFormatted
        {
            get => OriginalPostCreatedDateTime.HasValue
                ? DateTimeHelper.FormatDateTimeVietnam(OriginalPostCreatedDateTime.Value)
                : null;
        }

        public string OriginalPostUpdatedDateFormatted
        {
            get => OriginalPostUpdatedDateTime.HasValue
                ? DateTimeHelper.FormatDateTimeVietnam(OriginalPostUpdatedDateTime.Value)
                : null;
        }
    }
}
