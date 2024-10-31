using Newtonsoft.Json;

namespace DATN.MVC.Models.Request
{
    public class Comment_CreateReq
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public int? CommentId { get; set; }

        // Store Unix timestamp directly
        public long CreatedDateUnix { get; set; }

        //[JsonIgnore]
        //public DateTime CreatedDate
        //{
        //    get
        //    {
        //        return DateTimeHelper.UnixToDateTime(CreatedDateUnix);
        //    }
        //    set
        //    {
        //        CreatedDateUnix = DateTimeHelper.DateTimeToUnix(value);
        //    }
        //}
    }
}
