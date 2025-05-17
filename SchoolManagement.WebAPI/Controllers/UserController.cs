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
        public async Task<IActionResult> GetTeachers([FromQuery] string? search, [FromQuery] string? sortBy = "fullname", [FromQuery] bool isDescending = false, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _userService.GetUsersByRoleAsync("Teacher", search, sortBy, isDescending, pageNumber, pageSize);
            return StatusCode(result.Code, result);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("students")]
        public async Task<IActionResult> GetStudents([FromQuery] string? search, [FromQuery] string? sortBy = "fullname", [FromQuery] bool isDescending = false, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _userService.GetUsersByRoleAsync("Student", search, sortBy, isDescending, pageNumber, pageSize);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admins")]
        public async Task<IActionResult> GetAdmins([FromQuery] string? search, [FromQuery] string? sortBy = "fullname", [FromQuery] bool isDescending = false, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _userService.GetUsersByRoleAsync("Admin", search, sortBy, isDescending, pageNumber, pageSize);
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
