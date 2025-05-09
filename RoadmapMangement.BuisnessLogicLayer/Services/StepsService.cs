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
    }
}
