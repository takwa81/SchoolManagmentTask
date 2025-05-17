using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Application.DTOs.Assignment;
using SchoolManagement.Application.DTOs.Grade;
using SchoolManagement.Application.Responses;

namespace SchoolManagement.Application.Interfaces
{
    public interface IAssignmentService
    {
        Task<ApiResponse<string>> CreateAssignmentAsync(CreateAssignmentRequest request);
        Task<ApiResponse<List<AssignmentDto>>> GetAssignmentsByCourseAsync(int courseId);
        Task<ApiResponse<string>> SubmitGradeAsync(SubmitGradeRequest request);
        Task<ApiResponse<List<GradeDto>>> GetStudentGradesAsync(int studentId);
    }

}
