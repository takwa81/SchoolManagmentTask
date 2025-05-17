using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Application.DTOs.Assignment;
using SchoolManagement.Application.DTOs.Grade;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Application.Responses;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Enums;
using SchoolManagement.Infrastructure.Data;

namespace SchoolManagement.Infrastructure.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly AppDbContext _db;

        public AssignmentService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ApiResponse<string>> CreateAssignmentAsync(CreateAssignmentRequest request)
        {
            try
            {
                var course = await _db.Courses.FindAsync(request.CourseId);
                if (course == null)
                    return ApiResponse<string>.Fail("Course not found", 404);

                _db.Assignments.Add(new Assignment
                {
                    Title = request.Title,
                    CourseId = request.CourseId
                });

                await _db.SaveChangesAsync();
                return ApiResponse<string>.Success("Assignment created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Fail("Failed to create assignment: " + ex.Message, 500);
            }
        }

        public async Task<ApiResponse<List<AssignmentDto>>> GetAssignmentsByCourseAsync(int courseId)
        {
            try
            {
                var assignments = await _db.Assignments
                    .Where(a => a.CourseId == courseId)
                    .Select(a => new AssignmentDto
                    {
                        Id = a.Id,
                        Title = a.Title,
                        CourseId = a.CourseId
                    })
                    .ToListAsync();

                return ApiResponse<List<AssignmentDto>>.Success(assignments, "Assignments retrieved");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<AssignmentDto>>.Fail("Failed to retrieve assignments: " + ex.Message, 500);
            }
        }

        public async Task<ApiResponse<string>> SubmitGradeAsync(SubmitGradeRequest request)
        {
            try
            {
                var assignment = await _db.Assignments.FindAsync(request.AssignmentId);
                var student = await _db.Users.FindAsync(request.StudentId);

                if (assignment == null || student == null || student.Role != UserRole.Student)
                    return ApiResponse<string>.Fail("Invalid assignment or student", 400);

                var grade = new Grade
                {
                    AssignmentId = request.AssignmentId,
                    StudentId = request.StudentId,
                    Score = request.Score
                };

                _db.Grades.Add(grade);
                await _db.SaveChangesAsync();

                return ApiResponse<string>.Success("Grade submitted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Fail("Failed to submit grade: " + ex.Message, 500);
            }
        }

        public async Task<ApiResponse<List<GradeDto>>> GetStudentGradesAsync(int studentId)
        {
            try
            {
                var grades = await _db.Grades
                    .Where(g => g.StudentId == studentId)
                    .Select(g => new GradeDto
                    {
                        AssignmentId = g.AssignmentId,
                        AssignmentTitle = g.Assignment.Title,
                        Score = g.Score
                    })
                    .ToListAsync();

                return ApiResponse<List<GradeDto>>.Success(grades, "Grades retrieved");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GradeDto>>.Fail("Failed to retrieve grades: " + ex.Message, 500);
            }
        }
    }

}
