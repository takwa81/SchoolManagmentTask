using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Application.DTOs.Enrollment;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Application.Responses;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Enums;
using SchoolManagement.Infrastructure.Data;

namespace SchoolManagement.Infrastructure.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly AppDbContext _db;

        public EnrollmentService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ApiResponse<string>> EnrollAsync(EnrollStudentRequest request)
        {
            try
            {
                var user = await _db.Users.FindAsync(request.StudentId);
                if (user == null || user.Role != UserRole.Student)
                    return ApiResponse<string>.Fail("Invalid student", 400);

                var course = await _db.Courses.FindAsync(request.CourseId);
                if (course == null)
                    return ApiResponse<string>.Fail("Course not found", 404);

                var exists = await _db.Enrollments
                    .AnyAsync(e => e.UserId == request.StudentId && e.CourseId == request.CourseId);
                if (exists)
                    return ApiResponse<string>.Fail("Student already enrolled in this course", 400);

                _db.Enrollments.Add(new Enrollment
                {
                    UserId = request.StudentId,
                    CourseId = request.CourseId
                });

                await _db.SaveChangesAsync();
                return ApiResponse<string>.Success("Student enrolled successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Fail("Enrollment failed: " + ex.Message, 500);
            }
        }

    }

}
