﻿using AutoMapper;
using DatingApp.API.DTO;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserForRegisterDto>();
            CreateMap<UserForRegisterDto, User>();
        }
    }
}
