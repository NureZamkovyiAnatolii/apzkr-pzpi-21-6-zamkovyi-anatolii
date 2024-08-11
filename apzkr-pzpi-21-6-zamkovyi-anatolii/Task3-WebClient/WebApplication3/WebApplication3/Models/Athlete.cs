using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class Athlete
    {
        public int AthleteId { get; set; }
        public string AthleteName { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public int PhoneNumber { get; set; }
        [ForeignKey("Coach")]
        public int CoachId { get; set; } // Зберігати ID тренера

    }
}
