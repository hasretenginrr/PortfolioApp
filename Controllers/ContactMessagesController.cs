using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioBackend.Data;
using PortfolioBackend.Entities;
using PortfolioBackend.Security;
using PortfolioBackend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactMessagesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IBackgroundJobClient _backgroundJobClient; 
        private readonly IEmailService _emailService;
        public ContactMessagesController(AppDbContext context,IBackgroundJobClient backgroundJobClient,
            IEmailService emailService)
        {
            _context = context;
            _backgroundJobClient = backgroundJobClient;
            _emailService = emailService;
        }

        // GET: api/ContactMessages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactMessages>>> GetContactMessages()
        {
            return await _context.ContactMessages.ToListAsync();
        }

        // GET: api/ContactMessages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactMessages>> GetContactMessages(int id)
        {
            var contactMessages = await _context.ContactMessages.FindAsync(id);

            if (contactMessages == null)
            {
                return NotFound();
            }

            return contactMessages;
        }

        // PUT: api/ContactMessages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ApiKeyAuthorize]
        public async Task<IActionResult> PutContactMessages(int id, ContactMessages contactMessages)
        {
            if (id != contactMessages.Id)
            {
                return BadRequest();
            }

            _context.Entry(contactMessages).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactMessagesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ContactMessages
        
        [HttpPost]
        public async Task<ActionResult<ContactMessages>> PostContactMessages(
            [FromBody] ContactMessages contactMessages) 
        {
           
            _context.ContactMessages.Add(contactMessages);
            await _context.SaveChangesAsync(); 

            _backgroundJobClient.Enqueue(
                () => _emailService.SendContactEmail(
                    contactMessages.Name,
                    contactMessages.Email,
                    contactMessages.Subject,
                    contactMessages.Message
                )
            );

            return CreatedAtAction("GetContactMessages", new { id = contactMessages.Id }, contactMessages);
        }

        // DELETE: api/ContactMessages/5
        [HttpDelete("{id}")]
        [ApiKeyAuthorize]
        public async Task<IActionResult> DeleteContactMessages(int id)
        {
            var contactMessages = await _context.ContactMessages.FindAsync(id);
            if (contactMessages == null)
            {
                return NotFound();
            }

            _context.ContactMessages.Remove(contactMessages);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContactMessagesExists(int id)
        {
            return _context.ContactMessages.Any(e => e.Id == id);
        }
    }
}
