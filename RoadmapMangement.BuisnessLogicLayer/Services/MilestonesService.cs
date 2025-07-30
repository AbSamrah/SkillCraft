using AutoMapper;
using MongoDB.Driver;
using RoadmapMangement.BuisnessLogicLayer.Filters;
using RoadmapMangement.BuisnessLogicLayer.Models;
using RoadmapMangement.DataAccessLayer.Interfaces;
using RoadmapMangement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.BuisnessLogicLayer.Services
{
    public class MilestonesService
    {
        private readonly IMilestoneRepository _milestonesRepository;
        private readonly IRoadmapRepository _roadmapRepository;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IRepository<Step> _stepsRepository;

        public MilestonesService(IMilestoneRepository milestonesRepository, IRoadmapRepository roadmapRepository,IUnitOfWork uow, IMapper mapper, IRepository<Step> stepsRepository)
        {
            _milestonesRepository = milestonesRepository;
            _roadmapRepository = roadmapRepository;
            _uow = uow;
            _mapper = mapper;
            _stepsRepository = stepsRepository;
        }

        public async Task<List<MilestoneDto>> GetAll(EntityFilter filter)
        {
            var steps = await _milestonesRepository.GetAll(filter.Name, filter.PageNumber, filter.PageSize);
            return _mapper.Map<List<MilestoneDto>>(steps);
        }

        public async Task<MilestoneDto> Add(AddMilestoneRequest addMilestoneRequest)
        {
            Milestone milestone = _mapper.Map<Milestone>(addMilestoneRequest);

            _milestonesRepository.Add(milestone);
            await _uow.Commit();
            MilestoneDto milestoneDto = _mapper.Map<MilestoneDto>(milestone);

            return milestoneDto;
        }

        public async Task<MilestoneDto> Get(string id)
        {
            var milestone = await _milestonesRepository.GetById(id);
            if (milestone is null)
            {
                throw new Exception("Milestone not found.");
            }
            return _mapper.Map<MilestoneDto>(milestone);
        }

        public async Task<MilestoneDto> DeleteAsync(string id)
        {
            var milestone = await _milestonesRepository.GetById(id);
            if (milestone is null)
            {
                throw new System.Exception("Milestone not found.");
            }

            // Remove the milestone itself
            _milestonesRepository.Remove(milestone.Id);

            // Find all roadmaps that contain this milestone and remove the reference
            var roadmapsToUpdate = (await _roadmapRepository.GetAll())
                .Where(r => r.MilestonesIds.Contains(id));

            foreach (var roadmap in roadmapsToUpdate)
            {
                roadmap.MilestonesIds.Remove(id);
                await _roadmapRepository.Update(roadmap);
            }

            await _uow.Commit();

            MilestoneDto milestoneDto = _mapper.Map<MilestoneDto>(milestone);
            return milestoneDto;
        }

        public async Task<MilestoneDto> UpdateAsync(string id,UpdateMilestoneRequest updateMilestoneRequest)
        {
            var existingMilestone = await _milestonesRepository.GetById(id);

            if (existingMilestone is null)
            {
                throw new Exception("Milestone not found.");
            }

            existingMilestone.Name = updateMilestoneRequest.Name;
            existingMilestone.Description = updateMilestoneRequest.Description;
            existingMilestone.StepsIds.Clear();
            foreach (string step in updateMilestoneRequest.StepsIds)
            {
                existingMilestone.StepsIds.Add(step);
            }

            await _milestonesRepository.Update(existingMilestone);
            await _uow.Commit();

            MilestoneDto milestoneDto = _mapper.Map<MilestoneDto>(existingMilestone);

            return milestoneDto;


        }

        


    }
}
