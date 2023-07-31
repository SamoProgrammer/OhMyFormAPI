using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FormGeneratorAPI.Authentication;
using FormGeneratorAPI.Authentication.Entities;
using FormGeneratorAPI.Database;
using FormGeneratorAPI.ViewModels;

namespace FormGeneratorAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly IJwtManagerRepository _jwtManagerRepository;

    public UsersController(DatabaseContext context, JwtManagerRepository jwtManagerRepository)
    {
        _context = context;
        _jwtManagerRepository = jwtManagerRepository;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserViewModel user)
    {
        if (!ModelState.IsValid)
        {
            // The request data is invalid
            return BadRequest();
        }

        if (await _context.Users.AnyAsync(x => x.Username == user.Username))
        {
            return BadRequest("user exists.");
        }

        using (SHA256 sha256 = SHA256.Create())
        {
            // Convert the password string to a byte array
            byte[] passwordBytes = Encoding.UTF8.GetBytes(user.Password);

            // Compute the hash value of the password bytes
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);

            // Convert the hash bytes to a hexadecimal string
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }

            string hashedPassword = sb.ToString();

            // Save the hashed password in the database or use it as needed
            user.Password = hashedPassword;
        }

        await _context.Users.AddAsync(new User()
        {
            Username = user.Username,
            Password = user.Password,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role
        });
        await _context.SaveChangesAsync();
        return Ok();
    }
    [HttpGet("Login")]
    public async Task<IActionResult> Login(string username, string password)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        using (SHA256 sha256 = SHA256.Create())
        {
            // Convert the password string to a byte array
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Compute the hash value of the password bytes
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);

            // Convert the hash bytes to a hexadecimal string
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }

            string hashedPassword = sb.ToString();

            // Save the hashed password in the database or use it as needed
            password = hashedPassword;
        }
        if (!await _context.Users.AnyAsync(x => x.Username == username && x.Password == password))
        {
            return NotFound();
        }
        User user = await _context.Users.FirstAsync(x => x.Username == username);
        return Ok(_jwtManagerRepository.Authenticate(user));


    }
}