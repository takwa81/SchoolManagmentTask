using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Application.DTOs.Course;
using SchoolManagement.Application.Interfaces;

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
        public async Task<IActionResult> GetAll()
        {
            var result = await _courseService.GetAllAsync();
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet("by-teacher/{teacherId}")]
        public async Task<IActionResult> GetByTeacher(int teacherId)
        {
            var result = await _courseService.GetByTeacherAsync(teacherId);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Student")]
        [HttpGet("by-student/{studentId}")]
        public async Task<IActionResult> GetByStudent(int studentId)
        {
            var result = await _courseService.GetByStudentAsync(studentId);
            return StatusCode(result.Code, result);
        }
    }
}
