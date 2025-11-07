using UserStudentMgmt.Application.DTOs.Auth;
using UserStudentMgmt.Application.DTOs.Users;

namespace UserStudentMgmt.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
}