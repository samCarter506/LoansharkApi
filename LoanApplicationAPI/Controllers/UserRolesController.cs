using LoanApplicationAPI.Models;
using LoanApplicationAPI.Services;
using LoanApplicationLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoanApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserRolesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserRolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================================
        // GET: api/UserRoles
        // =========================================
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var roles = await _context.Roles.ToListAsync();

                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // =========================================
        // GET: api/UserRoles/{id}
        // =========================================
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(string id)
        {
            try
            {
                var role = await _context.Roles
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (role == null)
                    return NotFound("Role not found");

                return Ok(role);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // =========================================
        // POST: api/UserRoles
        // =========================================
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(UserRoles role)
        {
            try
            {
                if (role == null)
                    return BadRequest("Invalid role");

                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetRole),
                    new { id = role.Id },
                    role
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // =========================================
        // PUT: api/UserRoles/{id}
        // =========================================
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(string id, UserRoles updatedRole)
        {
            try
            {
                if (id != updatedRole.Id)
                    return BadRequest("Role ID mismatch");

                var role = await _context.Roles
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (role == null)
                    return NotFound("Role not found");

                // Update fields
                role.Name = updatedRole.Name;
                role.NormalizedName = updatedRole.NormalizedName;

                _context.Roles.Update(role);
                await _context.SaveChangesAsync();

                return Ok(role);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            try
            {
                var role = await _context.Roles
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (role == null)
                    return NotFound("Role not found");

                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Role deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}