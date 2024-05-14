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
    public class EmployeesMobileAppPagesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesMobileAppPagesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/EmployeesMobileAppPages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeesMobileAppPages>>> GetEmployeesMobileAppPages()
        {
          if (_context.EmployeesMobileAppPages == null)
          {
              return NotFound();
          }
            return await _context.EmployeesMobileAppPages.Select(emap => new EmployeesMobileAppPages
            {
                EmployeeId = emap.EmployeeId,
                MobileAppPageId = emap.MobileAppPageId,
                CanAdd = emap.CanAdd,
                CanDelete = emap.CanDelete,
                CanEdit = emap.CanEdit,
                CanView = emap.CanView,
                Employees = _context.Employees.Where(e => e.Id == emap.EmployeeId).Select(e => new Employees
                {
                    Id = e.Id,
                    Name = e.Name,
                    Surname = e.Surname,
                    Patronymic = e.Patronymic,
                    Gender = e.Gender,
                    Phone = e.Phone,
                    Birthday = e.Birthday,
                    DateOfHire = e.DateOfHire,
                    Email = e.Email,
                    SalaryPercent = e.SalaryPercent,
                }).FirstOrDefault(),
                MobileAppPage = _context.MobileAppPages.Where(map => map.Id == emap.MobileAppPageId).FirstOrDefault()
            }).ToListAsync();
        }

        // GET: api/EmployeesMobileAppPages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeesMobileAppPages>> GetEmployeesMobileAppPages(int id)
        {
          if (_context.EmployeesMobileAppPages == null)
          {
              return NotFound();
          }
            var employeesMobileAppPages = await _context.EmployeesMobileAppPages.FindAsync(id);

            if (employeesMobileAppPages == null)
            {
                return NotFound();
            }
            employeesMobileAppPages.Employees = _context.Employees.Where(e => e.Id == employeesMobileAppPages.EmployeeId).Select(e => new Employees
            {
                Id = e.Id,
                Name = e.Name,
                Surname = e.Surname,
                Patronymic = e.Patronymic,
                Gender = e.Gender,
                Phone = e.Phone,
                Birthday = e.Birthday,
                DateOfHire = e.DateOfHire,
                Email = e.Email,
                SalaryPercent = e.SalaryPercent,
            }).FirstOrDefault();
            employeesMobileAppPages.MobileAppPage = _context.MobileAppPages.Where(map => map.Id == employeesMobileAppPages.MobileAppPageId).FirstOrDefault();
            return employeesMobileAppPages;
        }

        // PUT: api/EmployeesMobileAppPages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{EmployeeId}/{MobileAppPgaId}")]
        public async Task<IActionResult> PutEmployeesMobileAppPages(int EmployeeId, int MobileAppPgaId, EmployeesMobileAppPages employeesMobileAppPages)
        {
            if (EmployeeId != employeesMobileAppPages.EmployeeId || MobileAppPgaId != employeesMobileAppPages.MobileAppPageId)
            {
                return BadRequest();
            }

            _context.Entry(employeesMobileAppPages).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeesMobileAppPagesExists(EmployeeId))
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

        // POST: api/EmployeesMobileAppPages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeesMobileAppPages>> PostEmployeesMobileAppPages(EmployeesMobileAppPages employeesMobileAppPages)
        {
          if (_context.EmployeesMobileAppPages == null)
          {
              return Problem("Entity set 'AppDbContext.EmployeesMobileAppPages'  is null.");
          }
            _context.EmployeesMobileAppPages.Add(employeesMobileAppPages);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EmployeesMobileAppPagesExists(employeesMobileAppPages.EmployeeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEmployeesMobileAppPages", new { id = employeesMobileAppPages.EmployeeId }, employeesMobileAppPages);
        }

        // DELETE: api/EmployeesMobileAppPages/5
        [HttpDelete("{EmployeeId}/{MobileAppPageId}")]
        public async Task<IActionResult> DeleteEmployeesMobileAppPages(int EmployeeId, int MobileAppPageId)
        {
            if (_context.EmployeesMobileAppPages == null)
            {
                return NotFound();
            }
            var employeesMobileAppPages = await _context.EmployeesMobileAppPages.Where(emap => emap.EmployeeId == EmployeeId && emap.MobileAppPageId == MobileAppPageId).FirstOrDefaultAsync();
            if (employeesMobileAppPages == null)
            {
                return NotFound();
            }

            _context.EmployeesMobileAppPages.Remove(employeesMobileAppPages);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeesMobileAppPagesExists(int id)
        {
            return (_context.EmployeesMobileAppPages?.Any(e => e.EmployeeId == id)).GetValueOrDefault();
        }
    }
}
