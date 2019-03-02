using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTO;
using DatingApp.API.Models;
using DatingApp.API.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
  
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;


        public AuthController(IAuthRepository authRepository, IJwtTokenGenerator jwtTokenGenerator, Mapper mapper)
        {
            _authRepository = authRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }

    
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {

            return Ok(
                new
                {
                    token = await _jwtTokenGenerator.GenerateJwtTokenString(userForLoginDto.Username,
                    userForLoginDto.Password)
                });
        }
        [HttpGet("log")]
        public IActionResult Log()
        {
            string testString = "test";
            return Ok(testString);

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            try
            {
                if (await _authRepository.UserExist(userForRegisterDto.UserName))
                    return BadRequest("user is already exist");

                var user = _mapper.Map<User>(userForRegisterDto);
                await _authRepository.Register(user, userForRegisterDto.Password);
                return Ok("The registration is Successful");
            }
            catch (Exception e)
            {
                return Ok("Something went wrong");
            }
        }
    }
}