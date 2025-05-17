using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SchoolManagement.Application.DTOs.Auth;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Application.Responses;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace SchoolManagement.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest request)
        {
            try
            {
                if (await _db.Users.AnyAsync(u => u.Email == request.Email))
                    return ApiResponse<AuthResponse>.Fail("Email already registered", 400);

                var user = new User
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    Role = request.Role,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
                };

                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();

                var token = GenerateJwtToken(user);

                return ApiResponse<AuthResponse>.Success(new AuthResponse
                {
                    Email = user.Email,
                    Role = user.Role.ToString(),
                    Token = "Bearer " + token
                }, "Registration successful");
            }
            catch (Exception ex)
            {
                return ApiResponse<AuthResponse>.Fail("Registration failed: " + ex.Message);
            }
        }

        public async Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                    return ApiResponse<AuthResponse>.Fail("Invalid credentials", 401);

                var token = GenerateJwtToken(user);

                return ApiResponse<AuthResponse>.Success(new AuthResponse
                {
                    Email = user.Email,
                    Role = user.Role.ToString(),
                    Token = "Bearer " + token
                }, "Login successful");
            }
            catch (Exception ex)
            {
                return ApiResponse<AuthResponse>.Fail("Login failed: " + ex.Message);
            }
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email), 
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("id", user.Id.ToString())
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    

    public async Task<ApiResponse<UserProfileDto>> GetProfileAsync(string userEmail)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        if (user == null)
            return ApiResponse<UserProfileDto>.Fail("User not found", 404);

        return ApiResponse<UserProfileDto>.Success(new UserProfileDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString()
        }, "Profile retrieved");
    }
    }
}
