using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace school.Models
{
    public class Section : ITrackTimestamps
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Section Name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Section Name must be between 1 and 100 characters.")]
        [Display(Name = "Section Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Section Code is required.")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Section Code must be between 1 and 20 characters.")]
        [Display(Name = "Section Code")]
        public string Code { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Updated Date")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Classe> Classes { get; set; } = new List<Classe>();
    }
}
