using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Application.DTOs.Enrollment;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Application.Responses;

namespace SchoolManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("enroll")]
        public async Task<IActionResult> Enroll([FromBody] EnrollStudentRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                return BadRequest(ApiResponse<string>.Fail(errorMessage, 400));
            }
            var result = await _enrollmentService.EnrollAsync(request);
            return StatusCode(result.Code, result);
        }
    }

}
