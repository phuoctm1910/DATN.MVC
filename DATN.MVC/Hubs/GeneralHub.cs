using DATN.API.Models.Response;
using DATN.MVC.Helpers;
using DATN.MVC.Models;
using DATN.MVC.Models.Request;
using DATN.MVC.Request.Comment;
using DATN.MVC.Request.Post;
using DATN.MVC.Ultilities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel.Design;

namespace DATN.MVC.Hubs
{
    public class GeneralHub : Hub
    {
        private readonly IMemoryCache _memoryCache;
        private static readonly object _cacheLock = new object();
        // Constructor để inject IMemoryCache
        public GeneralHub(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        // Gửi thông báo bình luận mới cho tất cả client
        public async Task NotifyNewComment(int postId)
        {
            await Clients.All.SendAsync("CommentAdded", postId);
        }
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
        public async Task LikePost(int userId, int postId, int type)
        {
            var data = new Post_ReactReq
            {
                UserId = userId,
                PostId = postId,
                Type = (type == 1) ? PostReact.React : PostReact.NotReact
            };

            try
            {
                // Gọi API backend để xử lý like
                var result = ApiHelpers.PostMethodAsync<bool, Post_ReactReq>("https://localhost:7296/api/post/reactPost", data);

                if (result)
                {
                    // Cập nhật số lượng like trong GlobalCache.PostLikes
                    if (type == 1)
                    {
                        GlobalCache.PostLikes.AddOrUpdate(postId, 1, (key, oldValue) => oldValue + 1);
                    }
                    else
                    {
                        GlobalCache.PostLikes.AddOrUpdate(postId, 0, (key, oldValue) => Math.Max(0, oldValue - 1));
                    }

                    // Gửi thông báo real-time đến tất cả client
                    await Clients.All.SendAsync("ReceivePostLikeUpdate", new
                    {
                        PostId = postId,
                        LikeCount = GlobalCache.PostLikes[postId]
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task LikeComment(int userId, int commentId, int type)
        {
            var data = new Comment_ReactReq
            {
                UserId = userId,
                CommentId = commentId,
                Type = (type == 1) ? PostReact.React : PostReact.NotReact
            };

            try
            {
                // Thực hiện API gọi để xử lý hành động like
                var result = ApiHelpers.PostMethodAsync<bool, Comment_ReactReq>("https://localhost:7296/api/comment/reactComment", data);

                if (result)
                {
                    // Cập nhật số lượng like trong GlobalCache.PostLikes
                    if (type == 1)
                    {
                        GlobalCache.CommentLikes.AddOrUpdate(commentId, 1, (key, oldValue) => oldValue + 1);
                    }
                    else
                    {
                        GlobalCache.CommentLikes.AddOrUpdate(commentId, 0, (key, oldValue) => Math.Max(0, oldValue - 1));
                    }

                    // Gửi thông báo cho những người dùng khác trong nhóm (có thể nhóm bạn bè hoặc nhóm chung)
                    await Clients.All.SendAsync("ReceiveCommentLikeUpdate", new
                    {
                        CommentId = commentId,
                        LikeCount = GlobalCache.CommentLikes[commentId]
                    });
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (log, thông báo lỗi, ...)
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task AddComment(Comment_CreateReq newComment)
        {
            try
            {
                // Thực hiện API gọi để xử lý hành động like
                var result = ApiHelpers.PostMethodAsync<Comment_ReadAllRes, Comment_CreateReq>("https://localhost:7296/api/Comment/create", newComment);

                if (result != null)
                {
                    // Gửi thông báo cho những người dùng khác trong nhóm (có thể nhóm bạn bè hoặc nhóm chung)
                    await Clients.All.SendAsync("ReceiveNewComment", result, newComment.PostId);
                    await Clients.Others.SendAsync("UpdateCommentCount", newComment.PostId);

                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (log, thông báo lỗi, ...)
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        #region Thông báo real time
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
        #endregion
    }
}
