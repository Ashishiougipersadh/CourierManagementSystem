using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakealotDriverManagementSystem.Models
{
    public class ActivityLog
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        public string ActionType { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;

        // Navigation Property
        public User User { get; set; }
    }
}
