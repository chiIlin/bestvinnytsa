using System.Security.Claims;
using System.Threading.Tasks;
using bestvinnytsa.web.Data.Services;
using bestvinnytsa.web.Data.Models; // ДОДАТИ ЦЕЙ USING
using bestvinnytsa.web.Data.Mongo;  // ДОДАТИ ЦЕЙ USING
using MongoDB.Driver; // ДОДАЙТЕ ЦЕЙ USING
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bestvinnytsa.web.Controllers
{
    [ApiController]
    [Route("api/messages")]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly MongoContext _mongoContext; // ДОДАТИ ПОЛЕ

        public MessagesController(IMessageService messageService, MongoContext mongoContext) // ДОДАТИ ПАРАМЕТР
        {
            _messageService = messageService;
            _mongoContext = mongoContext; // ІНІЦІАЛІЗУВАТИ
        }

        // GET: /api/messages/chats
        [HttpGet("chats")]
        public async Task<IActionResult> GetChats()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var chats = await _messageService.GetChatsForUserAsync(userId);

            // Додай це для дебагу:
            Console.WriteLine($"userId: {userId}");
            Console.WriteLine($"Chats found: {chats.Count}");
            foreach (var chat in chats)
                Console.WriteLine($"Chat: {chat.Id}, Participants: {string.Join(",", chat.Participants)}");

            // Додаємо ім'я та аватар співрозмовника
            var usersCollection = _mongoContext.Users;
            var chatDtos = new List<object>();
            foreach (var chat in chats)
            {
                var otherId = chat.Participants.FirstOrDefault(id => id != userId);
                var otherUser = otherId != null
                    ? await usersCollection.Find(u => u.Id == otherId).FirstOrDefaultAsync()
                    : null;

                chatDtos.Add(new
                {
                    id = chat.Id,
                    participants = chat.Participants,
                    lastMessage = chat.LastMessage,
                    lastMessageTime = chat.LastMessageTime,
                    unreadCount = chat.UnreadCount.ContainsKey(userId) ? chat.UnreadCount[userId] : 0,
                    participantName = otherUser?.FullName ?? otherUser?.CompanyName ?? "Співрозмовник",
                    participantAvatar = otherUser?.PhotoUrl ?? "",
                    participantType = otherUser?.Roles.Contains("Company") == true ? "brand" : "influencer",
                    isOnline = false // якщо треба, додай логику онлайн-статусу
                });
            }

            return Ok(chatDtos);
        }

        // GET: /api/messages/{chatId}
        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetMessages(string chatId)
        {
            var messages = await _messageService.GetMessagesForChatAsync(chatId);
            return Ok(messages);
        }

        // POST: /api/messages/{chatId}
        [HttpPost("{chatId}")]
        public async Task<IActionResult> SendMessage(string chatId, [FromBody] SendMessageRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var message = await _messageService.SendMessageAsync(chatId, userId, request.Text);
            return Ok(message);
        }

        // POST: /api/messages/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // Перевіряємо, чи вже існує чат між цими двома користувачами
            var chat = await _mongoContext.Chats
                .Find(c => c.Participants.Count == 2 &&
                           c.Participants.Contains(userId) &&
                           c.Participants.Contains(request.ParticipantId))
                .FirstOrDefaultAsync();
            if (chat == null)
            {
                // Створюємо новий чат
                chat = new Chat
                {
                    Participants = new List<string> { userId, request.ParticipantId },
                    LastMessage = "",
                    LastMessageTime = null,
                    UnreadCount = new Dictionary<string, int>()
                };
                await _mongoContext.Chats.InsertOneAsync(chat);
            }

            // Додаємо ім'я та аватар співрозмовника (як у GetChats)
            var otherId = chat.Participants.FirstOrDefault(id => id != userId);
            var usersCollection = _mongoContext.Users;
            var otherUser = otherId != null
                ? await usersCollection.Find(u => u.Id == otherId).FirstOrDefaultAsync()
                : null;

            var chatDto = new
            {
                id = chat.Id,
                participants = chat.Participants,
                lastMessage = chat.LastMessage,
                lastMessageTime = chat.LastMessageTime,
                unreadCount = chat.UnreadCount.ContainsKey(userId) ? chat.UnreadCount[userId] : 0,
                participantName = otherUser?.FullName ?? otherUser?.CompanyName ?? "Співрозмовник",
                participantAvatar = otherUser?.PhotoUrl ?? "",
                participantType = otherUser?.Roles.Contains("Company") == true ? "brand" : "influencer",
                isOnline = false
            };

            Console.WriteLine($"Створюємо чат для: {userId} і {request.ParticipantId}");
            Console.WriteLine($"Чат створено з id: {chat.Id}");

            return Ok(chatDto);
        }
    }

    public class SendMessageRequest
    {
        public string Text { get; set; } = null!;
    }

    public class CreateChatRequest
    {
        public string ParticipantId { get; set; } = null!;
    }
}