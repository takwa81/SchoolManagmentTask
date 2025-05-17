using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Enums;

namespace SchoolManagement.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Users.Any())
            {
                var admin = new User
                {
                    FullName = "Super Admin",
                    Email = "admin@school.com",
                    Role = UserRole.Admin,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
                };

                var teacher = new User
                {
                    FullName = "Teacher",
                    Email = "teacher@school.com",
                    Role = UserRole.Teacher,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
                };

                var student = new User
                {
                    FullName = "Takwa Student",
                    Email = "student@school.com",
                    Role = UserRole.Student,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
                };

                context.Users.AddRange(admin, teacher, student);
                context.SaveChanges();

                Console.WriteLine("✅ Seeded users:");
                Console.WriteLine("- Admin: admin@school.com / password123");
                Console.WriteLine("- Teacher: teacher@school.com / password123");
                Console.WriteLine("- Student: student@school.com / password123");

                var teacherId = teacher.Id;

                var courses = new List<Course>
                {
                    new Course { Name = "Mathematics", TeacherId = teacherId },
                    new Course { Name = "Physics", TeacherId = teacherId },
                    new Course { Name = "Computer Science", TeacherId = teacherId }
                };

                context.Courses.AddRange(courses);
                context.SaveChanges();

                Console.WriteLine("✅ Seeded courses assigned to teacher:");
                foreach (var c in courses)
                {
                    Console.WriteLine($"- {c.Name}");
                }
            }
        }

    }
}
