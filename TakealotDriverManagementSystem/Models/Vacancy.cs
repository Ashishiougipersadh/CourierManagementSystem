using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakealotDriverManagementSystem.Models
{
    public class Vacancy
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [ForeignKey("Warehouse")]
        public int WarehouseId { get; set; }

        // Navigation Properties
        public Warehouse? Warehouse { get; set; } 
        public ICollection<JobApplication>? Applications { get; set; } 
    }
}
