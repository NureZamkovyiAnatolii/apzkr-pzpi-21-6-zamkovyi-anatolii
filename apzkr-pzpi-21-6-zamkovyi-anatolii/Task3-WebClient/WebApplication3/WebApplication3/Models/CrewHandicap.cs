using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class CrewHandicap
    {
        public int CrewHandicapId { get; set; }
        public BoatType BoatType { get; set; }
        [ForeignKey("Competition")]
        public int CompetitionId { get; set; }
       
        [ForeignKey("RaceWithHandicap")]
        public int RaceWithHandicapId { get; set; }
        public int StartNumber {  get; set; }
    }
}
