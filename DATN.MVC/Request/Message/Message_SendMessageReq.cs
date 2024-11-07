namespace DATN.MVC.Request.Message
{
    public class Message_SendMessageReq
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
    }
}
