using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Application.DTOs.User;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Application.Responses;
using SchoolManagement.Infrastructure.Data;

namespace SchoolManagement.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;

        public UserService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ApiResponse<List<UserDto>>> GetUsersByRoleAsync(string role)
        {
            try
            {
                var normalizedRole = role.ToLower();
                var users = await _db.Users
                    .Where(u => u.Role.ToString().ToLower() == normalizedRole)
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        FullName = u.FullName,
                        Email = u.Email,
                        Role = u.Role.ToString()
                    })
                    .ToListAsync();

                return ApiResponse<List<UserDto>>.Success(users, "Users retrieved");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<UserDto>>.Fail("Failed to retrieve users: " + ex.Message);
            }
        }

        public async Task<ApiResponse<UserDto>> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _db.Users.FindAsync(id);
                if (user == null)
                    return ApiResponse<UserDto>.Fail("User not found", 404);

                return ApiResponse<UserDto>.Success(new UserDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = user.Role.ToString()
                }, "User retrieved");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.Fail("Error retrieving user: " + ex.Message);
            }
        }
    }

}
