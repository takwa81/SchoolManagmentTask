using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Application.DTOs.Assignment;
using SchoolManagement.Application.DTOs.Grade;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Application.Responses;

namespace SchoolManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;

        public AssignmentController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public async Task<IActionResult> CreateAssignment([FromBody] CreateAssignmentRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                return BadRequest(ApiResponse<string>.Fail(errorMessage, 400));
            }
            var result = await _assignmentService.CreateAssignmentAsync(request);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Teacher,Student")]
        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetAssignments(int courseId)
        {
            var result = await _assignmentService.GetAssignmentsByCourseAsync(courseId);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost("grade")]
        public async Task<IActionResult> SubmitGrade([FromBody] SubmitGradeRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                return BadRequest(ApiResponse<string>.Fail(errorMessage, 400));
            }

            var result = await _assignmentService.SubmitGradeAsync(request);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Student")]
        [HttpGet("grades/{studentId}")]
        public async Task<IActionResult> GetStudentGrades(int studentId)
        {
            var result = await _assignmentService.GetStudentGradesAsync(studentId);
            return StatusCode(result.Code, result);
        }
    }

}
