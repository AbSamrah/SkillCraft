using AutoMapper;
using RoadmapMangement.BuisnessLogicLayer.Filters;
using RoadmapMangement.BuisnessLogicLayer.Models;
using RoadmapMangement.DataAccessLayer.Interfaces;
using RoadmapMangement.DataAccessLayer.Models;
using RoadmapMangement.DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.BuisnessLogicLayer.Services
{
    public class StepsService
    {
        private readonly IRepository<Step> _stepsRepository;
        private readonly IMilestoneRepository _milestoneRepository;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public StepsService(IRepository<Step> stepsRepository, IMilestoneRepository milestoneRepository, IUnitOfWork uow, IMapper mapper)
        {
            _stepsRepository = stepsRepository;
            _milestoneRepository = milestoneRepository;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<StepDto>> GetAll(EntityFilter filter)
        {
            var steps = await _stepsRepository.GetAll(filter.Name, filter.PageNumber, filter.PageSize);
            return _mapper.Map<List<StepDto>>(steps);
        }

        public async Task<StepDto> Add(AddStepRequest addStepRequest)
        {
            Step step = _mapper.Map<Step>(addStepRequest);

            _stepsRepository.Add(step);
            await _uow.Commit();
            StepDto stepDto = _mapper.Map<StepDto>(step);

            return stepDto;
        }

        public async Task<StepDto> Get(string id)
        {
            var step = await _stepsRepository.GetById(id);
            if (step is null)
            {
                throw new Exception("Step not found.");
            }
            StepDto stepDto = _mapper.Map<StepDto>(step);

            return stepDto;
        }

        public async Task<StepDto> DeleteAsync(string id)
        {
            var step = await _stepsRepository.GetById(id);
            if (step is null)
            {
                throw new System.Exception("Step not found.");
            }

            _stepsRepository.Remove(step.Id);

            var milestonesToUpdate = (await _milestoneRepository.GetAll())
                .Where(m => m.StepsIds.Contains(id));

            foreach (var milestone in milestonesToUpdate)
            {
                milestone.StepsIds.Remove(id);
                await _milestoneRepository.Update(milestone);
            }

            await _uow.Commit();
            StepDto stepDto = _mapper.Map<StepDto>(step);
            return stepDto;
        }

        public async Task<StepDto> UpdateAsync(string id,UpdateStepRequest updateStepRequest)
        {
            var existingStep = await _stepsRepository.GetById(id);

            if (existingStep is null)
            {
                throw new Exception("Step not found.");
            }
            existingStep.Description = updateStepRequest.Description;
            existingStep.Name = updateStepRequest.Name;
            existingStep.DurationInMinutes = updateStepRequest.DurationInMinutes;

            await _stepsRepository.Update(existingStep);
            await _uow.Commit();

            StepDto stepDto = _mapper.Map<StepDto>(existingStep);

            return stepDto;


        }

    }
}
