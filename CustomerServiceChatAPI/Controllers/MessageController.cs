using CustomerServiceChatAPI.Auth;
using CustomerServiceChatAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerServiceChatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MessageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Message
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] Messages message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            message.DateTime = DateTime.Now;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return Ok(message);
        }

        // GET: api/Message
        [HttpGet]
        public async Task<IActionResult> GetAllMessages()
        {
            var messages = await _context.Messages.ToListAsync();
            return Ok(messages);
        }

        // GET: api/Message/{senderName}/{receiverName}
        [HttpGet("{UserName}")]
        public async Task<IActionResult> GetMessagesBySenderAndReceiver(string UserName)
        {
            var messages = await _context.Messages
                .Where(m => (m.SenderName == UserName) ||
                            (m.ReceiverName == UserName))
                .OrderBy(m => m.DateTime)
                .ToListAsync();

            if (messages == null || !messages.Any())
            {
                return NotFound();
            }

            return Ok(messages);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
