using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using bestvinnytsa.web.Data.Models;
using bestvinnytsa.web.Data.Mongo;
using MongoDB.Driver;
using System.Security.Claims;

namespace bestvinnytsa.web.Data.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMongoCollection<Chat> _chats;
        private readonly IMongoCollection<Message> _messages;

        public MessageService(MongoContext context)
        {

            _chats = context.Chats;
            _messages = context.Messages;
        }

        public async Task<List<Chat>> GetChatsForUserAsync(string userId)
        {
            return await _chats.Find(c => c.Participants.Contains(userId)).ToListAsync();
        }

        public async Task<List<Message>> GetMessagesForChatAsync(string chatId)
        {
            return await _messages.Find(m => m.ChatId == chatId).SortBy(m => m.Timestamp).ToListAsync();
        }

        public async Task<Message> SendMessageAsync(string chatId, string senderId, string text)
        {
            var message = new Message
            {
                ChatId = chatId,
                SenderId = senderId,
                Text = text,
                Timestamp = DateTime.UtcNow,
                IsRead = false
            };
            await _messages.InsertOneAsync(message);

            // Оновити останнє повідомлення в чаті
            var update = Builders<Chat>.Update
                .Set(c => c.LastMessage, text)
                .Set(c => c.LastMessageTime, message.Timestamp);
            await _chats.UpdateOneAsync(c => c.Id == chatId, update);

            return message;
        }
    }
}