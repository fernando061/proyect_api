using Api.Dtos;
using Api.Services;
using API.Controllers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[Authorize]
public class UserController : BaseApiController
{
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserController(IUserService userService, IUnitOfWork unitOfWork,IMapper mapper)
    {
        _userService = userService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> RegisterAsync(User model)
    {
        var existUser = await _unitOfWork.User.ExistsAsync(e => e.Email == model.Email);
        if (existUser) return Conflict(new { message = "El email ya existe" });
        var result = await _userService.RegisterAsync(model);
        return StatusCode(StatusCodes.Status201Created, new { message = true, data = result });

    }

    [HttpPost("token")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTokenAsync(LoginDto model)
    {
        var result = await _userService.GetTokenAsync(model);
        SetRefreshTokenInCookie(result.RefreshToken);
        return Ok(result);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] User model)
    {
        var user = _unitOfWork.User.Find(f => f.Id == id).FirstOrDefault();
        if (user == null) return BadRequest();
        user.Name = model.Name;
        user.LastName = model.LastName;
        user.MiddleName = model.MiddleName;

        var email = _unitOfWork.User.Find(f => f.Email == model.Email && f.Id != id).FirstOrDefault();
        if (email != null) return BadRequest();
        user.Email = model.Email;
        _unitOfWork.User.Update(user);
        await _unitOfWork.SaveAsync();

        return Ok(new { message = "Usuario actualizado" });
    }
    [HttpDelete("Delete/{email}")]
    public async Task<IActionResult> DeleteUser(string email)
    {
        var user = _unitOfWork.User.Find(f => f.Email == email).FirstOrDefault();
        _unitOfWork.User.Remove(user);
        await _unitOfWork.SaveAsync();
        return Ok(new { message = "Usuario eliminado" });
    }
    [HttpGet("{email}")]
    public async Task<IActionResult> GetByIdUser(string email)
    {
        var user = _unitOfWork.User.Find(f => f.Email == email).FirstOrDefault();
        if (user == null) return NoContent();
        return Ok(new { message = "Usuario encontrado", data = _mapper.Map<UserDto>(user) });
    }

    [HttpPut("UpdatePassword")]
    public async Task<IActionResult> UpdaPassword(UpdatePasswordDto updatePassword)
    {
        var email = _unitOfWork.User.Find(f => f.Email == updatePassword.Email).FirstOrDefault();
        if (email == null) return BadRequest();

        var result = await _userService.UpdatePasswordAsync(updatePassword);

        return Ok(new { message = result });
    }



    [HttpPost("addrole")]
    [AllowAnonymous]
    public async Task<IActionResult> AddRoleAsync(AddRoleDto model)
    {
        var result = await _userService.AddRoleAsync(model);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var response = await _userService.RefreshTokenAsync(refreshToken);
        if (!string.IsNullOrEmpty(response.RefreshToken))
            SetRefreshTokenInCookie(response.RefreshToken);
        return Ok(response);
    }


    private void SetRefreshTokenInCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(10),
        };
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }


}