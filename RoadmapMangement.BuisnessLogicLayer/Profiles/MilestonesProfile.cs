using AutoMapper;
using RoadmapMangement.BuisnessLogicLayer.Models;
using RoadmapMangement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.BuisnessLogicLayer.Profiles
{
    public class MileStonesProfile : Profile
    {
        public MileStonesProfile()
        {
            CreateMap<Milestone, MilestoneDto>();

            CreateMap<AddMilestoneRequest, Milestone>();

            CreateMap<UpdateMilestoneRequest, Milestone>();

        }
    }
}
