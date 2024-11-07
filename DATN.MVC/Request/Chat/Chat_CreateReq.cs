using DATN.MVC.Helpers;
using Newtonsoft.Json;

namespace DATN.MVC.Models.Request.Chat
{
    public class Chat_CreateReq
    {
        public int User1Id { get; set; }
        public int User2Id { get; set; }
    }
}
