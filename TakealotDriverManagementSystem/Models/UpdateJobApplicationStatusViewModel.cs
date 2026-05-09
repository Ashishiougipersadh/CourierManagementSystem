using System.ComponentModel.DataAnnotations;

namespace TakealotDriverManagementSystem.Models
{
    public class UpdateJobApplicationStatusViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Status {  get; set; }
    }
}
