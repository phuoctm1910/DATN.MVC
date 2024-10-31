namespace DATN.MVC.Models.Request
{
    public class Comment_UpdateReq
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }

        // Store Unix timestamp directly
        public long UpdatedDateUnix { get; set; }

        //[JsonIgnore]
        //public DateTime UpdatedDate
        //{
        //    get
        //    {
        //        return DateTimeHelper.UnixToDateTime(UpdatedDateUnix);
        //    }
        //    set
        //    {
        //        UpdatedDateUnix = DateTimeHelper.DateTimeToUnix(value);
        //    }
        //}
    }
}
