namespace DATN.MVC.Request.User
{
    public class Manage_FriendReq
    {
        public int Id { get; set; }
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public int Status { get; set; }
        public long CreatedDateUnix { get; set; }
    }
}
