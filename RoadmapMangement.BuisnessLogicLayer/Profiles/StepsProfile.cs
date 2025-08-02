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
    public class StepsProfile: Profile
    {
        public StepsProfile()
        {
            CreateMap<Step, StepDto>();

            CreateMap<AddStepRequest, Step>();

            CreateMap<UpdateStepRequest, Step>();
        }
    }
}
