

namespace RoadmapMangement.BuisnessLogicLayer.Models
{
    public class RoadmapDto: RoadmapRequest
    {
        public string Id { get; set; }
        public List<MilestoneDto> Milestones { get; set; }
    }
}