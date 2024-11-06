namespace DATN.MVC.Request.User
{
    public class Update_UserReq
    {

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public DateTime Birth { get; set; }
        public bool Gender { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Image { get; set; } = "default.png";
        public string? BackgroundImage { get; set; }
        public int RoleId { get; set; }
        public byte Status { get; set; }
        public decimal Balance { get; set; }
        public bool IsActived { get; set; } = true;
        public int UpdatedDate { get; set; }
    }
}
