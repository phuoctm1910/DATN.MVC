using DATN.MVC.Helpers;


namespace DATN.MVC.Respone.User
{
    public class EmployeeUserResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public DateTime Birth { get; set; }
        public bool Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string BackgroundImage { get; set; }
        public int RoleId { get; set; }
        public byte Status { get; set; }
        public decimal Balance { get; set; }
        public bool IsActived { get; set; }
        public int CreatedDate { get; set; } // Unix timestamp
        public int UpdatedDate { get; set; } // Unix timestamp

        public DateTime CreatedDateTime => ConvertFromUnixTimestamp(CreatedDate);
        public DateTime UpdatedDateTime => ConvertFromUnixTimestamp(UpdatedDate);

        private DateTime ConvertFromUnixTimestamp(int unixTimestamp)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTimestamp);
        }
    }
}
