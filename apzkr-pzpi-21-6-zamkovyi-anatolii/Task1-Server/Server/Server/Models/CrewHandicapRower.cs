using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class CrewHandicapRower
    {
        public int CrewHandicapRowerId { get; set; }

        [ForeignKey("Athlete")]
        public int AthleteId { get; set; }

        [ForeignKey("CrewHandicapId")]
        public int CrewHandicapId { get; set; }
    }
}
