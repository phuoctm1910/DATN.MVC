namespace DATN.MVC.Respone.Message
{
    public class Message_ReadRes
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}