using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKbeautyStudio.Db;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Security.Cryptography;

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
            try
            {
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
                SalaryPercent = e.SalaryPercent,
                EmployeeMobileAppPages = _context.EmployeesMobileAppPages
                                        .Where(emap => emap.EmployeeId == e.Id)
                                        .Select(emap => new EmployeesMobileAppPages
                                        {
                                            EmployeeId = emap.EmployeeId,
                                            MobileAppPageId = emap.MobileAppPageId,
                                            CanView = emap.CanView,
                                            CanAdd = emap.CanAdd,
                                            CanDelete = emap.CanDelete,
                                            CanEdit = emap.CanEdit,
                                            Employees = null,
                                            MobileAppPage = _context.MobileAppPages.Where(map => map.Id == emap.MobileAppPageId).FirstOrDefault()
                                        }).ToArray(),
                AvailableCategories = _context.EmployeesJobTitles
                                      .Where(ejt => ejt.EmployeesId == e.Id).ToArray()
            }).ToListAsync();
        } catch(Exception ex)
            {
                return NotFound(ex.Message + '\n' + ex.Source + '\n' + ex.InnerException + '\n' + ex.HelpLink);
            }
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

            employees.EmployeeMobileAppPages = _context.EmployeesMobileAppPages
                                        .Where(emap => emap.EmployeeId == employees.Id)
                                        .Select(emap => new EmployeesMobileAppPages
                                        {
                                            EmployeeId = emap.EmployeeId,
                                            MobileAppPageId = emap.MobileAppPageId,
                                            CanView = emap.CanView,
                                            CanAdd = emap.CanAdd,
                                            CanDelete = emap.CanDelete,
                                            CanEdit = emap.CanEdit,
                                            Employees = null,
                                            MobileAppPage = _context.MobileAppPages.Where(map => map.Id == emap.MobileAppPageId).FirstOrDefault()
                                        }).ToList();
            employees.AvailableCategories = _context.EmployeesJobTitles.Where(ejt => ejt.EmployeesId == employees.Id).ToArray();
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
        [HttpPost("rights")]
        public async Task<ActionResult<bool>> PostEmployees(EmployeesMobileAppPages rights)
        {

            if (_context.Employees == null)
            {
                return Problem("Entity set 'AppDbContext.Employees'  is null.");
            }
            try
            {
                _context.EmployeesMobileAppPages.Add(rights);
                await _context.SaveChangesAsync();
            } catch(Exception ex)
            {
                return false;
            }


            return true;
        }
        [HttpPost("job")]
        public async Task<ActionResult<bool>> PostEmployees(EmployeesJobTitles jobTitles)
        {

            if (_context.Employees == null)
            {
                return Problem("Entity set 'AppDbContext.Employees'  is null.");
            }
            try
            {
                _context.EmployeesJobTitles.Add(jobTitles);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        [HttpPut("{id}/rights")]
        public async Task<IActionResult> PutEmployees(int id, EmployeesMobileAppPages rights)
        {
            if (id != rights.EmployeeId)
            {
                return BadRequest();
            }

            _context.Entry(rights).State = EntityState.Modified;

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
        [HttpGet("{login}/password/validate/{password}")]
        public async Task<ActionResult<ICollection<EmployeesMobileAppPages>>> CheckPassword(string login, string password)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            EmployeesPasswords? account = await _context.EmployeesPasswords.FindAsync(login);
            if (account is null)
            {
                return NotFound();
            }
            
            if (validatePassword(account, password))
            {
                var rights = await _context.EmployeesMobileAppPages.Where(emap => emap.EmployeeId == account.EmployeeId).Select(
                    emap => new EmployeesMobileAppPages
                    {
                        EmployeeId = emap.EmployeeId,
                        MobileAppPageId = emap.MobileAppPageId,
                        CanAdd = emap.CanAdd,
                        CanDelete = emap.CanDelete,
                        CanEdit = emap.CanEdit,
                        CanView = emap.CanView,
                        Employees = _context.Employees.Where(e => e.Id == emap.EmployeeId).FirstOrDefault(),
                        MobileAppPage = _context.MobileAppPages.Where(map => map.Id == emap.MobileAppPageId).FirstOrDefault()
                    }
                ).ToListAsync();
                
                return rights is null ? NoContent() : rights;
            }


            return NoContent();
        }
        [HttpPost("{id}/password")]
        public async Task<IActionResult> SetPassword(int id, string login, string password)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'AppDbContext.Employees'  is null.");
            }
            if(await _context.Employees.FindAsync(id) == null)
            {
                return NotFound();
            }
            if(_context.EmployeesPasswords.Where(ep => ep.EmployeeId == id).Count() > 0)
            {
                return Problem("The password has already been set");
            }
            
            byte[] tmpSource;
            byte[] tmpHash;

            using(SHA256 hash = SHA256.Create())
            {
                tmpSource = Encoding.UTF8.GetBytes(password);
                tmpHash =  hash.ComputeHash(tmpSource);
            }

            
            StringBuilder sOutput = new StringBuilder(tmpHash.Length);
            for (int i = 0; i < tmpHash.Length; i++)
            {
                sOutput.Append(tmpHash[i].ToString("X2"));
            }

            var employeePassword = new EmployeesPasswords
            {
                Login = login,
                EmployeeId = id,
                Password = sOutput.ToString()
            };
            _context.EmployeesPasswords.Add(employeePassword);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPut("{id}/password")]
        public async Task<IActionResult> UpdatePassword(int id, string newPassword)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            if (_context.EmployeesPasswords.Where(ep => ep.EmployeeId == id).Count() == 0)
            {
                return NotFound();
            }
            if (await _context.Employees.FindAsync(id) == null)
            {
                return NotFound();
            }

            byte[] tmpSource;
            byte[] tmpHash;

            using (SHA256 hash = SHA256.Create())
            {
                tmpSource = Encoding.UTF8.GetBytes(newPassword);
                tmpHash = hash.ComputeHash(tmpSource);
            }

            StringBuilder sOutput = new StringBuilder(tmpHash.Length);
            for (int i = 0; i < tmpHash.Length; i++)
            {
                sOutput.Append(tmpHash[i].ToString("X2"));
            }

            EmployeesPasswords employeePassword = _context.EmployeesPasswords.Where(ep => ep.EmployeeId == id).First();
            if(employeePassword.Password == sOutput.ToString())
            {
                return Problem("The new password must be different from the old one");
            }
            employeePassword.Password = sOutput.ToString();
            _context.Entry(employeePassword).State = EntityState.Modified;

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
        [HttpDelete("{id}/password")]
        public async Task<IActionResult> DeletePassword(int id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            if (_context.EmployeesPasswords.Where(ep => ep.EmployeeId == id).Count() == 0)
            {
                return NotFound();
            }
            var employeePassword = _context.EmployeesPasswords.Where(ep => ep.EmployeeId == id).First();            

            _context.EmployeesPasswords.Remove(employeePassword);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool validatePassword(EmployeesPasswords employeePassword, string password)
        {
            byte[] tmpSource;
            byte[] tmpHash;

            using (SHA256 hash = SHA256.Create())
            {
                tmpSource = Encoding.UTF8.GetBytes(password);
                tmpHash = hash.ComputeHash(tmpSource);
            }

            StringBuilder sOutput = new StringBuilder(tmpHash.Length);
            for (int i = 0; i < tmpHash.Length; i++)
            {
                sOutput.Append(tmpHash[i].ToString("X2"));
            }

            return sOutput.ToString() == employeePassword.Password;
        }
        private bool EmployeesExists(int id)
        {
            return (_context.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
