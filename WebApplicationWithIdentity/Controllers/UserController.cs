using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationWithIdentity.Dtos.UserDtos;
using WebApplicationWithIdentity.Models;

namespace WebApplicationWithIdentity.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        


        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        private readonly SignInManager<AppUser> _signInManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {

            var Users = await _userManager.Users.ToListAsync();

            return Ok(Users);

        }
        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

     
      
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(string id, UserupdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }
            existingUser.UserName = dto.Username;
            existingUser.FirstName = dto.FirstName;
            existingUser.LastName = dto.LastName;
            var result = await _userManager.UpdateAsync(existingUser);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok("User updated successfully.");
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok("User deleted successfully.");
        }

        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string id, ResestPasswordDto dto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }


             var passwordCheck = await _userManager.CheckPasswordAsync(user, dto.OldPassword);

            if(!passwordCheck)
            {
                return BadRequest("The old password is incorrect.");
            }

            var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Password reset successfully.");
        }


        [HttpPost("LockoutUser")]
        public async Task<IActionResult> LockoutUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

           DateTime LockoutDate= DateTime.UtcNow.AddDays(1);

            var result = await _userManager.SetLockoutEndDateAsync(user, LockoutDate);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("User locked out successfully.");
        }

        [HttpPost("UnlockUser")]
        public async Task<IActionResult> UnlockUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.SetLockoutEndDateAsync(user, null);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("User unlocked successfully.");
        }


    }
}
