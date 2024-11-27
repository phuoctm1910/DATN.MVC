namespace DATN.MVC.Ultilities
{
    public class Constants
    {
        public static string NotUpdatedMessage = "Dữ liệu chưa được cập nhật";
        public static Dictionary<int, string> ErrorCodes = new Dictionary<int, string>() {
            { 200, "Request successful" },
            { 204 , "Resource not found" }
        };
    }

    public enum MessageStatus : int
    {
        Sending = 0, // Đang gửi
        Sent = 1,    // Đã gửi
        Read = 2     // Đã đọc
    }


    public enum FriendStatus : int
    {
        Pending = 0,        // Gửi lời mời kết bạn
        Accepted = 1,       // Đã chấp nhận, trở thành bạn bè
        Blocked = 2,        // Bị chặn
        Cancel = 3,          // Bị hủy
        Unfriend = 4
    }

    public enum PostReact : int
    {
        NotReact = 0,         // Công khai
        React = 1,         // Bị chặn
    }

    public enum PostFor : int
    {
        Public = 0,         // Công khai
        ForFriends = 1,         // Bị chặn
        JustUser = 2        // Chỉ người dùng thấy
    }
    public enum CRUDStatusCodeRes
    {
        Success,
        ResourceNotFound,
        Deny,
        NoExecute,
        Exists,
        InvalidAction,
        InvalidData,
        ReturnWithData,
        ResetContent
    }
    public enum ErrorCodeEnum
    {
        Success = 200,
        NotFound = 204,
        InvalidData = 406
    }
}
