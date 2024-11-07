using DATN.MVC.Helpers;
using DATN.MVC.Models.Request.Chat;
using DATN.MVC.Request.Message;
using DATN.MVC.Respone.Chat;
using DATN.MVC.Respone.Message;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DATN.MVC.Hubs
{
    public class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        // Method to handle a user disconnecting from the hub
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        // Method to create a chat room
        public async Task CreateRoom(Chat_CreateReq req)
        {
            var existungChatRoom = ApiHelpers.PostMethodAsync<Chat_ReadRes, Chat_CreateReq>("https://localhost:7296/api/ChatRoom/find-chatRoom", req);
            if (existungChatRoom != null)
            {
                // Notify the caller that the room was successfully created and return the roomId
                await Clients.Caller.SendAsync("RoomCreated", existungChatRoom.RoomId);
            }
            else
            {
                var createNew = ApiHelpers.PostMethodAsync<Chat_ReadRes, Chat_CreateReq>("https://localhost:7296/api/ChatRoom/create-chatRoom", req);
                if (createNew != null)
                {
                    await Clients.Caller.SendAsync("RoomCreated", createNew.RoomId);
                }
                else
                {
                    throw new Exception("Failed to create chat room.");
                }
            }
        }

        // Method to join a chat room
        public async Task JoinRoom(int roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
            Console.WriteLine($"User {Context.UserIdentifier} joined room {roomId}");
        }

        // Method to load messages from a specific chat room
        public async Task LoadMessages(int roomId)
        {
            // Load messages only if this is an existing room with prior messages
            var response =  ApiHelpers.GetMethod<List<Message_ReadRes>>($"https://localhost:7296/api/ChatRoom/getMessagesInChatRoom/{roomId}");

            if (response != null && response.Count > 0)
            {
                // Existing chat with messages
                await Clients.Caller.SendAsync("MessagesLoaded", response);
            }
            else
            {
                // New chat with no messages, return an empty list
                await Clients.Caller.SendAsync("MessagesLoaded", new List<Message_ReadRes>());
            }
        }

        // Method to send a message in a chat room
        public async Task SendMessage(Message_SendMessageReq messageDto)
        {
            var response = ApiHelpers.PostMethodAsync<bool, Message_SendMessageReq>("https://localhost:7296/api/messages/sendMessage", messageDto);

            if (response)
            {
                await Clients.Group(messageDto.RoomId.ToString()).SendAsync("ReceiveMessage", messageDto);
            }
            else
            {
                throw new Exception("Failed to send message via API.");
            }
        }
    }
}
