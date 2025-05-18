using AutoMapper;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersManagement.BuissnessLogicLayer.Models;

namespace UsersManagement.BuissnessLogicLayer.Profiles
{
    public class RolesProfile: Profile
    {
        public RolesProfile() 
        {
            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title));
        }
    }
}
