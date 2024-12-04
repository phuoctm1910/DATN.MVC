using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace DATN.MVC.Hubs
{
    public class NotificationHub : Hub
    {
        // Khi một user kết nối, tự động thêm vào group của họ
        public override async Task OnConnectedAsync()
        {
            if (Context.Items.ContainsKey("userId"))
            {
                var userId = Context.Items["userId"]?.ToString();
                if (!string.IsNullOrEmpty(userId))
                {
                    // Thêm user vào group riêng của họ
                    await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
                }
            }
            await base.OnConnectedAsync();
        }

        // Khi user ngắt kết nối, tự động rời khỏi group
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (Context.Items.ContainsKey("userId"))
            {
                var userId = Context.Items["userId"]?.ToString();
                if (!string.IsNullOrEmpty(userId))
                {
                    // Xóa kết nối khỏi group riêng của họ
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"User_{userId}");
                }
            }
            await base.OnDisconnectedAsync(exception);
        }

        // Gửi thông báo cá nhân
        public async Task SendPersonalNotification(string userId, string message)
        {
            // Gửi thông báo tới group của user cụ thể
            await Clients.Group($"User_{userId}").SendAsync("ReceiveNotification", message);
        }

        // Gửi thông báo tới tất cả bạn bè của user
        public async Task SendBroadcastNotification(string groupId, string message)
        {
            // Gửi thông báo tới group bạn bè của user
            await Clients.Group(groupId).SendAsync("ReceiveNotification", message);
        }

        // Thêm user vào group bạn bè
        public async Task JoinFriendsGroup(string userId)
        {
            var groupName = $"FriendsOf_{userId}";
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        // Rời khỏi group bạn bè
        public async Task LeaveFriendsGroup(string userId)
        {
            var groupName = $"FriendsOf_{userId}";
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
