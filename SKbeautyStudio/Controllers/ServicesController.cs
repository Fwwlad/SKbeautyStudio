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
    public class ServicesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Services
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Services>>> GetServices()
        {
          if (_context.Services == null)
          {
              return NotFound();
          }
            try
            {
            return await _context.Services.Select(s => new Services
            {
                Id = s.Id,
                Name = s.Name,
                BaseCost = s.BaseCost,
                BaseTimeMinutes = s.BaseTimeMinutes,
                CategoryId = s.CategoryId,
                Category = _context.Categories.Where(c => c.Id == s.CategoryId).FirstOrDefault()
            }).ToListAsync();
            } catch(Exception ex)
            {
                return NotFound(ex.Message + '\n' + ex.Source + '\n' + ex.InnerException + '\n' + ex.HelpLink);
                
            }
        }

        // GET: api/Services/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Services>> GetServices(int id)
        {
          if (_context.Services == null)
          {
              return NotFound();
          }
            var services = await _context.Services.FindAsync(id);

            if (services == null)
            {
                return NotFound();
            }
            services.Category = _context.Categories.Select(c => new Categories
            {
                Id = c.Id,
                Name = c.Name,
                UIColor = c.UIColor,
                JobName = c.JobName
            }).ToList().Find(c => c.Id == services.CategoryId);
            return services;
        }

        // PUT: api/Services/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServices(int id, Services services)
        {
            if (id != services.Id)
            {
                return BadRequest();
            }

            _context.Entry(services).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicesExists(id))
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

        // POST: api/Services
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Services>> PostServices(Services services)
        {
          if (_context.Services == null)
          {
              return Problem("Entity set 'AppDbContext.Services'  is null.");
          }
            _context.Services.Add(services);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServices", new { id = services.Id }, services);
        }

        // DELETE: api/Services/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServices(int id)
        {
            if (_context.Services == null)
            {
                return NotFound();
            }
            var services = await _context.Services.FindAsync(id);
            if (services == null)
            {
                return NotFound();
            }

            _context.Services.Remove(services);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServicesExists(int id)
        {
            return (_context.Services?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
