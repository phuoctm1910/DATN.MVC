namespace DATN.MVC.Respone.Chat
{
    public class Chat_ReadRes
    {
        public int RoomId { get; set; }
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public bool IsActive { get; set; }
        public string CreatedDate { get; set; }
    }
}
