namespace DATN.MVC.Respone.User
{
    public class User_GetUserInfoRes
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
        public int CreatedDate { get; set; }
        public int UpdatedDate { get; set; }
        public int MutualFriendsCount { get; set; }
    }
}
