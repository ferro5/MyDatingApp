using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.DTO;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppSettings _appSettings;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
        {
            List<string> errorList = new List<string>();
            var user = new IdentityUser
            {
                Email = userForRegisterDto.Email,
                UserName = userForRegisterDto.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, userForRegisterDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");
                return Ok(new
                    {username = user.UserName, email = user.Email, status = 1, message = "Registration Successful"});
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    errorList.Add(error.Description);
                }
            }

            return BadRequest(new JsonResult(errorList));

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.Username);
            var roles = await _userManager.GetRolesAsync(user);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
            double tokenExpireTime = Convert.ToDouble(_appSettings.ExpireTime);
            if (user != null&& await _userManager.CheckPasswordAsync(user,loginModel.Password))
            {
                var  tokenHandler = new JwtSecurityTokenHandler();
                var  tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, loginModel.Username),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier,user.Id),
                        new Claim(ClaimTypes.Role,roles.FirstOrDefault()),
                        new Claim("LoggedIn" , DateTime.Now.ToString()), 
                    }),
                    SigningCredentials = new SigningCredentials(key ,SecurityAlgorithms.HmacSha256),
                    Issuer = _appSettings.Site,
                    Audience = _appSettings.Audience,
                    Expires = DateTime.Now.AddMinutes(tokenExpireTime)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new {token = tokenHandler.WriteToken(token),expiration = token.ValidTo,username = user.UserName,userRole = roles.FirstOrDefault()});
            }
            ModelState.AddModelError("" , "Username/Password was not found");
            return Unauthorized(new
                {LoginError = "Please check the login Credentials-Invalid Username/Password was entered"});

        }
    }
}
