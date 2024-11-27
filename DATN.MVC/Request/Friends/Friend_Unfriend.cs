using DATN.MVC.Ultilities;

namespace DATN.MVC.Request.Friends
{
    public class Friend_Unfriend
    {
        public int FriendId { get; set; }
        public FriendStatus Status { get; set; }
    }
}
