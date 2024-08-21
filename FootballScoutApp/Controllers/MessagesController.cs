using FootballScoutApp.Data;
using FootballScoutApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FootballScoutApp.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MessagesController(ApplicationDBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Messages/Inbox
        public async Task<IActionResult> Inbox()
        {
            var userId = _userManager.GetUserId(User);

            // Calculate unread messages count
            var unreadMessagesCount = await _context.Messages
                                                    .Where(m => m.ReceiverId == userId && !m.IsRead)
                                                    .CountAsync();

            // Store the count in ViewBag
            ViewBag.UnreadMessages = unreadMessagesCount;

            var messages = await _context.Messages
                                         .Where(m => m.ReceiverId == userId)
                                         .Include(m => m.Sender)
                                         .OrderByDescending(m => m.Timestamp)
                                         .ToListAsync();
            return View(messages);
        }

        // GET: Messages/Send
        public IActionResult Send()
        {
            return View();
        }

        // POST: Messages/Send
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(string receiverEmail, string content)
        {
            if (string.IsNullOrWhiteSpace(receiverEmail) || string.IsNullOrWhiteSpace(content))
            {
                ModelState.AddModelError(string.Empty, "Receiver and content cannot be empty.");
                return View();
            }

            var receiver = await _userManager.FindByEmailAsync(receiverEmail);
            if (receiver == null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return View();
            }

            var userId = _userManager.GetUserId(User);
            var message = new Message
            {
                SenderId = userId,
                ReceiverId = receiver.Id,
                Content = content,
                Timestamp = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Inbox));
        }

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var message = await _context.Messages
                                        .Include(m => m.Sender)
                                        .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null || message.ReceiverId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            // Mark the message as read
            if (!message.IsRead)
            {
                message.IsRead = true;
                _context.Update(message);
                await _context.SaveChangesAsync();
            }

            return View(message);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleImportant(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null) return NotFound();

            message.IsImportant = !message.IsImportant;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Inbox));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null) return NotFound();

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Inbox));
        }

    }
}
