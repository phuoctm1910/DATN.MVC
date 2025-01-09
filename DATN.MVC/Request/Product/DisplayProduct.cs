namespace DATN.MVC.Request.Product
{
    public class DisplayProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; } 
        public int Quantity { get; set; }

        public string Description { get; set; }
        public string? Image { get; set; }
        public bool IsActived { get; set; }
        public int CategoryId { get; set; }
  
        public int CreatedDate { get; set; }
        public int UpdatedDate { get; set; }

        // Chuyển đổi Unix timestamp sang DateTime
        public DateTime CreatedDateTime => ConvertFromUnixTimestamp(CreatedDate);
        public DateTime UpdatedDateTime => ConvertFromUnixTimestamp(UpdatedDate);

        private DateTime ConvertFromUnixTimestamp(int unixTimestamp)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTimestamp);
        }
    }
}
