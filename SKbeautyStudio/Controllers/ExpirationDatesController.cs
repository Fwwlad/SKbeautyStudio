using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKbeautyStudio.Db;

namespace SKbeautyStudio.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpirationDatesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExpirationDatesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/materials/ExpirationDates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpirationDates>>> GetExpirationDates()
        {
          if (_context.ExpirationDates == null)
          {
              return NotFound();
          }
            return await _context.ExpirationDates.Select(e => new ExpirationDates
            {
                Id = e.Id,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                PurchaseDate = e.PurchaseDate,
                DisposalDate = e.DisposalDate,
                MaterialId = e.MaterialId,
                Material = _context.Materials.Where(m => m.Id == e.MaterialId).FirstOrDefault()
            }).ToListAsync();
        }

        // GET: api/materials/ExpirationDates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpirationDates>> GetExpirationDates(int id)
        {
          if (_context.ExpirationDates == null)
          {
              return NotFound();
          }
            var expirationDates = await _context.ExpirationDates.FindAsync(id);

            if (expirationDates == null)
            {
                return NotFound();
            }

            return expirationDates;
        }

        // PUT: api/materials/ExpirationDates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpirationDates(int id, ExpirationDates expirationDates)
        {
            if (id != expirationDates.Id)
            {
                return BadRequest();
            }

            _context.Entry(expirationDates).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpirationDatesExists(id))
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

        // POST: api/materials/ExpirationDates
        [HttpPost]
        public async Task<ActionResult<ExpirationDates>> PostExpirationDates(ExpirationDates expirationDates)
        {
          if (_context.ExpirationDates == null)
          {
              return Problem("Entity set 'AppDbContext.ExpirationDates'  is null.");
          }
            _context.ExpirationDates.Add(expirationDates);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExpirationDates", new { id = expirationDates.Id }, expirationDates);
        }

        // DELETE: api/materials/ExpirationDates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpirationDates(int id)
        {
            if (_context.ExpirationDates == null)
            {
                return NotFound();
            }
            var expirationDates = await _context.ExpirationDates.FindAsync(id);
            if (expirationDates == null)
            {
                return NotFound();
            }

            _context.ExpirationDates.Remove(expirationDates);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExpirationDatesExists(int id)
        {
            return (_context.ExpirationDates?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
