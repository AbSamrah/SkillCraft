using RoadmapMangement.BuisnessLogicLayer.Filters;
using RoadmapMangement.BuisnessLogicLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoadmapMangement.BuisnessLogicLayer.Services
{
    /// <summary>
    /// Defines the contract for the service that manages roadmaps.
    /// </summary>
    public interface IRoadmapsService
    {
        /// <summary>
        /// Retrieves all roadmaps.
        /// </summary>
        /// <returns>A list of all roadmaps as DTOs.</returns>
        Task<List<RoadmapDto>> GetAll(EntityFilter filter);

        /// <summary>
        /// Adds a new roadmap using a specified creation strategy.
        /// </summary>
        /// <param name="strategy">The strategy to use for creation (e.g., manual, AI).</param>
        /// <param name="parameters">The parameters required by the chosen strategy.</param>
        /// <returns>The newly created roadmap as a DTO.</returns>
        Task<RoadmapDto> Add(IRoadmapCreationStrategy strategy, object parameters);

        /// <summary>
        /// Retrieves a specific roadmap by its ID.
        /// </summary>
        /// <param name="id">The ID of the roadmap to retrieve.</param>
        /// <returns>The roadmap DTO if found; otherwise, throws a KeyNotFoundException.</returns>
        Task<RoadmapDto> Get(string id);

        /// <summary>
        /// Deletes a roadmap by its ID.
        /// </summary>
        /// <param name="id">The ID of the roadmap to delete.</param>
        /// <returns>The deleted roadmap as a DTO.</returns>
        Task<RoadmapDto> DeleteAsync(string id);

        /// <summary>
        /// Updates an existing roadmap.
        /// </summary>
        /// <param name="id">The ID of the roadmap to update.</param>
        /// <param name="updateRoadmapRequest">The request object containing the updated data.</param>
        /// <returns>The updated roadmap as a DTO.</returns>
        Task<RoadmapDto> UpdateAsync(string id, UpdateRoadmapRequest updateRoadmapRequest);
    }
}
