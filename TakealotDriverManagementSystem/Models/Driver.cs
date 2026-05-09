using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakealotDriverManagementSystem.Models
{
    public class Driver
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        public string LicenseNumber { get; set; }

        [ForeignKey("Vehicle")]
        public int? AssignedVehicleId { get; set; }

        // Navigation Properties
        public User? User { get; set; } 
        public Vehicle? AssignedVehicle { get; set; }
    }
}
