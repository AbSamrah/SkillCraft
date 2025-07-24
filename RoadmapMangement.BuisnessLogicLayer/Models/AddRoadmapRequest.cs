namespace RoadmapMangement.BuisnessLogicLayer.Models
{
    public class AddRoadmapRequest: RoadmapRequest
    {
        public List<string> MilestonesIds { get; set; }
        public bool IsActive { get; internal set; }
    }
}