using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using WebApplication3.Models;
using Microsoft.Extensions.Logging;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ServerContext _context;
        private readonly ILogger<LoginController> _logger;

        public LoginController(ServerContext context, ILogger<LoginController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // POST: api/Login
        [HttpPost]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                _logger.LogError("Invalid login request received.");
                return BadRequest("Invalid login request.");
            }

            try
            {
                if (IsUserLoggedIn())
                {
                    _logger.LogWarning($"User with name {loginRequest.Name} and role {loginRequest.Role} is  registered.");
                    return BadRequest(new LoginResponse { Success = false, Message = "User is  registered." });
                }
                switch (loginRequest.Role)
                {
                    case "Тренер":
                        return await LoginCoach(loginRequest.Name, loginRequest.Password);
                    case "Спортсмен":
                        return await LoginAthlete(loginRequest.Name, loginRequest.Password);
                    case "Організація":
                        return await LoginOrganization(loginRequest.Name, loginRequest.Password);
                    default:
                        _logger.LogError($"Invalid role: {loginRequest.Role}");
                        return BadRequest("Invalid role.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while processing the login request: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
        private bool IsUserLoggedIn()
        {
            return HttpContext.Session.GetInt32("CoachId") != null ||
                   HttpContext.Session.GetInt32("AthleteId") != null ||
                   HttpContext.Session.GetInt32("OrgId") != null;
        }


        private async Task<ActionResult<LoginResponse>> LoginAthlete(string athleteName, string password)
        {
            var athlete = await _context.Athlete.FirstOrDefaultAsync(a => a.AthleteName == athleteName);
            if (athlete == null)
            {
                _logger.LogWarning($"Athlete with name {athleteName} not found.");
                return NotFound(new LoginResponse { Success = false, Message = "Athlete with this name not found." });
            }

            if (BCrypt.Net.BCrypt.Verify(password, athlete.Password))
            {
                HttpContext.Session.SetInt32("AthleteId", athlete.AthleteId);
                return Ok(new LoginResponse { Success = true, Message = "Login successful.", UserId = athlete.AthleteId });
            }
            else
            {
                _logger.LogWarning("Incorrect password for athlete.");
                return Unauthorized(new LoginResponse { Success = false, Message = "Incorrect password." });
            }
        }

        private async Task<ActionResult<LoginResponse>> LoginCoach(string coachName, string password)
        {
            var coach = await _context.Coach.FirstOrDefaultAsync(c => c.CoachName == coachName);
            if (coach == null)
            {
                _logger.LogWarning($"Coach with name {coachName} not found.");
                return NotFound(new LoginResponse { Success = false, Message = "Coach with this name not found." });
            }

            if (BCrypt.Net.BCrypt.Verify(password, coach.Password))
            {
                HttpContext.Session.SetInt32("CoachId", coach.CoachId);
                return Ok(new LoginResponse { Success = true, Message = "Login successful.", UserId = coach.CoachId });
            }
            else
            {
                _logger.LogWarning("Incorrect password for coach.");
                return Unauthorized(new LoginResponse { Success = false, Message = "Incorrect password." });
            }
        }

        private async Task<ActionResult<LoginResponse>> LoginOrganization(string organizationName, string password)
        {
            var organization = await _context.Organization.FirstOrDefaultAsync(o => o.OrganizationName == organizationName);
            if (organization == null)
            {
                _logger.LogWarning($"Organization with name {organizationName} not found.");
                return NotFound(new LoginResponse { Success = false, Message = "Organization with this name not found." });
            }

            if (BCrypt.Net.BCrypt.Verify(password, organization.Password))
            {
                HttpContext.Session.SetInt32("OrgId", organization.OrganizationId);
                return Ok(new LoginResponse { Success = true, Message = "Login successful.", UserId = organization.OrganizationId });
            }
            else
            {
                _logger.LogWarning("Incorrect password for organization.");
                return Unauthorized(new LoginResponse { Success = false, Message = "Incorrect password." });
            }
        }
        // POST: api/Logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            _logger.LogInformation("Logout successful.");
            return Ok(new { message = "Logout successful." });
        }
    }



    public class LoginRequest
    {
        public string Role { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int? UserId { get; set; }
    }
}
