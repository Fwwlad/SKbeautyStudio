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
    public class MaterialsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MaterialsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Materials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Materials>>> GetMaterials()
        {
          if (_context.Materials == null)
          {
              return NotFound();
          }
            return await _context.Materials.Select(m => new Materials
            {
                Id = m.Id,
                Name = m.Name,
                Color = m.Color,
                ExpirationDates = _context.ExpirationDates.Where(ed => ed.MaterialId == m.Id).Select(edn => new ExpirationDates
                {
                    StartDate = edn.StartDate,
                    EndDate = edn.EndDate,
                    PurchaseDate = edn.PurchaseDate,
                    DisposalDate = edn.DisposalDate
                }).ToList()
            }).ToListAsync();
        }

        // GET: api/Materials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Materials>> GetMaterials(int id)
        {
          if (_context.Materials == null)
          {
              return NotFound();
          }
            var materials = await _context.Materials.FindAsync(id);
            
            if (materials == null)
            {
                return NotFound();
            }
            materials = new Materials
            {
                Id = materials.Id,
                Name = materials.Name,
                Color = materials.Color,
                ExpirationDates = _context.ExpirationDates.Where(ed => ed.MaterialId == materials.Id).Select(edn => new ExpirationDates
                {
                    StartDate = edn.StartDate,
                    EndDate = edn.EndDate,
                    PurchaseDate = edn.PurchaseDate,
                    DisposalDate = edn.DisposalDate
                }).ToList()
            };
            return materials;
        }

        // PUT: api/Materials/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaterials(int id, Materials materials)
        {
            if (id != materials.Id)
            {
                return BadRequest();
            }

            _context.Entry(materials).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialsExists(id))
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

        // POST: api/Materials
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Materials>> PostMaterials(Materials materials)
        {
          if (_context.Materials == null)
          {
              return Problem("Entity set 'AppDbContext.Materials'  is null.");
          }
            _context.Materials.Add(materials);
            await _context.SaveChangesAsync();

            return NoContent();
        }   

        // DELETE: api/Materials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterials(int id)
        {
            if (_context.Materials == null)
            {
                return NotFound();
            }
            var materials = await _context.Materials.FindAsync(id);
            if (materials == null)
            {
                return NotFound();
            }

            _context.Materials.Remove(materials);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MaterialsExists(int id)
        {
            return (_context.Materials?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
