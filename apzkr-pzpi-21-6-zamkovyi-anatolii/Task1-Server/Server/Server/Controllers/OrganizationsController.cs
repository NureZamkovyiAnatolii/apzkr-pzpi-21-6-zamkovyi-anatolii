using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using WebApplication3.Models;
using BC = BCrypt.Net.BCrypt;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationsController : ControllerBase
    {
        private readonly ServerContext _context;

        public OrganizationsController(ServerContext context)
        {
            _context = context;
        }

        // GET: api/Organizations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Organization>>> GetOrganizations()
        {
            if (_context.Organization == null)
            {
                return Problem("Entity set 'ServerContext.Organization' is null.");
            }
            return await _context.Organization.ToListAsync();
        }

        // GET: api/Organizations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Organization>> GetOrganization(int id)
        {
            if (_context.Organization == null)
            {
                return NotFound();
            }

            var organization = await _context.Organization.FindAsync(id);

            if (organization == null)
            {
                return NotFound();
            }

            return organization;
        }

        // POST: api/Organizations
        [HttpPost]
        public async Task<ActionResult<Organization>> CreateOrganization([Bind("OrganizationId,OrganizationName,Password,PhoneNumber,Country")] Organization organization)
        {
            if (ModelState.IsValid)
            {
                organization.Password = BC.HashPassword(organization.Password);
                _context.Add(organization);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetOrganization), new { id = organization.OrganizationId }, organization);
            }

            return BadRequest(ModelState);
        }

        // PUT: api/Organizations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditOrganization(int id, [Bind("OrganizationId,OrganizationName,Password,PhoneNumber,Country")] Organization organization)
        {
            if (id != organization.OrganizationId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    organization.Password = BC.HashPassword(organization.Password);
                    _context.Update(organization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationExists(organization.OrganizationId))
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
            return BadRequest(ModelState);
        }

        // DELETE: api/Organizations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganization(int id)
        {
            if (_context.Organization == null)
            {
                return NotFound();
            }

            var organization = await _context.Organization.FindAsync(id);
            if (organization == null)
            {
                return NotFound();
            }

            _context.Organization.Remove(organization);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrganizationExists(int id)
        {
            return (_context.Organization?.Any(e => e.OrganizationId == id)).GetValueOrDefault();
        }
    }
}
