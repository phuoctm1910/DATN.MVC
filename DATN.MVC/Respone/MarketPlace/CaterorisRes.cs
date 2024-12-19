namespace DATN.MVC.Respone.MarketPlace
{
    public class CaterorisRes
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Image { get; set; }
        public bool IsActived { get; set; }
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
