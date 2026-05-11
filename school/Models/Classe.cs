using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using school.Models.Validation;

namespace school.Models
{
    public class Classe : ITrackTimestamps
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Class Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Class Name must be between 2 and 100 characters.")]
        [Display(Name = "Class Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Level is required.")]
        [StringLength(50, ErrorMessage = "Level cannot exceed 50 characters.")]
        [Display(Name = "Level")]
        public string Level { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Filiere cannot exceed 100 characters.")]
        [Display(Name = "Filiere")]
        public string? Filiere { get; set; }

        [Required(ErrorMessage = "Max Capacity is required.")]
        [Range(1, 500, ErrorMessage = "Max Capacity must be between 1 and 500.")]
        [Display(Name = "Max Capacity")]
        public int MaxCapacity { get; set; }

        [Display(Name = "Archived")]
        public bool IsArchived { get; set; } = false;

        [Required(ErrorMessage = "Academic Year is required.")]
        [StringLength(10, ErrorMessage = "Academic Year cannot exceed 10 characters.")]
        [RegularExpression(@"^\d{4}/\d{4}$", ErrorMessage = "Academic Year must be in the format YYYY/YYYY (e.g., 2026/2027).")]
        [Display(Name = "Academic Year")]
        public string AcademicYear { get; set; } = string.Empty;

        [Display(Name = "Created Date")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Updated Date")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // FK
        public int? SectionId { get; set; }
        public Section? Section { get; set; }

        [Display(Name = "Referent Teacher")]
        public Guid? ReferentTeacherId { get; set; }
        public Teacher? ReferentTeacher { get; set; }

        // Navigation
        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<ClassSubject> ClassSubjects { get; set; } = new List<ClassSubject>();
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
