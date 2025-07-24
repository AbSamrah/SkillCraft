﻿using AutoMapper;
using MongoDB.Driver;
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
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IRepository<Step> _stepsRepository;

        public MilestonesService(IMilestoneRepository milestonesRepository, IUnitOfWork uow, IMapper mapper, IRepository<Step> stepsRepository)
        {
            _milestonesRepository = milestonesRepository;
            _uow = uow;
            _mapper = mapper;
            _stepsRepository = stepsRepository;
        }

        public async Task<List<MilestoneDto>> GetAll()
        {
            var steps = await _milestonesRepository.GetAll();
            return _mapper.Map<List<MilestoneDto>>(steps);
        }

        public async Task<MilestoneDto> Add(AddMilestoneRequest addMilestoneRequest)
        {
            Milestone milestone = _mapper.Map<Milestone>(addMilestoneRequest);
            /*Console.WriteLine(milestone.StepsIds.Count);
            foreach (string step in addMilestoneRequest.StepsIds)
            {
                milestone.StepsIds.Add(step);
            }*/

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
                throw new Exception("Milestone not found.");
            }

            MilestoneDto milestoneDto = _mapper.Map<MilestoneDto>(milestone);

            _milestonesRepository.Remove(milestone.Id);
            await _uow.Commit();
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
            existingMilestone.IsCompleted = updateMilestoneRequest.IsCompleted;
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
