using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class ResultHandicap
    {
        public int ResultHandicapId { get; set; }

        [ForeignKey("CrewHandicapId")]
        public int CrewHandicapId { get; set; }
        public TimeSpan TimeTaken { get; set; }
    }
}
