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

                var assignment = new Assignment
                {
                    Title = request.Title,
                    Description = request.Description,
                    DueDate = request.DueDate,
                    CourseId = request.CourseId
                };

                await _db.Assignments.AddAsync(assignment);
                await _db.SaveChangesAsync();

                return ApiResponse<string>.Success("Assignment created successfully");
            }
            catch (Exception ex)
            {
                var error = ex.InnerException?.Message ?? ex.Message;
                return ApiResponse<string>.Fail("Failed to create assignment: " + error, 500);
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
                        Description = a.Description,
                        DueDate = a.DueDate,
                        CourseId = a.CourseId
                    })
                    .ToListAsync();

                return ApiResponse<List<AssignmentDto>>.Success(assignments, "Assignments retrieved");
            }
            catch (Exception ex)
            {
                var error = ex.InnerException?.Message ?? ex.Message;
                return ApiResponse<List<AssignmentDto>>.Fail("Failed to retrieve assignments: " + error, 500);
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

                var isEnrolled = await _db.Enrollments.AnyAsync(e =>
                e.CourseId == assignment.CourseId && e.UserId == request.StudentId);

                if (!isEnrolled)
                    return ApiResponse<string>.Fail("Student is not enrolled in the course for this assignment", 403);

                var existingGrade = await _db.Grades.FirstOrDefaultAsync(g =>
                    g.AssignmentId == request.AssignmentId && g.StudentId == request.StudentId);

                if (existingGrade != null)
                    return ApiResponse<string>.Fail("Grade already submitted for this assignment", 409);

                var grade = new Grade
                {
                    AssignmentId = request.AssignmentId,
                    StudentId = request.StudentId,
                    Score = request.Score,
                    Feedback = request.Feedback
                };

                await _db.Grades.AddAsync(grade);
                await _db.SaveChangesAsync();

                return ApiResponse<string>.Success("Grade submitted successfully");
            }
            catch (Exception ex)
            {
                var error = ex.InnerException?.Message ?? ex.Message;
                return ApiResponse<string>.Fail("Failed to submit grade: " + error, 500);
            }
        }

        public async Task<ApiResponse<List<GradeDto>>> GetStudentGradesAsync(int studentId)
        {
            try
            {
                var grades = await _db.Grades
                    .Where(g => g.StudentId == studentId)
                    .Include(g => g.Assignment)
                    .Select(g => new GradeDto
                    {
                        AssignmentId = g.AssignmentId,
                        AssignmentTitle = g.Assignment.Title,
                        Score = g.Score,
                        Feedback = g.Feedback
                    })
                    .ToListAsync();

                return ApiResponse<List<GradeDto>>.Success(grades, "Grades retrieved");
            }
            catch (Exception ex)
            {
                var error = ex.InnerException?.Message ?? ex.Message;
                return ApiResponse<List<GradeDto>>.Fail("Failed to retrieve grades: " + error, 500);
            }
        }
    }
}
