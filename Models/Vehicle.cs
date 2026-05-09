using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakealotDriverManagementSystem.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public string LicensePlate { get; set; }

        public string VehicleImagePath { get; set; }

        [ForeignKey("Warehouse")]
        public int WarehouseId { get; set; }

        // Navigation Properties
        public Warehouse? Warehouse { get; set; } 
        public Driver? Driver { get; set; } 
    }
}
