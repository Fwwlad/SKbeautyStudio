using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;
using SKbeautyStudio.Db;

namespace SKbeautyStudio.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesPasswordsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesPasswordsController(AppDbContext context)
        {
            _context = context;
        }
        // GET: api/EmployeesPasswords
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeesPasswords>>> GetEmployeesAccounts()
        {
            if (_context.EmployeesMobileAppPages == null)
            {
                return NotFound();
            }
            try
            {
            var res =  _context.EmployeesPasswords.Select(ep => new EmployeesPasswords
            {
                Login = ep.Login,
                Password = ep.Password,
                EmployeeId = ep.EmployeeId,
                Employee = _context.Employees.Where(em => em.Id == ep.EmployeeId).Select(em => new Employees
                {
                    Id = em.Id,
                    Name = em.Name,
                    Surname = em.Surname,
                    Patronymic = em.Patronymic,
                    Gender = em.Gender,
                    Phone = em.Phone,
                    Birthday = em.Birthday,
                    DateOfHire = em.DateOfHire,
                    Email = em.Email,
                    SalaryPercent = em.SalaryPercent,
                    EmployeeMobileAppPages = _context.EmployeesMobileAppPages
                                        .Where(emap => emap.EmployeeId == em.Id)
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
                                        }).ToArray()
                }).FirstOrDefault()
            }).ToList();
            return res;
            } catch(Exception ex)
            {
                return NotFound();
            }
        }
        // PUT: api/EmployeesPasswords/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{login}")]
        public async Task<IActionResult> PutEmployeesPasswords(string login, EmployeesPasswords employeesPasswords)
        {
            if (login != employeesPasswords.Login)
            {
                return BadRequest();
            }

            if (_context.EmployeesPasswords.Where(ep => ep.EmployeeId == employeesPasswords.EmployeeId).Count() == 0)
            {
                return NotFound();
            }
            if (_context.Employees.Where(e => e.Id == employeesPasswords.EmployeeId).Count() == 0)
            {
                return NotFound();
            }

            byte[] tmpSource;
            byte[] tmpHash;

            using (SHA256 hash = SHA256.Create())
            {
                tmpSource = Encoding.UTF8.GetBytes(employeesPasswords.Password);
                tmpHash = hash.ComputeHash(tmpSource);
            }

            StringBuilder sOutput = new StringBuilder(tmpHash.Length);
            for (int i = 0; i < tmpHash.Length; i++)
            {
                sOutput.Append(tmpHash[i].ToString("X2"));
            }

            EmployeesPasswords employeePassword = _context.EmployeesPasswords.Where(ep => ep.EmployeeId == employeesPasswords.EmployeeId).First();
            if (employeePassword.Password == sOutput.ToString())
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
                if (!EmployeesPasswordsExists(login))
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

        // POST: api/EmployeesPasswords
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeesPasswords>> PostEmployeesPasswords(EmployeesPasswords employeesPasswords)
        {
          if (_context.EmployeesPasswords == null)
          {
              return Problem("Entity set 'AppDbContext.EmployeesPasswords'  is null.");
          }
            if (_context.EmployeesPasswords.Where(ep => ep.EmployeeId == employeesPasswords.EmployeeId).Count() > 0)
            {
                return Problem("The password has already been set");
            }

            byte[] tmpSource;
            byte[] tmpHash;

            using (SHA256 hash = SHA256.Create())
            {
                tmpSource = Encoding.UTF8.GetBytes(employeesPasswords.Password);
                tmpHash = hash.ComputeHash(tmpSource);
            }


            StringBuilder sOutput = new StringBuilder(tmpHash.Length);
            for (int i = 0; i < tmpHash.Length; i++)
            {
                sOutput.Append(tmpHash[i].ToString("X2"));
            }

            var employeePassword = new EmployeesPasswords
            {
                Login = employeesPasswords.Login,
                EmployeeId = employeesPasswords.EmployeeId,
                Password = sOutput.ToString()
            };
            _context.EmployeesPasswords.Add(employeePassword);
            await _context.SaveChangesAsync();

            return NoContent();
            
        }

        // DELETE: api/EmployeesPasswords/5
        [HttpDelete("{login}")]
        public async Task<IActionResult> DeleteEmployeesPasswords(string login)
        {
            if (_context.EmployeesPasswords == null)
            {
                return NotFound();
            }
            var employeesPasswords = await _context.EmployeesPasswords.FindAsync(login);
            if (employeesPasswords == null)
            {
                return NotFound();
            }

            _context.EmployeesPasswords.Remove(employeesPasswords);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [AllowAnonymous]
        [HttpGet("{login}/password/validate/{password}")]
        public async Task<IActionResult> CheckPassword(string login, string password)
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
                var employees = await _context.Employees.FindAsync(account.EmployeeId);

                if (employees == null)
                {
                    return NotFound();
                }

                /*employees.EmployeeMobileAppPages = _context.EmployeesMobileAppPages
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
                employees.AvailableCategories = _context.EmployeesJobTitles
                                                .Where(ejt => ejt.EmployeesId == employees.Id)
                                                .Select(ejt => new EmployeesJobTitles
                                                {
                                                    CategoriesId = ejt.CategoriesId,
                                                    EmployeesId = ejt.EmployeesId,
                                                    Categories = _context.Categories.Where(c => c.Id == ejt.CategoriesId).FirstOrDefault()
                                                }).ToArray();

                return employees;*/
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("qiuf111HisAxm39S9cfk!dfid9ScC31JhdblaEIdn4bwoe342");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, login)
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    Issuer = "SKstudioMobileApp",
                    Audience = "SKstudioApi",
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new { Token = tokenHandler.WriteToken(token), EmployeeId = employees.Id });
            }


            return Unauthorized();
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
        private bool EmployeesPasswordsExists(string id)
        {
            return (_context.EmployeesPasswords?.Any(e => e.Login == id)).GetValueOrDefault();
        }
    }
}
