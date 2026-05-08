using System.ComponentModel.DataAnnotations;
using static System.Collections.Specialized.BitVector32;

namespace school.Models
{
    public class Subject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Subject Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Subject Name must be between 2 and 100 characters.")]
        [Display(Name = "Subject Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subject Code is required.")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Subject Code must be between 1 and 10 characters.")]
        [Display(Name = "Subject Code")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Coefficient is required.")]
        [Range(0.01, 5, ErrorMessage = "Coefficient must be between 0.01 and 5.")]
        [Display(Name = "Coefficient")]
        public decimal Coefficient { get; set; }

        [Required(ErrorMessage = "Hours Per Week is required.")]
        [Range(1, 40, ErrorMessage = "Hours Per Week must be between 1 and 40.")]
        [Display(Name = "Hours Per Week")]
        public int HoursPerWeek { get; set; }

        [Required(ErrorMessage = "Subject Type is required.")]
        [Display(Name = "Subject Type")]
        public SubjectType Type { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Updated Date")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<ClassSubject> ClassSubjects { get; set; } = new List<ClassSubject>();
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
