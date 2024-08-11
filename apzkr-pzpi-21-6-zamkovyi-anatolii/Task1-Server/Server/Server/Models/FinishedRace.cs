using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class FinishedRace
    {
        public int FinishedRaceId { get; set; }

        public int RaceId { get; set; }
        [ForeignKey("RaceId")]

        public DateTime FinishTime { get; set; }

    }
}
