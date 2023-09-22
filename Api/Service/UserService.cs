using Api.Dtos;
using Api.Helpers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Api.Services;

public class UserService : IUserService
{
    private readonly JWT _jwt;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(IUnitOfWork unitOfWork, IOptions<JWT> jwt,
        IPasswordHasher<User> passwordHasher)
    {
        _jwt = jwt.Value;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<string> RegisterAsync(User user)
    {
        var usuario = new User
        {
            Name = user.Name,
            LastName = user.LastName,
            MiddleName = user.MiddleName,
            Email = user.Email,
        };

        usuario.Password = _passwordHasher.HashPassword(usuario, user.Password);

        var usuarioExiste = _unitOfWork.User
                                    .Find(u => u.Email.ToLower() == user.Email.ToLower())
                                    .FirstOrDefault();

        if (usuarioExiste == null)
        {
            //var rolPredeterminado = _unitOfWork.Roles
            //                        .Find(u => u.Nombre == Autorizacion.rol_predeterminado.ToString())
            //                        .First();
            try
            {
                //usuario.Roles.Add(rolPredeterminado);
                _unitOfWork.User.Add(usuario);
                await _unitOfWork.SaveAsync();

                return $"El usuario  {user.Email} ha sido registrado exitosamente";
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return $"Error: {message}";
            }
        }
        else
        {
            return $"El usuario con {user.Email} ya se encuentra registrado.";
        }
    }


    public async Task<DatosUsuarioDto> GetTokenAsync(LoginDto model)
    {
        DatosUsuarioDto datosUsuarioDto = new DatosUsuarioDto();
        var usuario = await _unitOfWork.User
                    .GetByUsernameAsync(model.Email);

        if (usuario == null)
        {
            datosUsuarioDto.EstaAutenticado = false;
            datosUsuarioDto.Mensaje = $"No existe ningún usuario con el email {model.Email}.";
            return datosUsuarioDto;
        }

        var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, model.Password);

        if (resultado == PasswordVerificationResult.Success)
        {
            datosUsuarioDto.EstaAutenticado = true;
            JwtSecurityToken jwtSecurityToken = CreateJwtToken(usuario);
            datosUsuarioDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            datosUsuarioDto.Email = usuario.Email;
            datosUsuarioDto.UserName = usuario.Email;
            //datosUsuarioDto.Roles = usuario.Roles
            //                                .Select(u => u.Nombre)
            //                                .ToList();
            return datosUsuarioDto;
        }
        datosUsuarioDto.EstaAutenticado = false;
        datosUsuarioDto.Mensaje = $"Credenciales incorrectas para el usuario {usuario.Email}.";
        return datosUsuarioDto;
    }

    //public async Task<string> AddRoleAsync(AddRoleDto model)
    //{

    //    var usuario = await _unitOfWork.Usuarios
    //                .GetByUsernameAsync(model.Username);

    //    if (usuario == null)
    //    {
    //        return $"No existe algún usuario registrado con la cuenta {model.Username}.";
    //    }


    //    var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, model.Password);

    //    if (resultado == PasswordVerificationResult.Success)
    //    {


    //        var rolExiste = _unitOfWork.Roles
    //                                    .Find(u => u.Nombre.ToLower() == model.Role.ToLower())
    //                                    .FirstOrDefault();

    //        if (rolExiste != null)
    //        {
    //            var usuarioTieneRol = usuario.Roles
    //                                        .Any(u => u.Id == rolExiste.Id);

    //            if (usuarioTieneRol == false)
    //            {
    //                usuario.Roles.Add(rolExiste);
    //                _unitOfWork.Usuarios.Update(usuario);
    //                await _unitOfWork.SaveAsync();
    //            }

    //            return $"Rol {model.Role} agregado a la cuenta {model.Username} de forma exitosa.";
    //        }

    //        return $"Rol {model.Role} no encontrado.";
    //    }
    //    return $"Credenciales incorrectas para el usuario {usuario.Username}.";
    //}



    private JwtSecurityToken CreateJwtToken(User usuario)
    {
        //var roles = usuario.Roles;
        var roleClaims = new List<Claim>();
        //foreach (var role in roles)
        //{
        //    roleClaims.Add(new Claim("roles", role.Nombre));
        //}
        var claims = new[]
        {
                                new Claim(JwtRegisteredClaimNames.Sub, usuario.Email),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                                new Claim("uid", usuario.Id.ToString())
                        }
        .Union(roleClaims);
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;
    }

    public async Task<string> UpdatePasswordAsync(UpdatePasswordDto model)
    {




        var usuarioExiste = _unitOfWork.User
                                    .Find(u => u.Email.ToLower() == model.Email.ToLower())
                                    .FirstOrDefault();
        if (usuarioExiste == null) return $"No se ha podido actualizar la contraseña";
        var resultado = _passwordHasher.VerifyHashedPassword(usuarioExiste, usuarioExiste.Password, model.OldPassword);


        if (resultado == PasswordVerificationResult.Success)
        {
            usuarioExiste.Password = _passwordHasher.HashPassword(usuarioExiste, model.NewPassword);
            try
            {
                //usuario.Roles.Add(rolPredeterminado);
                _unitOfWork.User.Update(usuarioExiste);
                await _unitOfWork.SaveAsync();

                return $"La contraseña del email  {model.Email} ha sido actualizada exitosamente";
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return $"Error: {message}";
            }
        }
        else
        {
            return $"Credenciales incorrectas para el usuario {model.Email}";
        }


    }
}
