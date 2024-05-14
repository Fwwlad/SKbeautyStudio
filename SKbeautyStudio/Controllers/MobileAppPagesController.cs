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
    public class MobileAppPagesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MobileAppPagesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MobileAppPages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MobileAppPages>>> GetMobileAppPages()
        {
          if (_context.MobileAppPages == null)
          {
              return NotFound();
          }
            return await _context.MobileAppPages.ToListAsync();
        }

        // GET: api/MobileAppPages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MobileAppPages>> GetMobileAppPages(int id)
        {
          if (_context.MobileAppPages == null)
          {
              return NotFound();
          }
            var mobileAppPages = await _context.MobileAppPages.FindAsync(id);

            if (mobileAppPages == null)
            {
                return NotFound();
            }

            return mobileAppPages;
        }

        // PUT: api/MobileAppPages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMobileAppPages(int id, MobileAppPages mobileAppPages)
        {
            if (id != mobileAppPages.Id)
            {
                return BadRequest();
            }

            _context.Entry(mobileAppPages).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MobileAppPagesExists(id))
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

        // POST: api/MobileAppPages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MobileAppPages>> PostMobileAppPages(MobileAppPages mobileAppPages)
        {
          if (_context.MobileAppPages == null)
          {
              return Problem("Entity set 'AppDbContext.MobileAppPages'  is null.");
          }
            _context.MobileAppPages.Add(mobileAppPages);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMobileAppPages", new { id = mobileAppPages.Id }, mobileAppPages);
        }

        // DELETE: api/MobileAppPages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMobileAppPages(int id)
        {
            if (_context.MobileAppPages == null)
            {
                return NotFound();
            }
            var mobileAppPages = await _context.MobileAppPages.FindAsync(id);
            if (mobileAppPages == null)
            {
                return NotFound();
            }

            _context.MobileAppPages.Remove(mobileAppPages);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MobileAppPagesExists(int id)
        {
            return (_context.MobileAppPages?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
