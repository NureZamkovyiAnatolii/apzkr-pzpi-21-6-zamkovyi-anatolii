using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class FinishedRaceWithHandicap
    {
        public int FinishedRaceWithHandicapId { get; set; }

        public int RaceWithHandicapId { get; set; } // Посилання на заїзди з гандикапом
        [ForeignKey("RaceWithHandicap")]
        public DateTime FinishTime { get; set; }

    }
}
