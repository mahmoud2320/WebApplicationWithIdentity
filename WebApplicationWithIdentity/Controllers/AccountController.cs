using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationWithIdentity.Dtos.AccountDtos;
using WebApplicationWithIdentity.Models;

namespace WebApplicationWithIdentity.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
      private readonly   UserManager<AppUser> _userManager;
      private readonly   RoleManager<AppRole> _roleManager;

      private readonly   SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }


        

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if( await _userManager.FindByEmailAsync(registerDto.Email) != null||await _userManager.FindByNameAsync(registerDto.Username) != null)
            {
                return BadRequest("Email or username is already taken.");
            }

            var user = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                FirstName = registerDto.firstName,
                LastName = registerDto.lastName
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if(!result.Succeeded)
            {
                return BadRequest("Failed to create user.");
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok("User created successfully.");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return BadRequest("Invalid email or password.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);


            if(user.LockoutEnd!=null)
            {
                return BadRequest("User is locked out.");
            }

            if (!result.Succeeded)
            {
                return BadRequest("Invalid email or password.");
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok("User logged in successfully.");
        }


        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("User logged out successfully.");
        }

    }
}
