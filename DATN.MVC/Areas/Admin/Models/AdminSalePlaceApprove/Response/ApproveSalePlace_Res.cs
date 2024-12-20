namespace DATN.MVC.Respone.SalePlaceApprove
{
    public class ApproveSalePlace_Res
    {
        public int Id { get; set; } // ID của SalePlace
        public string Name { get; set; } // Tên SalePlace
        public int? UserId { get; set; } // ID của người dùng
        public string Image { get; set; } // Link hình ảnh của SalePlace
        public byte? Status { get; set; } // Trạng thái (0: Chờ, 1: Đang hoạt động,...)
        public bool IsActived { get; set; }  // Đã được duyệt hay chưa
        public int CreatedDate { get; set; }
        public int UpdatedDate { get; set; }

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
