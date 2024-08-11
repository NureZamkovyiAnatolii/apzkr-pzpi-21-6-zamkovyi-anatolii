using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Data;
using Server.Data;
using WebApplication3.Models;
using System.Threading.Tasks;
using System;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly ServerContext _context;

        public SignUpController(ServerContext context)
        {
            _context = context;
        }

        [HttpGet("SignUpPage")]
        public IActionResult SignUpPage()
        {
            return Ok(new { role = "Тренер" }); // Початкове значення ролі
        }

        [HttpPost("SignUpAll")]
        public async Task<IActionResult> SignUpAll([FromBody] SignUpRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Role))
            {
                return BadRequest("Invalid sign-up request.");
            }

            var role = request.Role;
            Console.WriteLine("role=" + role);

            if (role == "Тренер")
            {
                return Ok(new { Message = "Redirect to Coaches Create" });
            }
            if (role == "Фанат")
            {
                return Ok(new { Message = "Redirect to Fans Create" });
            }
            if (role == "Спортсмен")
            {
                return Ok(new { Message = "Redirect to Athletes Create" });
            }
            if (role == "Організація")
            {
                return Ok(new { Message = "Redirect to Organizations Create" });
            }

            return BadRequest("Invalid role specified.");
        }
    }

    public class SignUpRequest
    {
        public string Role { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
