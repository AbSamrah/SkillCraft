﻿using AutoMapper;
using RoadmapMangement.BuisnessLogicLayer.Models;
using RoadmapMangement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.BuisnessLogicLayer.Services
{
    public class ManualRoadmapCreationStrategy: IRoadmapCreationStrategy
    {
        private readonly IMapper _mapper;

        public ManualRoadmapCreationStrategy(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task<Roadmap> CreateRoadmap(object parameters)
        {
            if (parameters is not AddRoadmapRequest addRoadmapRequest)
            {
                throw new ArgumentException("Invalid parameter type for ManualRoadmapCreationStrategy. Expected AddRoadmapRequest.", nameof(parameters));
            }

            var roadmap = _mapper.Map<Roadmap>(addRoadmapRequest);
            return Task.FromResult(roadmap);
        }
    }
}
