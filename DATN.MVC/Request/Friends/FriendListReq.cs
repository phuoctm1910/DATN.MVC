using DATN.MVC.Ultilities;

namespace DATN.MVC.Request.Friends
{
    public class FriendListReq
    {
        public int UserId { get; set; }
        public FriendStatus Status { get; set; }
    }
}
