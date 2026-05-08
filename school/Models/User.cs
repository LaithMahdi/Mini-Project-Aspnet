using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using school.Models.Validation;

namespace school.Models
{
    public class User : IdentityUser<Guid>
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Full Name must be between 3 and 100 characters.")]
        [AlphabeticalName(ErrorMessage = "Full Name can only contain letters, spaces, hyphens, and apostrophes. Numbers are not allowed.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required.")]
        [Display(Name = "Role")]
        public Role Role { get; set; }

        public string? Password { get; set; }

        [Display(Name = "Gender")]
        public Gender? Gender { get; set; }

        public Guid? CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Updated Date")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        // Navigation
        public Student? Student { get; set; }
        public Teacher? Teacher { get; set; }
    }
}
