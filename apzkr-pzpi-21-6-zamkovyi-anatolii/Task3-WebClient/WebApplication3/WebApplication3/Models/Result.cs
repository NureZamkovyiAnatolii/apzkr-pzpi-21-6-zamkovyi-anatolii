using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class Result
    {
        public int ResultId { get; set; }

        public int CrewId { get; set; }
        [ForeignKey("CrewId")]

        public TimeSpan TimeTaken { get; set; }
    }
}
