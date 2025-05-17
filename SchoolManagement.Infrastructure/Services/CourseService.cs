using Microsoft.EntityFrameworkCore;
using SchoolManagement.Application.DTOs.Course;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Application.Responses;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Infrastructure.Data;

namespace SchoolManagement.Infrastructure.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _db;

        public CourseService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ApiResponse<CourseDto>> CreateAsync(CreateCourseRequest request)
        {
            try
            {
                var course = new Course
                {
                    Name = request.Name,
                    TeacherId = request.TeacherId
                };

                _db.Courses.Add(course);
                await _db.SaveChangesAsync();

                var result = await _db.Courses
                    .Include(c => c.Teacher)
                    .FirstOrDefaultAsync(c => c.Id == course.Id);

                return ApiResponse<CourseDto>.Success(new CourseDto
                {
                    Id = result!.Id,
                    Name = result.Name,
                    TeacherId = result.TeacherId,
                    TeacherName = result.Teacher.FullName
                }, "Course created");
            }
            catch (Exception ex)
            {
                return ApiResponse<CourseDto>.Fail("Create failed: " + ex.Message);
            }
        }

        public async Task<ApiResponse<CourseDto>> UpdateAsync(UpdateCourseRequest request)
        {
            try
            {
                var course = await _db.Courses.FindAsync(request.Id);
                if (course == null)
                    return ApiResponse<CourseDto>.Fail("Course not found", 404);

                course.Name = request.Name;
                await _db.SaveChangesAsync();

                return ApiResponse<CourseDto>.Success(new CourseDto
                {
                    Id = course.Id,
                    Name = course.Name,
                    TeacherId = course.TeacherId,
                    TeacherName = (await _db.Users.FindAsync(course.TeacherId))?.FullName ?? ""
                }, "Course updated");
            }
            catch (Exception ex)
            {
                return ApiResponse<CourseDto>.Fail("Update failed: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int courseId)
        {
            try
            {
                var course = await _db.Courses.FindAsync(courseId);
                if (course == null)
                    return ApiResponse<bool>.Fail("Course not found", 404);

                _db.Courses.Remove(course);
                await _db.SaveChangesAsync();

                return ApiResponse<bool>.Success(true, "Course deleted");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail("Delete failed: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<CourseDto>>> GetAllAsync()
        {
            var courses = await _db.Courses.Include(c => c.Teacher).ToListAsync();

            return ApiResponse<List<CourseDto>>.Success(
                courses.Select(c => new CourseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    TeacherId = c.TeacherId,
                    TeacherName = c.Teacher.FullName
                }).ToList(), "All courses");
        }

        public async Task<ApiResponse<List<CourseDto>>> GetByTeacherAsync(int teacherId)
        {
            var courses = await _db.Courses
                .Include(c => c.Teacher)
                .Where(c => c.TeacherId == teacherId)
                .ToListAsync();

            return ApiResponse<List<CourseDto>>.Success(
                courses.Select(c => new CourseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    TeacherId = c.TeacherId,
                    TeacherName = c.Teacher.FullName
                }).ToList(), "Teacher courses");
        }

        public async Task<ApiResponse<List<CourseDto>>> GetByStudentAsync(int studentId)
        {
            var courses = await _db.Enrollments
                .Include(e => e.Course).ThenInclude(c => c.Teacher)
                .Where(e => e.UserId == studentId)
                .Select(e => e.Course)
                .ToListAsync();

            return ApiResponse<List<CourseDto>>.Success(
                courses.Select(c => new CourseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    TeacherId = c.TeacherId,
                    TeacherName = c.Teacher.FullName
                }).ToList(), "Student courses");
        }
    }
}
