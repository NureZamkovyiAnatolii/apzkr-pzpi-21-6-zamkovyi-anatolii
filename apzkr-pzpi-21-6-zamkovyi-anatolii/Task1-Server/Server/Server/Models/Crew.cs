using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class Crew
    {
        public int CrewId { get; set; }
        public BoatType BoatType { get; set; }
        [ForeignKey("Competition")]
        public int CompetitionId { get; set; }
       
        [ForeignKey("Race")]
        public int RaceId { get; set; }
        public int StartNumber {  get; set; }
    }
}
