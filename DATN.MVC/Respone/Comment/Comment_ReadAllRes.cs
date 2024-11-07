namespace DATN.MVC.Models.Response
{
    public class Comment_ReadAllRes
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public int CommentId { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsActived { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set;}

    }
}
