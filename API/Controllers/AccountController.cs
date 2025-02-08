using System.Security.Claims;
using API.DTOs;
using API.Services;
using Application.Core;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;
        private readonly IUserAccessor _userAccessor;
        public AccountController(UserManager<AppUser> userManager, TokenService tokenService, IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);
            if (user == null) return Unauthorized();

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (result)
            {
                return CreateUserObject(user);
            }

            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("register")]

        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username)
                || await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                return BadRequest("Username is already taken");
            }

            var user = new AppUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Username
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }

            return BadRequest(result.Errors.Count() > 1 ? result.Errors : result.Errors.First());
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

            return CreateUserObject(user);
        }

        [Authorize]
        [HttpPut("edit-name")]
        public async Task<ActionResult<UserDto>> EditName(EditNameDto editNameDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

            if (user == null) return NotFound();

            user.FirstName = editNameDto.FirstName;
            user.LastName = editNameDto.LastName;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return CreateUserObject(user);

            return BadRequest("Problem updating name");
        }

        [Authorize]
        [HttpPut("edit-username")]
        public async Task<ActionResult<UserDto>> EditUsername(EditUsernameDto editUsernameDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

            if (user == null) return NotFound();

            if (await _userManager.Users.AnyAsync(x => x.UserName == editUsernameDto.Username))
            {
                return BadRequest("Username is already taken");
            }

            user.UserName = editUsernameDto.Username;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return CreateUserObject(user);

            return BadRequest("Problem updating username");
        }

        [Authorize]
        [HttpPut("edit-email")]
        public async Task<ActionResult<UserDto>> EditEmail(EditEmailDto editEmailDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

            if (user == null) return NotFound();

            if (await _userManager.Users.AnyAsync(x => x.Email == editEmailDto.Email))
            {
                return BadRequest("Email is already taken");
            }

            user.Email = editEmailDto.Email;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return CreateUserObject(user);

            return BadRequest("Problem updating email");
        }

        private UserDto CreateUserObject(AppUser user)
        {
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = _tokenService.CreateToken(user),
                Username = user.UserName,
                Email = user.Email
            };
        }

    }
}