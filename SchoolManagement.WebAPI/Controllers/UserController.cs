using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Application.Interfaces;

namespace SchoolManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("teachers")]
        public async Task<IActionResult> GetTeachers()
        {
            var result = await _userService.GetUsersByRoleAsync("Teacher");
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("students")]
        public async Task<IActionResult> GetStudents()
        {
            var result = await _userService.GetUsersByRoleAsync("Student");
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admins")]
        public async Task<IActionResult> GetAdmins()
        {
            var result = await _userService.GetUsersByRoleAsync("Admin");
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            return StatusCode(result.Code, result);
        }
    }

}
