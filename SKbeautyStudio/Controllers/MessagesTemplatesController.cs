using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKbeautyStudio.Db;

namespace SKbeautyStudio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesTemplatesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MessagesTemplatesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MessagesTemplates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessagesTemplates>>> GetMessagesTemplates()
        {
          if (_context.MessagesTemplates == null)
          {
              return NotFound();
          }
            return await _context.MessagesTemplates.Select(mt => new MessagesTemplates
            {
                Id = mt.Id,
                Before = mt.Before,
                CategoriesId = mt.CategoriesId,
                HoursCount = mt.HoursCount,
                Text = mt.Text,
                TimeStamp = mt.TimeStamp,
                Category = new Categories
                {
                    Id = mt.Category.Id,
                    UIColor = mt.Category.UIColor,
                    Name = mt.Category.Name,
                    JobName = mt.Category.JobName
                }
            }).ToListAsync();
        }

        // GET: api/MessagesTemplates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MessagesTemplates>> GetMessagesTemplates(int id)
        {
            if (_context.MessagesTemplates == null)
            {
                return NotFound();
            }
            var messagesTemplates = await _context.MessagesTemplates.FindAsync(id);

            if (messagesTemplates == null)
            {
                return NotFound();
            }
            
            messagesTemplates.Category = _context.Categories.Where(c => c.Id == messagesTemplates.CategoriesId).Select(c => new Categories
            {
                Id = c.Id,
                Name = c.Name,
                UIColor = c.UIColor,
                JobName = c.JobName
            }).FirstOrDefault();
            
            return messagesTemplates;
        }

        // PUT: api/MessagesTemplates/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessagesTemplates(int id, MessagesTemplates messagesTemplates)
        {
            if (id != messagesTemplates.Id)
            {
                return BadRequest();
            }

            _context.Entry(messagesTemplates).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessagesTemplatesExists(id))
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

        // POST: api/MessagesTemplates
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MessagesTemplates>> PostMessagesTemplates(MessagesTemplates messagesTemplates)
        {
          if (_context.MessagesTemplates == null)
          {
              return Problem("Entity set 'AppDbContext.MessagesTemplates'  is null.");
          }
            _context.MessagesTemplates.Add(messagesTemplates);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessagesTemplates", new { id = messagesTemplates.Id }, messagesTemplates);
        }

        // DELETE: api/MessagesTemplates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessagesTemplates(int id)
        {
            if (_context.MessagesTemplates == null)
            {
                return NotFound();
            }
            var messagesTemplates = await _context.MessagesTemplates.FindAsync(id);
            if (messagesTemplates == null)
            {
                return NotFound();
            }

            _context.MessagesTemplates.Remove(messagesTemplates);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MessagesTemplatesExists(int id)
        {
            return (_context.MessagesTemplates?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
