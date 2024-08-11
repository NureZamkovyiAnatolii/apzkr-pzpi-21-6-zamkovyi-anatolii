using System.Composition;

namespace WebApplication3.Models
{
    public class Organization
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string Password { get; set; }
        public int PhoneNumber { get; set; }
        public string Country { get; set; }

    }
}
