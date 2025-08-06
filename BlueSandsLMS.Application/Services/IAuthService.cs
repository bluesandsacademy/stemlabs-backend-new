using BlueSandsLMS.Common.DTOs;

namespace BlueSandsLMS.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterUserDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<AuthResponseDto> AdminCreateUserAsync(AdminCreateUserDto dto);

    }
}
