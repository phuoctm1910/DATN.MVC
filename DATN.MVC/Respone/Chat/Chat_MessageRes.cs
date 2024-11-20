namespace DATN.MVC.Models.Response.Chat
{
    public class Chat_MessageRes
    {
        public int Id { get; set; }
        public int ChatRoomId { get; set; }
        public int SenderId { get; set; }
        public string? Content { get; set; }
        public int CreatedDate { get; set; }
        public int UpdatedDate { get; set;}
        public int Status { get; set; }
        public List<string>? MessageImageUrls { get; set; }

    }
}
