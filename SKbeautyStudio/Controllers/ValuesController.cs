using Microsoft.AspNetCore.Mvc;
using SKbeautyStudio.Db;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SKbeautyStudio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ValuesController(AppDbContext context)
        {
            _context = context;
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public string Get()
        {
            return "Тест пройден";
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public AppDbContext Get(int id)
        {
            return _context;
        }
    }
}
