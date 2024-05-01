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
    public class AppointmentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AppointmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointments>>> GetAppointments()
        {
          if (_context.Appointments == null)
          {
              return NotFound();
          }
            return await _context.Appointments.Select(a => new Appointments
            {
                Id = a.Id,
                ClientId= a.ClientId,
                ServiceId = a.ServiceId,
                StartDateTime = a.StartDateTime,
                EndDateTime = a.EndDateTime,
                StatusId = a.StatusId,
                Price = a.Price,
                Client = _context.Clients.Where(c => c.Id == a.ClientId)
                                         .FirstOrDefault(),
                Service = _context.Services.Where(s => s.Id == a.ServiceId).Select(s => new Services
                {
                    Id = s.Id,
                    Name= s.Name,
                    BaseCost= s.BaseCost,
                    BaseTimeMinutes= s.BaseTimeMinutes,
                    CategoryId= s.CategoryId,
                    Category= _context.Categories.Where(c => c.Id == s.CategoryId).FirstOrDefault()
                }).FirstOrDefault(),
                Status = _context.StatusesOfAppointments.Where(soa => soa.Id == a.StatusId).FirstOrDefault()
            }).ToListAsync();
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointments>> GetAppointments(int id)
        {
          if (_context.Appointments == null)
          {
              return NotFound();
          }
            var appointments = await _context.Appointments.FindAsync(id);

            if (appointments == null)
            {
                return NotFound();
            }

            appointments.Service = await _context.Services.FindAsync(appointments.ServiceId);
            appointments.Status = await _context.StatusesOfAppointments.FindAsync(appointments.StatusId);
            appointments.Client = await _context.Clients.FindAsync(appointments.ClientId);
            if(appointments.Client != null)
            {
                appointments.Client.Appointments = new List<Appointments>();
            }

            return appointments;
        }

        // PUT: api/Appointments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointments(int id, Appointments appointments)
        {
            if (id != appointments.Id)
            {
                return BadRequest();
            }

            _context.Entry(appointments).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentsExists(id))
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

        // POST: api/Appointments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Appointments>> PostAppointments(Appointments appointments)
        {
          if (_context.Appointments == null)
          {
              return Problem("Entity set 'AppDbContext.Appointments'  is null.");
          }
            _context.Appointments.Add(appointments);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppointments", new { id = appointments.Id }, appointments);
        }

        // DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointments(int id)
        {
            if (_context.Appointments == null)
            {
                return NotFound();
            }
            var appointments = await _context.Appointments.FindAsync(id);
            if (appointments == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointments);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AppointmentsExists(int id)
        {
            return (_context.Appointments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
