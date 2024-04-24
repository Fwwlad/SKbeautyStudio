﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKbeautyStudio.Db;
using Newtonsoft.Json.Linq;
using System.Text;

namespace SKbeautyStudio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employees>>> GetEmployees()
        {
          if (_context.Employees == null)
          {
              return NotFound();
          }
            return await _context.Employees.Select(e => new Employees
            {
                Id = e.Id, 
                Surname = e.Surname,
                Name = e.Name,
                Patronymic = e.Patronymic,
                Phone = e.Phone,
                Birthday = e.Birthday,
                DateOfHire = e.DateOfHire,
                Gender = e.Gender,
                Email = e.Email,
                SalaryPercent = e.SalaryPercent
            }).ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employees>> GetEmployees(int id)
        {
          if (_context.Employees == null)
          {
              return NotFound();
          }
            var employees = await _context.Employees.FindAsync(id);

            if (employees == null)
            {
                return NotFound();
            }

            return employees;
        }
        
        // GET: api/Employees/5/photos
        [HttpGet("{id}/photos")]
        public async Task<ActionResult<List<string>>> GetEmployePhotos(int id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employees = await _context.Employees.FindAsync(id);

            if (employees == null)
            {
                return NotFound();
            }

            List<string> result = new List<string>();
            await _context.photosOfEmployees.Where(p => p.EmployeeId == id).ForEachAsync(async photo =>
                 {
                        using (System.IO.FileStream fs = System.IO.File.OpenRead(photo.Source))
                        {
                            byte[] buffer = new byte[fs.Length];
                            await fs.ReadAsync(buffer, 0, buffer.Length);
                            result.Add(Convert.ToBase64String(buffer));
                    
                        }
                  });

            return result;
        }
        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("photo")]
        public async Task<ActionResult<List<string>>> PostEmployees(PhotosOfEmployees photo)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'AppDbContext.Employees'  is null.");
            }
            string source = photo.Source;
            photo.Source = "PhotosOfEmployees/" + photo.EmployeeId + "_" + (_context.photosOfEmployees.Max(p => p.Id) + 1) + ".png";
            using (FileStream fs = new FileStream(photo.Source, FileMode.OpenOrCreate))
            {
                byte[] buffer = Convert.FromBase64String(source);
                await fs.WriteAsync(buffer, 0, buffer.Length);
            }
            _context.photosOfEmployees.Add(photo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployePhotos", new { id = photo.EmployeeId }, photo);
        }
        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployees(int id, Employees employees)
        {
            if (id != employees.Id)
            {
                return BadRequest();
            }

            _context.Entry(employees).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeesExists(id))
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

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employees>> PostEmployees(Employees employees)
        {
          if (_context.Employees == null)
          {
              return Problem("Entity set 'AppDbContext.Employees'  is null.");
          }
            _context.Employees.Add(employees);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployees", new { id = employees.Id }, employees);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployees(int id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employees = await _context.Employees.FindAsync(id);
            if (employees == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employees);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeesExists(int id)
        {
            return (_context.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
