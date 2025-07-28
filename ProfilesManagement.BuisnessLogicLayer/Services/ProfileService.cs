using AutoMapper;
using ProfilesManagement.BuisnessLogicLayer.Models;
using ProfilesManagement.DataAccessLayer.Interfaces;
using ProfilesManagement.DataAccessLayer.Models;
using ProfilesManagement.DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfilesManagement.BuisnessLogicLayer.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IMapper _mapper;

        public ProfileService(IProfileRepository profileRepository, IMapper mapper)
        {
            _profileRepository = profileRepository;
            _mapper = mapper;
        }
        public async Task Add(string userId)
        {
            UserProfile profile = new UserProfile(userId);
            await _profileRepository.Add(profile);
        }

        public async Task<List<RoadmapStatusDto>> GetAllRoadmaps(string userId)
        {
            var profile = await _profileRepository.GetById(userId);
            if (profile is null)
            {
                throw new KeyNotFoundException("Profile not found.");
            }
            return _mapper.Map<List<RoadmapStatusDto>>(profile.Roadmaps);
        }

        public async Task<List<RoadmapStatusDto>> GetRoadmaps(string userId, bool isFinished=true)
        {
            var profile = await _profileRepository.GetById(userId);
            if (profile is null)
            {
                throw new KeyNotFoundException("Profile not found.");
            }
            var roadmaps = profile.Roadmaps.Where(r => r.IsFinished = isFinished);
            return _mapper.Map<List<RoadmapStatusDto>>(roadmaps);
        }

        public async Task<List<string>> GetFinishedStep(string userId, List<string> stepsIds)
        {
            var profile = await _profileRepository.GetById(userId);
            if (profile is null)
            {
                throw new KeyNotFoundException("Profile not found.");
            }
            List<string> steps = new List<string>();
            steps = profile.FinishedSteps.FindAll(s => stepsIds.Contains(s));
            return steps;
        }

        public async Task FinishSteps(string userId, List<string> stepsIds)
        {
            var profile = await _profileRepository.GetById(userId);
            if (profile is null)
            {
                throw new KeyNotFoundException("Profile not found.");
            }
            List<string> newSteps = stepsIds.Where(s => !profile.FinishedSteps.Contains(s)).ToList();
            profile.FinishedSteps.AddRange(newSteps);
            await _profileRepository.Update(profile);
        }

        public async Task UnFinishSteps(string userId, List<string> stepsIds)
        {
            var profile = await _profileRepository.GetById(userId);
            if (profile is null)
            {
                throw new KeyNotFoundException("Profile not found.");
            }
            profile.FinishedSteps.RemoveAll(s => stepsIds.Contains(s));
            await _profileRepository.Update(profile);
        }

        public async Task ChangeRoadmapStatus(string userId, string roadmapId, bool finished = true)
        {
            var profile = await _profileRepository.GetById(userId);
            if (profile is null)
            {
                throw new KeyNotFoundException("Profile not found.");
            }
            var roadmap = profile.Roadmaps.FirstOrDefault(r => r.Id == roadmapId);
            if(roadmap is null)
            {
                throw new KeyNotFoundException("Roadmap not found.");
            }
            roadmap.IsFinished = finished;
            await _profileRepository.Update(profile);
        }

        public async Task AddRoadmap(string userId, string roadmapId)
        {
            var profile = await _profileRepository.GetById(userId);
            if (profile is null)
            {
                throw new KeyNotFoundException("Profile not found.");
            }
            if (profile.Roadmaps.Exists(r => r.Id == roadmapId))
            {
                throw new KeyNotFoundException("Roadmap is already exists.");
            }
            var roadmap = new RoadmapStatus();
            roadmap.Id = roadmapId;
            roadmap.IsFinished = false;
            profile.Roadmaps.Add(roadmap);
            await _profileRepository.Update(profile);
        }

        public async Task RemoveRoadmap(string userId, string roadmapId)
        {
            var profile = await _profileRepository.GetById(userId);
            if (profile is null)
            {
                throw new KeyNotFoundException("Profile not found.");
            }
            if (!profile.Roadmaps.Exists(r => r.Id == roadmapId))
            {
                throw new KeyNotFoundException("Roadmap is not found.");
            }
            var roadmap = profile.Roadmaps.Find(r => r.Id == roadmapId);
            profile.Roadmaps.Remove(roadmap);
            await _profileRepository.Update(profile);
        }

        public async Task Remove(string userId)
        {
            await _profileRepository.Remove(userId);
        }

    }
}
