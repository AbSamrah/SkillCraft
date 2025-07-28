using RoadmapMangement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.BuisnessLogicLayer.Services
{
    public interface IRoadmapCreationStrategy
    {
        Task<Roadmap> CreateRoadmap(object parameters);
    }
}
