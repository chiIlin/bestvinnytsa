using System.Security.Claims;
using System.Threading.Tasks;
using bestvinnytsa.web.Data.Services;
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

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // GET: /api/messages/chats
        [HttpGet("chats")]
        public async Task<IActionResult> GetChats()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var chats = await _messageService.GetChatsForUserAsync(userId);
            return Ok(chats);
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
    }

    public class SendMessageRequest
    {
        public string Text { get; set; } = null!;
    }
}