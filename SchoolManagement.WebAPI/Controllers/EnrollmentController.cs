using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Application.DTOs.Enrollment;
using SchoolManagement.Application.Interfaces;

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
            var result = await _enrollmentService.EnrollAsync(request);
            return StatusCode(result.Code, result);
        }
    }

}
