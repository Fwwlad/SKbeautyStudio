using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SKbeautyStudio.Db;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _context.EmployeesPasswords.Where(ep => ep.Login == model.Username).FirstOrDefaultAsync();
        if (user != null && validatePassword(user, model.Password))
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("qiuf111HisAxm39S9cfk!dfid9ScC31JhdblaEIdn4bwoe342");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Login)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = "SKstudioMobileApp",
                Audience = "SKstudioApi",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { Token = tokenHandler.WriteToken(token) });
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

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}
