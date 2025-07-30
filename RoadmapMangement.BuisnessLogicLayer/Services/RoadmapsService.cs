using AutoMapper;
using RoadmapMangement.BuisnessLogicLayer.Filters;
using RoadmapMangement.BuisnessLogicLayer.Models;
using RoadmapMangement.DataAccessLayer.Interfaces;
using RoadmapMangement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoadmapMangement.BuisnessLogicLayer.Services
{
    /// <summary>
    /// Service for managing roadmap operations.
    /// </summary>
    public class RoadmapsService : IRoadmapsService
    {
        private readonly IRoadmapRepository _roadmapsRepository;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public RoadmapsService(IRoadmapRepository roadmapsRepository, IUnitOfWork uow, IMapper mapper)
        {
            _roadmapsRepository = roadmapsRepository;
            _uow = uow;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds a new roadmap by delegating its creation to the provided strategy.
        /// </summary>
        public async Task<RoadmapDto> Add(IRoadmapCreationStrategy strategy, object parameters)
        {
            var roadmap = await strategy.CreateRoadmap(parameters);

            _roadmapsRepository.Add(roadmap);
            await _uow.Commit();
            return _mapper.Map<RoadmapDto>(roadmap);
        }

        public async Task<RoadmapDto> DeleteAsync(string id)
        {
            var roadmap = await _roadmapsRepository.GetById(id);

            if (roadmap is null)
            {
                throw new KeyNotFoundException("Roadmap not found.");
            }

            var roadmapDto = _mapper.Map<RoadmapDto>(roadmap);
            _roadmapsRepository.Remove(roadmap.Id);
            await _uow.Commit();
            return roadmapDto;
        }

        public async Task<RoadmapDto> Get(string id)
        {
            var roadmap = await _roadmapsRepository.GetById(id);
            if (roadmap is null)
            {
                throw new KeyNotFoundException("Roadmap not found.");
            }
            var roadmapDto = _mapper.Map<RoadmapDto>(roadmap);
            return roadmapDto;
        }

        public async Task<List<RoadmapDto>> GetAll(EntityFilter filter)
        {
            var roadmaps = await _roadmapsRepository.GetAll(filter.Name, filter.PageNumber, filter.PageSize);
            return _mapper.Map<List<RoadmapDto>>(roadmaps);
        }

        public async Task<RoadmapDto> UpdateAsync(string id, UpdateRoadmapRequest updateRoadmapRequest)
        {
            var existingRoadmap = await _roadmapsRepository.GetById(id);

            if (existingRoadmap is null)
            {
                throw new KeyNotFoundException("Roadmap not found.");
            }

            // Use AutoMapper to update the existing entity from the request object
            _mapper.Map(updateRoadmapRequest, existingRoadmap);

            await _roadmapsRepository.Update(existingRoadmap);
            await _uow.Commit();

            return _mapper.Map<RoadmapDto>(existingRoadmap);
        }
    }
}
