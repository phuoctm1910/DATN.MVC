using DATN.MVC.Helpers;
using DATN.MVC.Models;
using DATN.MVC.Models.Request.Chat;
using DATN.MVC.Request.Message;
using DATN.MVC.Respone.Chat;
using DATN.MVC.Respone.Message;
using DATN.MVC.Ultilities;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DATN.MVC.Hubs
{
    public class ChatHub : Hub
    {
        public async Task<Chat_MessageAfterSendRes> SendMessageToGroup(int chatRoomId, int senderId, string content, string fileMetadataJson = null)
        {
            // Chuyển đổi JSON metadata thành danh sách file
            List<FileMetaData> fileMetadataList = string.IsNullOrEmpty(fileMetadataJson)
                ? new List<FileMetaData>()
                : JsonConvert.DeserializeObject<List<FileMetaData>>(fileMetadataJson) ?? new List<FileMetaData>();

            // Chuyển đổi metadata thành IFormFile
            var formFiles = fileMetadataList
                .Select(metadata => FileHelper.CreateFormFile(metadata))
                .Where(file => file != null) // Loại bỏ file null
                .ToList();

            var messageData = new Chat_SendMessageRequest
            {
                ChatRoomId = chatRoomId,
                SenderId = senderId,
                Content = content,
                ImageFile = formFiles // Truyền xuống API
            };

            try
            {
                // Gọi API để gửi tin nhắn và ảnh
                var result = ApiHelpers.PostMethodWithFileAsync<Chat_MessageAfterSendRes, Chat_SendMessageRequest>(
                    "https://localhost:7296/api/chatroom/send-message", messageData, messageData.ImageFile, fileKeyName: "ImageFile");

                if (result != null && result.MessageId > 0)
                {
                    var updateStatus = ApiHelpers.PostMethodAsync<bool, object>(
                        "https://localhost:7296/api/chatroom/update-message-status",
                        new { MessageId = result.MessageId, Status = MessageStatus.Sent });

                    if (updateStatus)
                    {
                        await Clients.Caller.SendAsync("UpdateMessageStatus", result.MessageId, MessageStatus.Sent);

                        await Clients.OthersInGroup(chatRoomId.ToString())
                            .SendAsync("ReceiveMessage", senderId, content, result.CreatedDate, result.MessageId, result.MessageImageUrls);

                        return result;
                    }
                }

                throw new HubException("Failed to save the message to the database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while sending the message: " + ex.Message);
                throw new HubException("An error occurred while processing the message.", ex);
            }
        }

        public async Task MarkMessagesAsRead(int chatRoomId, List<int> messageIds)
        {
            try
            {
                // Gọi API để cập nhật trạng thái tin nhắn thành "Đã đọc"
                var result = ApiHelpers.PostMethodAsync<bool, object>(
                    "https://localhost:7296/api/chatroom/update-message-status",
                    new { MessageIds = messageIds, Status = MessageStatus.Read });

                if (result)
                {
                    // Gửi thông báo về cho người gửi
                    await Clients.OthersInGroup(chatRoomId.ToString()).SendAsync("MessagesMarkedAsRead", messageIds);

                    Console.WriteLine("Messages marked as read for chat room ID: " + chatRoomId);
                }
                else
                {
                    throw new HubException("Failed to update message status to 'Read'.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while marking messages as read: " + ex.Message);
                throw new HubException("An error occurred while processing the request.", ex);
            }
        }
        public async Task NotifyMessageRead(int chatRoomId, int messageId)
        {
            try
            {
                // Cập nhật trạng thái "Đã đọc" ngay lập tức cho tin nhắn
                var result = ApiHelpers.PostMethodAsync<bool, object>(
                    "https://localhost:7296/api/chatroom/update-message-status",
                    new { MessageId = messageId, Status = MessageStatus.Read });

                if (result)
                {
                    await Clients.OthersInGroup(chatRoomId.ToString()).SendAsync("MessageRead", messageId);
                    Console.WriteLine($"Message ID {messageId} marked as read in chat room ID {chatRoomId}");
                }
                else
                {
                    throw new HubException("Failed to update message status to 'Read'.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error marking message as read: {ex.Message}");
                throw new HubException("An error occurred while processing the request.", ex);
            }
        }   

        public async Task JoinChatRoom(int chatRoomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId.ToString());
            Console.WriteLine("User with ConnectionId " + Context.ConnectionId + " joined chat room ID: " + chatRoomId);
        }


        public async Task LeaveChatRoom(int chatRoomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId.ToString());
        }
    }
}
