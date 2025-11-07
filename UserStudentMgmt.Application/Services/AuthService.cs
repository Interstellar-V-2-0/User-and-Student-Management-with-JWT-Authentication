using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserStudentMgmt.Application.DTOs.Auth;
using UserStudentMgmt.Application.Interfaces;
using UserStudentMgmt.Domain.Entities;
using UserStudentMgmt.Domain.Interfaces;

namespace UserStudentMgmt.Application.Services;

public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepo, IConfiguration config, IMapper mapper)
        {
            _userRepo = userRepo;
            _config = config;
            _mapper = mapper;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            var existing = await _userRepo.GetByUserNameAsync(request.UserName);
            if (existing != null)
                throw new InvalidOperationException("User already exists.");

            var user = _mapper.Map<User>(request);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            await _userRepo.AddAsync(user);

            return GenerateJwtToken(user, "User");
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepo.GetByUserNameAsync(request.UserName);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials.");

            return GenerateJwtToken(user, "User");
        }

        private AuthResponseDto GenerateJwtToken(User user, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(4),
                signingCredentials: creds
            );

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserName = user.UserName,
                Role = role
            };
    }
}