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

        public async Task<ApiResponse<List<UserDto>>> GetUsersByRoleAsync(string role, string? search, string? sortBy, bool isDescending, int pageNumber, int pageSize)
        {
            try
            {
                var query = _db.Users
                    .Where(u => u.Role.ToString() == role);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(u => u.FullName.Contains(search) || u.Email.Contains(search));
                }

                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    query = sortBy.ToLower() switch
                    {
                        "fullname" => isDescending ? query.OrderByDescending(u => u.FullName) : query.OrderBy(u => u.FullName),
                        "email" => isDescending ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                        _ => query
                    };
                }

                var users = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        FullName = u.FullName,
                        Email = u.Email,
                        Role = u.Role.ToString()
                    })
                    .ToListAsync();

                return ApiResponse<List<UserDto>>.Success(users, "Users fetched");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<UserDto>>.Fail("Failed to fetch users: " + ex.Message, 500);
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
