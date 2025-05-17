using Microsoft.EntityFrameworkCore;
using SchoolManagement.Application.DTOs.Course;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Application.Responses;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using SchoolManagement.Domain.Enums;

namespace SchoolManagement.Infrastructure.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _db;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CourseService(AppDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<ApiResponse<CourseDto>> CreateAsync(CreateCourseRequest request)
        {
            try
            {
                var teacher = await _db.Users.FirstOrDefaultAsync(u => u.Id == request.TeacherId && u.Role == UserRole.Teacher);

                if (teacher == null)
                {
                    return ApiResponse<CourseDto>.Fail("Invalid Teacher ID: No teacher found with the specified ID.", 400);
                }
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
                }, "Course created successfully.");
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

        public async Task<ApiResponse<List<CourseDto>>> GetAllAsync(
        string? search, string? sortBy, bool isDescending, int pageNumber, int pageSize)
        {
            try
            {
                var user = _httpContextAccessor.HttpContext?.User;
                var role = user?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                var userIdStr = user?.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

                Console.WriteLine($"[GetAllAsync] Role: {role}, User ID: {userIdStr}");

                IQueryable<Course> query;

                if (role == "Student" && int.TryParse(userIdStr, out var studentId))
                {
                    query = _db.Enrollments
                        .Where(e => e.UserId == studentId)
                        .Include(e => e.Course).ThenInclude(c => c.Teacher)
                        .Select(e => e.Course)
                        .AsQueryable();
                }
                else
                {
                    query = _db.Courses.Include(c => c.Teacher).AsQueryable();
                }

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(c =>
                        c.Name.Contains(search) || c.Teacher.FullName.Contains(search));
                }

                query = sortBy?.ToLower() switch
                {
                    "name" => isDescending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
                    "teacher" => isDescending ? query.OrderByDescending(c => c.Teacher.FullName) : query.OrderBy(c => c.Teacher.FullName),
                    _ => query.OrderBy(c => c.Id)
                };

                var pagedCourses = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var result = pagedCourses.Select(c => new CourseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    TeacherId = c.TeacherId,
                    TeacherName = c.Teacher.FullName
                }).ToList();

                return ApiResponse<List<CourseDto>>.Success(result, "Courses retrieved");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CourseDto>>.Fail("Failed to fetch courses: " + ex.Message, 500);
            }
        }

        public async Task<ApiResponse<List<CourseDto>>> GetByTeacherAsync(int teacherId)
        {
            try
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
            catch (Exception ex)
            {
                return ApiResponse<List<CourseDto>>.Fail("Failed to fetch courses: " + ex.Message, 500);
            }
        }

        public async Task<ApiResponse<List<CourseDto>>> GetByStudentAsync(int studentId)
        {
            try
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
            catch (Exception ex)
            {
                return ApiResponse<List<CourseDto>>.Fail("Failed to fetch courses: " + ex.Message, 500);
            }
            
        }
    }
}
