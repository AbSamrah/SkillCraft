using AutoMapper;
using RoadmapMangement.BuisnessLogicLayer.Models;
using RoadmapMangement.DataAccessLayer.Interfaces;
using RoadmapMangement.DataAccessLayer.Models;
using RoadmapMangement.DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.BuisnessLogicLayer.Services
{
    public class RoadmapsService
    {
        private readonly IRoadmapRepository _roadmapsRepository;
        //private readonly IRepository<Milestone> _milestoneRepository;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public RoadmapsService(IRoadmapRepository roadmapsRepository, IUnitOfWork uow, IMapper mapper/*, IRepository<Milestone> milestoneRepository*/)
        {
            _roadmapsRepository = roadmapsRepository;
            //_milestoneRepository = milestoneRepository;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<RoadmapDto>> GetAll()
        {
            var roadmaps = await _roadmapsRepository.GetAll();
            return _mapper.Map<List<RoadmapDto>>(roadmaps);
        }

        public async Task<RoadmapDto> Add(AddRoadmapRequest addRoadmapRequest)
        {
            Roadmap roadmap = _mapper.Map<Roadmap>(addRoadmapRequest);
            foreach (string milestone in addRoadmapRequest.MilestonesIds)
            {
                roadmap.MilestoneIds.Add(milestone);
            }
            _roadmapsRepository.Add(roadmap);
            await _uow.Commit();
            RoadmapDto roadmapDto = _mapper.Map<RoadmapDto>(roadmap);

            return roadmapDto;
        }

        public async Task<RoadmapDto> Get(string id)
        {
            var roadmap = await _roadmapsRepository.GetById(id);
            if (roadmap is null)
            {
                throw new Exception("Roadmap not found.");
            }

            RoadmapDto roadmapDto = _mapper.Map<RoadmapDto>(roadmap);

            return roadmapDto;
        }


        public async Task<RoadmapDto> DeleteAsync(string id)
        {
            var roadmap = await _roadmapsRepository.GetById(id);

            if (roadmap is null)
            {
                throw new Exception("Roadmap not found.");
            }

            RoadmapDto roadmapDto = _mapper.Map<RoadmapDto>(roadmap);

            _roadmapsRepository.Remove(roadmap.Id);
            await _uow.Commit();
            return roadmapDto;
        }

        public async Task<RoadmapDto> UpdateAsync(string id,UpdateRoadmapRequest updateRoadmapRequest)
        {
            var existingRoadmap = await _roadmapsRepository.GetById(id);

            if (existingRoadmap is null)
            {
                throw new Exception("Roadmap not found.");
            }
            existingRoadmap.Duration = updateRoadmapRequest.Duration;
            existingRoadmap.Salary = updateRoadmapRequest.Salary;
            existingRoadmap.Tags = updateRoadmapRequest.Tags;
            existingRoadmap.Description = updateRoadmapRequest.Description;
            existingRoadmap.IsActive = updateRoadmapRequest.IsActive;
            existingRoadmap.Name = updateRoadmapRequest.Name;
            existingRoadmap.MilestoneIds.Clear();
            
            foreach (string milestone in updateRoadmapRequest.MilestonesIds)
            {
                existingRoadmap.MilestoneIds.Add(milestone);
            }

            await _roadmapsRepository.Update(existingRoadmap);
            await _uow.Commit();

            RoadmapDto roadmapDto = _mapper.Map<RoadmapDto>(existingRoadmap);

            return roadmapDto;


        }

    }
}
