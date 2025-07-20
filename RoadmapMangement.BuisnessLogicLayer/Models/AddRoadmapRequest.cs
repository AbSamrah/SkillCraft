namespace RoadmapMangement.BuisnessLogicLayer.Models
{
    public class AddRoadmapRequest: RoadmapRequest
    {
        public List<string> MilestonesIds { get; set; } = new List<string>();
        public bool IsActive { get; internal set; }
    }
}