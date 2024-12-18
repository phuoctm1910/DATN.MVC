using DATN.MVC.Helpers;
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
                // Thực hiện API gọi để xử lý hành động like
                var result = ApiHelpers.PostMethodAsync<bool, Post_ReactReq>("https://localhost:7296/api/post/reactPost", data);

                if (result)
                {
                    // Cập nhật số lượng like trong cache sau khi phản hồi thành công
                    if (_memoryCache.TryGetValue("PostLikes", out Dictionary<int, int> postLikes))
                    {
                        // Tăng số lượng like cho bài viết hiện tại
                        if (postLikes.ContainsKey(postId))
                        {
                            postLikes[postId] += (type == 1) ? 1 : -1;
                        }
                        else
                        {
                            postLikes[postId] = (type == 1) ? 1 : 0;
                        }

                        // Cập nhật lại cache sau khi thay đổi số lượng like
                        _memoryCache.Set("PostLikes", postLikes, TimeSpan.FromMinutes(5));

                        // Gửi thông báo cho những người dùng khác trong nhóm (có thể nhóm bạn bè hoặc nhóm chung)
                        await Clients.All.SendAsync("ReceivePostLikeUpdate", new
                        {
                            PostId = postId,
                            LikeCount = postLikes[postId]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (log, thông báo lỗi, ...)
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
                    // Cập nhật số lượng like trong cache sau khi phản hồi thành công
                    if (_memoryCache.TryGetValue("CommentLikes", out Dictionary<int, int> commentLikes))
                    {
                        // Tăng số lượng like cho bài viết hiện tại
                        if (commentLikes.ContainsKey(commentId))
                        {
                            commentLikes[commentId] += (type == 1) ? 1 : -1;
                        }
                        else
                        {
                            commentLikes[commentId] = (type == 1) ? 1 : 0;
                        }

                        // Cập nhật lại cache sau khi thay đổi số lượng like
                        _memoryCache.Set("CommentLikes", commentLikes, TimeSpan.FromMinutes(5));

                        // Gửi thông báo cho những người dùng khác trong nhóm (có thể nhóm bạn bè hoặc nhóm chung)
                        await Clients.All.SendAsync("ReceiveCommentLikeUpdate", new
                        {
                            CommentId = commentId,
                            LikeCount = commentLikes[commentId]
                        });
                    }
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
                var result = ApiHelpers.PostMethodAsync<bool, Comment_CreateReq>("https://localhost:7296/api/Comment/create", newComment);

                if (result)
                {
                    // Gửi thông báo cho những người dùng khác trong nhóm (có thể nhóm bạn bè hoặc nhóm chung)
                    await Clients.All.SendAsync("ReceiveNewComment", newComment);
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
