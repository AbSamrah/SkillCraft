using AutoMapper;
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
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public StepsService(IRepository<Step> stepsRepository, IUnitOfWork uow, IMapper mapper)
        {
            _stepsRepository = stepsRepository;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<StepDto>> GetAll()
        {
            var steps = await _stepsRepository.GetAll();
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
                throw new Exception("Step not found.");
            }


            _stepsRepository.Remove(step.Id);
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
            existingStep.Duration = updateStepRequest.Duration;
            existingStep.Description = updateStepRequest.Description;
            existingStep.IsCompleted = updateStepRequest.IsCompleted;
            existingStep.Name = updateStepRequest.Name;

            await _stepsRepository.Update(existingStep);
            await _uow.Commit();

            StepDto stepDto = _mapper.Map<StepDto>(existingStep);

            return stepDto;


        }

    }
}
