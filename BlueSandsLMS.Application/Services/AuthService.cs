
using BlueSandsLMS.Common.DTOs;
using BlueSandsLMS.Core.Entities;
using BlueSandsLMS.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlueSandsLMS.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly BlueSandsLMSDbContext _db;
        private readonly IConfiguration       _config;

        public AuthService(BlueSandsLMSDbContext db, IConfiguration config)
        {
            _db     = db;
            _config = config;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterUserDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Email already exists");

            var studentRole = await _db.Roles.FirstAsync(r => r.Name == "Student");

            var user = new User
            {
                Id           = Guid.NewGuid(),
                FullName     = dto.FullName,
                Email        = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RoleId       = studentRole.Id,
                IsActive     = true,
                DateCreated  = DateTime.UtcNow
                // SchoolId left null → assigned later by Admin
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return GenerateAuthResponse(user);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _db.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email && u.IsActive);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            user.LastLogin = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return GenerateAuthResponse(user);
        }

        public async Task<AuthResponseDto> AdminCreateUserAsync(AdminCreateUserDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Email already exists");

            var user = new User
            {
                Id           = Guid.NewGuid(),
                FullName     = dto.FullName,
                Email        = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RoleId       = dto.RoleId,
                SchoolId     = dto.SchoolId,
                IsActive     = true,
                DateCreated  = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return GenerateAuthResponse(user);
        }

        private AuthResponseDto GenerateAuthResponse(User user)
{
    var secret   = _config["Jwt:Secret"]   ?? throw new InvalidOperationException("Jwt:Secret not configured");
    var issuer   = _config["Jwt:Issuer"]   ?? string.Empty;
    var audience = _config["Jwt:Audience"] ?? string.Empty;
    var key      = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

    var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim("FullName", user.FullName ?? string.Empty),
        new Claim(ClaimTypes.Role, user.Role?.Name ?? string.Empty)
    };

    // Add SchoolId claim only if it has a value and is not Guid.Empty
    if (user.SchoolId.HasValue && user.SchoolId.Value != Guid.Empty)
        claims.Add(new Claim("SchoolId", user.SchoolId.Value.ToString()));

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(claims),
        Issuer = issuer,
        Audience = audience,
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
    };

    var handler = new JwtSecurityTokenHandler();
    var securityToken = handler.CreateToken(tokenDescriptor);
    var tokenString = handler.WriteToken(securityToken);

    return new AuthResponseDto
    {
        Token    = tokenString,
        FullName = user.FullName,
        Role     = user.Role?.Name ?? string.Empty,
        UserId   = user.Id,
        SchoolId = user.SchoolId // keep this Guid? for flexibility
    };
}

    }
}
