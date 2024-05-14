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
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categories>>> GetCategories()
        {
          if (_context.Categories == null)
          {
              return NotFound();
          }
            try
            {
                return await _context.Categories.Select(c => new Categories
                {
                    Id = c.Id,
                    Name = c.Name,
                    UIColor = c.UIColor,
                    JobName = c.JobName,
                    Services = _context.Services.Where(s => s.CategoryId == c.Id)
                                                .Select(s => new Services
                                                {
                                                    Id = s.Id,
                                                    Name = s.Name,
                                                    CategoryId = s.CategoryId,
                                                    BaseCost = s.BaseCost,
                                                    BaseTimeMinutes = s.BaseTimeMinutes
                                                }).ToArray(),
                    MessagesTemplates = _context.MessagesTemplates.Where(mt => mt.CategoriesId == c.Id).ToArray(),
                    EmployeesJobsTitles = _context.EmployeesJobTitles.Where(ejt => ejt.CategoriesId == c.Id).ToArray()
                }).ToListAsync();
            } catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Categories>> GetCategories(int id)
        {
          if (_context.Categories == null)
          {
              return NotFound();
          }
            var categories = await _context.Categories.FindAsync(id);

            if (categories == null)
            {
                return NotFound();
            }
            categories.Services = _context.Services.Where(s => s.CategoryId == categories.Id).Select(s => new Services
            {
                Id = s.Id,
                Name = s.Name,
                CategoryId = s.CategoryId,
                BaseCost = s.BaseCost,
                BaseTimeMinutes = s.BaseTimeMinutes
            }).ToArray();
            categories.MessagesTemplates = _context.MessagesTemplates.Where(mt => mt.CategoriesId == categories.Id).ToArray();
            categories.EmployeesJobsTitles = _context.EmployeesJobTitles.Where(ejt => ejt.CategoriesId == categories.Id).ToArray();
            return categories;
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategories(int id, Categories categories)
        {
            if (id != categories.Id)
            {
                return BadRequest();
            }

            _context.Entry(categories).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriesExists(id))
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

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Categories>> PostCategories(Categories categories)
        {
          if (_context.Categories == null)
          {
              return Problem("Entity set 'AppDbContext.Categories'  is null.");
          }
            _context.Categories.Add(categories);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategories", new { id = categories.Id }, categories);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategories(int id)
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }
            var categories = await _context.Categories.FindAsync(id);
            if (categories == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(categories);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("{id}/messages")]
        public async Task<ActionResult<IEnumerable<MessagesTemplates>>> GetMessages(int id)
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }
            var messages = await _context.MessagesTemplates.Where(mt => mt.CategoriesId == id).Select(m => new MessagesTemplates
            {
                Id = m.Id,
                CategoriesId = m.CategoriesId,
                Before = m.Before,
                HoursCount = m.HoursCount,
                Text = m.Text,
                TimeStamp = m.TimeStamp
            }).ToListAsync();

            if (messages == null)
            {
                return NotFound();
            }

            return messages;
        }

        private bool CategoriesExists(int id)
        {
            return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
