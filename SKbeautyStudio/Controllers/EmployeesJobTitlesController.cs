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
    public class EmployeesJobTitlesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesJobTitlesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/EmployeesJobTitles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeesJobTitles>>> GetEmployeesJobTitles()
        {
          if (_context.EmployeesJobTitles == null)
          {
              return NotFound();
          }
            return await _context.EmployeesJobTitles.Select(ejt => new EmployeesJobTitles
            {
                CategoriesId = ejt.CategoriesId,
                EmployeesId = ejt.EmployeesId,
                Categories = _context.Categories.Where(c => c.Id == ejt.CategoriesId).Select(c => new Categories
                {
                    Id = c.Id,
                    UIColor = c.UIColor,
                    JobName = c.JobName,
                    Name = c.Name
                }).FirstOrDefault(),
                Employees = _context.Employees.Where(e => e.Id == ejt.EmployeesId).Select(e => new Employees
                {
                    Id = e.Id,
                    Birthday = e.Birthday,
                    DateOfHire = e.DateOfHire,
                    Email = e.Email,
                    Gender = e.Gender,
                    Name = e.Name,
                    Phone = e.Phone,
                    Patronymic = e.Patronymic,
                    SalaryPercent = e.SalaryPercent,
                    Surname = e.Surname
                }).FirstOrDefault()
            }).ToListAsync();
        }

        // GET: api/EmployeesJobTitles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeesJobTitles>> GetEmployeesJobTitles(int id)
        {
          if (_context.EmployeesJobTitles == null)
          {
              return NotFound();
          }
            var employeesJobTitles = await _context.EmployeesJobTitles.FindAsync(id);
            if (employeesJobTitles == null)
            {
                return NotFound();
            }
            employeesJobTitles.Categories = _context.Categories.Where(c => c.Id == employeesJobTitles.CategoriesId).Select(c => new Categories
            {
                Id = c.Id,
                UIColor = c.UIColor,
                JobName = c.JobName,
                Name = c.Name
            }).FirstOrDefault();
            employeesJobTitles.Employees = _context.Employees.Where(e => e.Id == employeesJobTitles.EmployeesId).Select(e => new Employees
            {
                Id = e.Id,
                Birthday = e.Birthday,
                DateOfHire = e.DateOfHire,
                Email = e.Email,
                Gender = e.Gender,
                Name = e.Name,
                Phone = e.Phone,
                Patronymic = e.Patronymic,
                SalaryPercent = e.SalaryPercent,
                Surname = e.Surname
            }).FirstOrDefault();
            return employeesJobTitles;
        }

        // PUT: api/EmployeesJobTitles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{EmployeeId}/{CategoryId}")]
        public async Task<IActionResult> PutEmployeesJobTitles(int EmployeeId, int CategoryId, EmployeesJobTitles employeesJobTitles)
        {
            if (EmployeeId != employeesJobTitles.EmployeesId || CategoryId != employeesJobTitles.CategoriesId)
            {
                return BadRequest();
            }

            _context.Entry(employeesJobTitles).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeesJobTitlesExists(EmployeeId))
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

        // POST: api/EmployeesJobTitles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeesJobTitles>> PostEmployeesJobTitles(EmployeesJobTitles employeesJobTitles)
        {
          if (_context.EmployeesJobTitles == null)
          {
              return Problem("Entity set 'AppDbContext.EmployeesJobTitles'  is null.");
          }
            _context.EmployeesJobTitles.Add(employeesJobTitles);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EmployeesJobTitlesExists(employeesJobTitles.EmployeesId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEmployeesJobTitles", new { id = employeesJobTitles.EmployeesId }, employeesJobTitles);
        }

        // DELETE: api/EmployeesJobTitles/5
        [HttpDelete("{EmployeeId}/{CategoriesId}")]
        public async Task<IActionResult> DeleteEmployeesJobTitles(int EmployeeId, int CategoriesId)
        {
            if (_context.EmployeesJobTitles == null)
            {
                return NotFound();
            }
            var employeesJobTitles = await _context.EmployeesJobTitles.Where(ejt => ejt.EmployeesId == EmployeeId && ejt.CategoriesId == CategoriesId).FirstOrDefaultAsync();
            if (employeesJobTitles == null)
            {
                return NotFound();
            }

            _context.EmployeesJobTitles.Remove(employeesJobTitles);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeesJobTitlesExists(int id)
        {
            return (_context.EmployeesJobTitles?.Any(e => e.EmployeesId == id)).GetValueOrDefault();
        }
    }
}
