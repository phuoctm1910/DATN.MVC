using DATN.MVC.Helpers;
using DATN.MVC.Request.Friends;
using DATN.MVC.Respone.Friends;
using DATN.MVC.Ultilities;
using Microsoft.AspNetCore.SignalR;

namespace DATN.MVC.Hubs
{
    public class StatusHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userIdStr = httpContext.Request.Query["userId"].ToString();

            if (!string.IsNullOrEmpty(userIdStr))
            {
                // 1. Lưu thông tin kết nối
                ConnectionMapping.AddConnection(userIdStr, Context.ConnectionId);

                if (int.TryParse(userIdStr, out int currentUserId))
                {
                    // 2. Lấy danh sách bạn bè
                    var friendList = await GetFriendList(currentUserId);
                    // friendList: List<FriendListRes>

                    // 3. Gửi thông báo "UserOnline" đến bạn bè
                    var friendConnections = new List<string>();
                    foreach (var friend in friendList)
                    {
                        var friendConnectionIds = ConnectionMapping.GetConnections(friend.FriendUserId.ToString());
                        friendConnections.AddRange(friendConnectionIds);
                    }
                    await Clients.Clients(friendConnections).SendAsync("UserOnline", userIdStr);

                    // =========================
                    // PHẦN BẠN THÊM Ở ĐÂY
                    // =========================
                    // 4. Lọc những friend nào đang online
                    var onlineFriends = friendList
                        .Where(f => ConnectionMapping.IsUserOnline(f.FriendUserId.ToString()))
                        .Select(f => f.FriendUserId.ToString())
                        .ToList();

                    // 5. Gửi danh sách friend đang online cho chính user này
                    // Tức là client caller
                    await Clients.Caller.SendAsync("FriendsAlreadyOnline", onlineFriends);
                }
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            var userIdStr = httpContext.Request.Query["userId"].ToString();

            if (!string.IsNullOrEmpty(userIdStr))
            {
                // 1. Gỡ connectionId
                ConnectionMapping.RemoveConnection(userIdStr, Context.ConnectionId);

                // 2. Nếu user này thực sự không còn connection => user offline
                if (!ConnectionMapping.IsUserOnline(userIdStr))
                {
                    if (int.TryParse(userIdStr, out int currentUserId))
                    {
                        // 3. Lấy danh sách bạn bè
                        var friendList = await GetFriendList(currentUserId);

                        // 4. Lấy connections của bạn bè
                        var friendConnections = new List<string>();
                        foreach (var friend in friendList)
                        {
                            var friendConnectionIds = ConnectionMapping.GetConnections(friend.FriendUserId.ToString());
                            friendConnections.AddRange(friendConnectionIds);
                        }

                        // 5. Gửi event UserOffline đến connections của bạn bè
                        await Clients.Clients(friendConnections).SendAsync("UserOffline", userIdStr);


                    }
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        private async Task<List<FriendListRes>> GetFriendList(int userId)
        {
            var req = new FriendListReq
            {
                UserId = userId,
                // Lấy danh sách friend đã Accepted, tùy bạn
                Status = FriendStatus.Accepted
            };

            // gọi API, trả về IEnumerable<FriendListRes>
            var result =  ApiHelpers.PostMethodAsync<IEnumerable<FriendListRes>, FriendListReq>(
                "https://localhost:7296/api/Friends/get-friend-list", req);

            // ép sang List
            return result?.ToList() ?? new List<FriendListRes>();
        }
    }
}
