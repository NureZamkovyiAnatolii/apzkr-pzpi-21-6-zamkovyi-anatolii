using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class Race
    {
        public int RaceId { get; set; }
        public DateTime StartTime { get; set; }
        [ForeignKey("Competition")]
        public int CompetitionId { get; set; }
        
        public string Stage { get; set; }

    }
}
