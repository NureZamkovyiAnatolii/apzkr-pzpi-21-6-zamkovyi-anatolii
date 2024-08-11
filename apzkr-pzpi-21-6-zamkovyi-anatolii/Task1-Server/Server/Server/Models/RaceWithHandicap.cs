using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class RaceWithHandicap
    {
        public int RaceWithHandicapId { get; set; }
        public DateTime StartTime { get; set; }

        
        public int CompetitionId { get; set; }
        [ForeignKey("Competition")]
        public string Stage { get; set; } 

        public TimeSpan HandicapDuration { get; set; } = TimeSpan.FromMinutes(1); 
    }
}
