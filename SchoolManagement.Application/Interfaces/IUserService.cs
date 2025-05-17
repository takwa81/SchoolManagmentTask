using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Application.DTOs.User;
using SchoolManagement.Application.Responses;

namespace SchoolManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<List<UserDto>>> GetUsersByRoleAsync(string role, string? search, string? sortBy, bool isDescending, int pageNumber, int pageSize);
        Task<ApiResponse<UserDto>> GetUserByIdAsync(int id);
    }

}
