using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class CrewRower
    {
        public int CrewRowerId { get; set; }

        [ForeignKey("Athlete")]
        public int AthleteId { get; set; }

        [ForeignKey("Crew")]
        public int CrewId { get; set; }
    }
}
