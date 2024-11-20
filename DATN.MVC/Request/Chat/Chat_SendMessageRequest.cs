using DATN.MVC.Ultilities;

namespace DATN.MVC.Models.Request.Chat
{
    public class Chat_SendMessageRequest
    {
        public int ChatRoomId { get; set; }
        public int SenderId { get; set; }
        public string? Content { get; set; }
        public IEnumerable<IFormFile>? ImageFile { get; set; }

    }
}
