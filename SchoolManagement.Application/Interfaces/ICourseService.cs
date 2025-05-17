using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Application.DTOs.Course;
using SchoolManagement.Application.Responses;

namespace SchoolManagement.Application.Interfaces
{
    public interface ICourseService
    {
        Task<ApiResponse<CourseDto>> CreateAsync(CreateCourseRequest request);
        Task<ApiResponse<CourseDto>> UpdateAsync(UpdateCourseRequest request);
        Task<ApiResponse<bool>> DeleteAsync(int courseId);
        Task<ApiResponse<List<CourseDto>>> GetAllAsync();
        Task<ApiResponse<List<CourseDto>>> GetByTeacherAsync(int teacherId);
        Task<ApiResponse<List<CourseDto>>> GetByStudentAsync(int studentId);
    }

}
