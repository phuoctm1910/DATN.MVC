namespace DATN.MVC.Respone.User
{
    public class FriendRequestResponse
    {
        public int Id { get; set; }
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public bool IsActived { get; set; }
        public int Status { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
    }
}
