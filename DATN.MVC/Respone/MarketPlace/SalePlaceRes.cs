using DATN.MVC.Ultilities;
namespace DATN.MVC.Respone.MarketPlace
{
    public class SalePlaceRes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string Image { get; set; }
        public byte Status { get; set; }
        public bool IsActived { get; set; } // Dùng kiểu bool cho dữ liệu kiểu bit
        public int CreatedDate { get; set; }
        public int UpdatedDate { get; set; }

        public DateTime CreatedDateTime => ConvertFromUnixTimestamp(CreatedDate);
        public DateTime UpdatedDateTime => ConvertFromUnixTimestamp(UpdatedDate);

        private DateTime ConvertFromUnixTimestamp(int unixTimestamp)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTimestamp);
        }
    }
}
