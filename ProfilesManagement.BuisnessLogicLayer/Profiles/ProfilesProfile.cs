using AutoMapper;
using ProfilesManagement.BuisnessLogicLayer.Models;
using ProfilesManagement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfilesManagement.BuisnessLogicLayer.Profiles
{
    public class ProfilesProfile: Profile
    {
        public ProfilesProfile()
        {
            CreateMap<RoadmapStatus, RoadmapStatusDto>();
        }
    }
}
