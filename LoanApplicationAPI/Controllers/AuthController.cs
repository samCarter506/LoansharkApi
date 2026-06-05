using LoanApplicationAPI.Models;
using LoanApplicationAPI.Models.DTO;
using LoanApplicationAPI.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoanApplicationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser>
            _userManager;

        private readonly IJwtService
            _jwtService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            IJwtService jwtService)
        {
            _userManager = userManager;

            _jwtService = jwtService;
        }

        // =========================================
        // REGISTER
        // =========================================
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(
            RegisterDto model)
        {
            var userExists =
                await _userManager
                    .FindByEmailAsync(model.Email);

            if (userExists != null)
            {
                return BadRequest(new
                {
                    message = "User already exists"
                });
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result =
                await _userManager.CreateAsync(
                    user,
                    model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(
                    result.Errors);
            }

            await _userManager.AddToRoleAsync(
                user,
                "admin");

            return Ok(new
            {
                message =
                    "User created successfully"
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> ChangeUserRoles(string id,[FromBody] RolesDto obj)
        {
            try
            {
                // FIND USER
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new
                    {
                        message = "User not found"
                    });
                }

                // GET CURRENT ROLES
                var currentRoles =
                    await _userManager.GetRolesAsync(user);

                // REMOVE CURRENT ROLES
                var removeResult =
                    await _userManager.RemoveFromRolesAsync(
                        user,
                        currentRoles);

                if (!removeResult.Succeeded)
                {
                    return BadRequest(removeResult.Errors);
                }

                // ADD NEW ROLE
                var addResult =
                    await _userManager.AddToRoleAsync(
                        user,
                        obj.Role);

                if (!addResult.Succeeded)
                {
                    return BadRequest(addResult.Errors);
                }

                return Ok(new
                {
                    message = "User role updated successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
        }
        // =========================================
        // LOGIN
        // =========================================
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(LoginDto model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.email);

                if (user == null)
                    return Unauthorized("Invalid credentials");

                var validPassword =
                    await _userManager.CheckPasswordAsync(user, model.password);

                if (!validPassword)
                    return Unauthorized("Invalid credentials");

                // generate token

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }
        // =========================================
        // CURRENT USER
        // =========================================


        [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            email =
        User.FindFirst(ClaimTypes.Email)?.Value,

            fullname =
        User.FindFirst(ClaimTypes.Name)?.Value,

            role =
        User.FindFirst(ClaimTypes.Role)?.Value
        });
    }
    // =========================================
    // LOGOUT
    // =========================================
    [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(
                "access_token");

            return Ok(new
            {
                message =
                    "Logged out successfully"
            });
        }
    }
}