using System.ComponentModel.DataAnnotations;

namespace TakealotDriverManagementSystem.Models
{
    public class Warehouse
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        // Navigation Properties
        public ICollection<Vehicle>? Vehicles { get; set; } 
        public ICollection<Vacancy>? Vacancies { get; set; } 
    }
}
