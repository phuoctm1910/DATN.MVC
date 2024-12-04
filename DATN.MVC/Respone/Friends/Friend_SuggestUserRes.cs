namespace DATN.MVC.Respone.Friends
{
    public class Friend_SuggestUserRes
    {
        public int UserId { get; set; }
        public string? FullName { get; set; }
        public string? UserImage { get; set; }
        public int MutualFriendsCount { get; set; }
    }
}
