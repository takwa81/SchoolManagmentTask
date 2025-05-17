using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Application.DTOs.Enrollment;
using SchoolManagement.Application.Responses;

namespace SchoolManagement.Application.Interfaces
{
    public interface IEnrollmentService
    {
        Task<ApiResponse<string>> EnrollAsync(EnrollStudentRequest request);
    }
}
