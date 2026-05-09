using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakealotDriverManagementSystem.Models
{
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Vacancy")]
        public int VacancyId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        public string LicenseNumber { get; set; }

        [Required]
        public string ResumePath { get; set; }

        [Required]
        public string Status { get; set; }

        public DateTime DateSubmitted { get; set; }

        // Navigation Properties
        public Vacancy? Vacancy { get; set; }
        public User? User { get; set; }
    }
}
