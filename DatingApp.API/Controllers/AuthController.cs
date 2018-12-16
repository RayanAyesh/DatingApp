using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _cong;
        public AuthController(IAuthRepository repo, IConfiguration cong)
        {
            _cong = cong;
            _repo = repo;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterForDtos userRegisterForDtos)
        {
            userRegisterForDtos.Username = userRegisterForDtos.Username.ToLower();
            if (await _repo.UserExists(userRegisterForDtos.Username))
                return BadRequest("user already exitsts");
            var UserToCreate = new User()
            {
                Username = userRegisterForDtos.Username
            };
            var createuser = await _repo.Register(UserToCreate, userRegisterForDtos.Password);
            return StatusCode(201);

        }
        [HttpPost("login")]
        public async Task<IActionResult> login(UserLoginForDtos userLoginForDtos)
        {
          //  throw new Exception("Computer says no!");
            var userlo = await _repo.Login(userLoginForDtos.Username.ToLower(), userLoginForDtos.Password);
            if (userlo == null)
                return Unauthorized();
            var Claims = new[]{
                new Claim(ClaimTypes.NameIdentifier,userlo.Id.ToString()),
                new Claim(ClaimTypes.Name,userlo.Username.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cong.GetSection("AppSetting:Token").Value));
            
            var creds =new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
            
            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject =new ClaimsIdentity(Claims),
                Expires= DateTime.Now.AddDays(1),
                SigningCredentials=creds
            };
            var TokenHandler = new JwtSecurityTokenHandler();
            var token=TokenHandler.CreateToken(TokenDescriptor);
            return Ok( new{
                token=TokenHandler.WriteToken(token)
            });

        }
    }
}