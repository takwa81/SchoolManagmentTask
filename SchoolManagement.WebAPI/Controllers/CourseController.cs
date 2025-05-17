using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Application.DTOs.Course;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Application.Responses;

namespace SchoolManagement.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCourseRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                return BadRequest(ApiResponse<string>.Fail(errorMessage, 400));
            }


            var result = await _courseService.CreateAsync(request);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCourseRequest request)
        {
            var result = await _courseService.UpdateAsync(request);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _courseService.DeleteAsync(id);
            return StatusCode(result.Code, result);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll(
          [FromQuery] string? search,
          [FromQuery] string? sortBy = "name",
          [FromQuery] bool isDescending = false,
          [FromQuery] int pageNumber = 1,
          [FromQuery] int pageSize = 10)
        {
            var result = await _courseService.GetAllAsync(search, sortBy, isDescending, pageNumber, pageSize);
            return StatusCode(result.Code, result);
        }

        //[Authorize(Roles = "Teacher")]
        //[HttpGet("by-teacher/{teacherId}")]
        //public async Task<IActionResult> GetByTeacher(int teacherId)
        //{
        //    var result = await _courseService.GetByTeacherAsync(teacherId);
        //    return StatusCode(result.Code, result);
        //}

        //[Authorize(Roles = "Student")]
        //[HttpGet("by-student/{studentId}")]
        //public async Task<IActionResult> GetByStudent(int studentId)
        //{
        //    var result = await _courseService.GetByStudentAsync(studentId);
        //    return StatusCode(result.Code, result);
        //}
    }
}
