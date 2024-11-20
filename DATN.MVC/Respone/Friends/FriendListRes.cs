using DATN.MVC.Ultilities;

namespace DATN.MVC.Respone.Friends
{
    public class FriendListRes
    {
        public int FriendUserId { get; set; }
        public FriendStatus FriendshipStatus { get; set; }
        public string? FriendFullName { get; set; }
        public string? FriendImage { get; set; }
        public bool IsFriendOnline { get; set; }
        public int ChatRoomId { get; set; }

    }
}
