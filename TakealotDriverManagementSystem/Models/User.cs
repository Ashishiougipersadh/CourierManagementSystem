using System.ComponentModel.DataAnnotations;

namespace TakealotDriverManagementSystem.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string ContactNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        // Navigation Properties
        public Driver Driver { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<ActivityLog> ActivityLogs { get; set; }
        public ICollection<JobApplication> JobApplications { get; set; }
    }
}
