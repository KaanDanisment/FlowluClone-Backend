using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace FlowluCloneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDto userLoginDto)
        {
            var login = _authService.Login(userLoginDto);
            if (!login.Success)
            {
                return BadRequest(login.Message);
            }
            var token = _authService.CreateAccessToken(login.Data);
            if (token.Success)
            {
                return Ok(token.Data);
            }
            return BadRequest(token.Message);
        }

        [HttpPost("register")]
        public IActionResult Register(UserRegisterDto userRegisterDto)
        {
            var userExist = _authService.UserExist(userRegisterDto.Email);
            if (!userExist.Success)
            {
                return BadRequest(userExist.Message);
            }
            var register = _authService.Register(userRegisterDto,userRegisterDto.Password);
            var token = _authService.CreateAccessToken(register.Data);
            if (token.Success)
            {
                return Ok(token.Data);
            }
            return BadRequest(token.Message);
        }
    }
}
