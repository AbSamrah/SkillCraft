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

            CreateMap<User, UserProfileDto>();
            
            
            CreateMap<AddUserRequest, User>().ReverseMap();


            CreateMap<Models.UserLogin, DataAccessLayer.Models.User>().ReverseMap();
        }
    }
}
