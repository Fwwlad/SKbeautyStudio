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
    [Route("api/Employee/{EmployeeId:int}/photos")]
    [ApiController]
    public class PhotosOfEmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PhotosOfEmployeesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Employee/5/photos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetPhotosOfEmployees(int EmployeeId)
        {
          if (_context.PhotosOfEmployees == null)
          {
              return NotFound();
          }
            return await _context.PhotosOfEmployees.Where(x => x.EmployeeId == EmployeeId).Select(x => GetDataString(x)).ToListAsync();
        }
        private static string GetDataString(PhotosOfEmployee x)
        {
            using (FileStream fs = new FileStream(x.Source, FileMode.Open))
            {
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                return Convert.ToBase64String(data);
            }
        }

        // POST: api/Employee/5/photos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PhotosOfEmployee>> PostPhotosOfEmployee(int EmployeeId, string ImageBase64Data)
        {
            if (_context.PhotosOfEmployees == null)
            {
                return Problem("Entity set 'AppDbContext.PhotosOfEmployees'  is null.");
            }
            var photosOfEmployee = new PhotosOfEmployee { EmployeeId = EmployeeId };
            var a = _context.PhotosOfEmployees.Add(photosOfEmployee);
            
            
            string path = $"../PhotosOfEmployees/{a.Entity.Id}.png";
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                byte[] data = Convert.FromBase64String(ImageBase64Data);
                fs.Write(data, 0, data.Length);
            }
            photosOfEmployee.Source = path;
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetPhotosOfEmployee", new { id = photosOfEmployee.Id, photosOfEmployee });
        }

        /*// DELETE: api/PhotosOfEmployees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhotosOfEmployee(int id)
        {
            if (_context.PhotosOfEmployees == null)
            {
                return NotFound();
            }
            var photosOfEmployee = await _context.PhotosOfEmployees.FindAsync(id);
            if (photosOfEmployee == null)
            {
                return NotFound();
            }

            _context.PhotosOfEmployees.Remove(photosOfEmployee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PhotosOfEmployeeExists(int id)
        {
            return (_context.PhotosOfEmployees?.Any(e => e.Id == id)).GetValueOrDefault();
        }*/
    }
}
