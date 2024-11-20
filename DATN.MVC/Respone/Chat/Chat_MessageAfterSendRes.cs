namespace DATN.MVC.Respone.Chat
{
    public class Chat_MessageAfterSendRes
    {
        public int MessageId { get; set; }
        public int CreatedDate { get; set; }
        public List<string>? MessageImageUrls { get; set; }

    }
}
