using System.Collections.Generic;
using System.Threading.Tasks;
using bestvinnytsa.web.Data.Models;

namespace bestvinnytsa.web.Data.Services
{
    public interface IMessageService
    {
        Task<List<Chat>> GetChatsForUserAsync(string userId);
        Task<List<Message>> GetMessagesForChatAsync(string chatId);
        Task<Message> SendMessageAsync(string chatId, string senderId, string text);
    }
}