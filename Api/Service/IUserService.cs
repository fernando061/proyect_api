using Api.Dtos;
using Core.Entities;

namespace Api.Services;
public interface IUserService
{
    Task<string> RegisterAsync(User model);
    Task<string> UpdatePasswordAsync(UpdatePasswordDto model);
    Task<DatosUsuarioDto> GetTokenAsync(LoginDto model);

    Task<string> AddRoleAsync(AddRoleDto model);
    Task<DatosUsuarioDto> RefreshTokenAsync(string refreshToken);

}
