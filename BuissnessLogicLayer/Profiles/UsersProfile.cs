using AutoMapper;
using BuissnessLogicLayer.Models;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuissnessLogicLayer.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {

            CreateMap<User, UpdateUserRequest>()
              .ForMember(dest => dest.Role,
              opt => opt.MapFrom(src => src.Role.Title));
            
            CreateMap<User, UserDto>().ForMember(dest => dest.Role,
              opt => opt.MapFrom(src => src.Role.Title));

            CreateMap<AddUserRequest, User>().ReverseMap();


            CreateMap<UserLogin, User>().ReverseMap();
        }
    }
}
