using DATN.MVC.Ultilities;
namespace DATN.MVC.Respone.MarketPlace
{
    public class SaleManRes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string Image { get; set; }
        public byte Status { get; set; }
        public bool IsActived { get; set; } // Dùng kiểu bool cho dữ liệu kiểu bit
        public long CreatedDate { get; set; }
        public long UpdatedDate { get; set; }
    }
}
