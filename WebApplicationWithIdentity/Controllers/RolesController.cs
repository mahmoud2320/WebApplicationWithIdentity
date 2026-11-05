using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationWithIdentity.Dtos.RolesDto;
using WebApplicationWithIdentity.Models;

namespace WebApplicationWithIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
       private readonly RoleManager<AppRole> _roleManager;

       private readonly UserManager<AppUser> _userManager;

        public RolesController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }



        [HttpGet("get-all-roles")]
        public IActionResult GetAllRoles()
        {
          var Roles = _roleManager.Roles.ToList();

            if(Roles == null)
            {
                return NotFound(new { Message = "No roles found." });
            }

            return Ok(Roles);
        }
        [HttpGet("get-role/{roleId}")]
        public async Task<IActionResult> GetRoleById(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound(new { Message = "Role not found." });
            }
            return Ok(role);
        }

        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole(AddRoleDto role)
        {
            var result = await _roleManager.CreateAsync(new AppRole { Name = role.RoleName });
            if (result.Succeeded)
            {
                return Ok(new { Message = "Role created successfully." });
            }
            return BadRequest(result.Errors);
        }

        [HttpPut("updateRole/{roleId}")]
        public async Task<IActionResult> UpdateRole(string roleId, string newRoleName)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound(new { Message = "Role not found." });
            }
            role.Name = newRoleName;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return Ok(role);
            }
            return BadRequest(result.Errors);
        }


        [HttpDelete("deleteRole/{roleId}")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound(new { Message = "Role not found." });
            }
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Role deleted successfully." });
            }
            return BadRequest(result.Errors);
        }


        [HttpGet("UserRoles")] 
         public async Task<IActionResult> GetUserRoles

    }
}
