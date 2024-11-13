namespace DATN.MVC.Areas.Admin.Models.AdminPost.Response
{
    public class ViewPostResponse_Res
    {
        public int Id { get; set; } // Có thể 'Id' chính là 'PostId'
        public string Name { get; set; }
        public int UserId { get; set; }
        public string Image { get; set; }
        public byte Status { get; set; }
        public bool IsActived { get; set; } // Dùng kiểu bool cho dữ liệu kiểu bit
        public int CreatedDate { get; set; }
        public int UpdatedDate { get; set; }
    }
}
