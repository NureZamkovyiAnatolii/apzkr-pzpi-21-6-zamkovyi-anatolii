using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class Competition
    {
        public int? CompetitionId { get; set; } = null;
        [ForeignKey("Organization")]
        public int? OrganizationId { get; set; } = null;
        [ForeignKey("Coach")]
        public int? CoachId { get; set; }
        public string CompetitionName { get; set; }
        public string CompetitionCountry { get; set; }
        public string CompetitionCity { get; set; }
        public DateTime RegistrationDeadline { get; set; }
        public DateTime CompetitioneDate { get; set; }
    }
}
