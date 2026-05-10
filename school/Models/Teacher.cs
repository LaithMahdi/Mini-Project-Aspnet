using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using static System.Collections.Specialized.BitVector32;
using school.Models.Validation;

namespace school.Models
{
    public class Teacher
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [StringLength(100, ErrorMessage = "Specialization cannot exceed 100 characters.")]
        [Display(Name = "Specialization")]
        public string? Specialization { get; set; }

        [Display(Name = "Gender")]
        public Gender? Gender { get; set; }

        [Display(Name = "Hire Date")]
        [DataType(DataType.Date)]
        [NoFutureDate(ErrorMessage = "Hire Date cannot be in the future.")]
        public DateTime? HireDate { get; set; }

        [Range(0, 100000, ErrorMessage = "Salary must be a positive number.")]
        [Display(Name = "Salary")]
        public decimal? Salary { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        // FK
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // Navigation
        public ICollection<Classe> ReferentClasses { get; set; } = new List<Classe>();
        public ICollection<ClassSubject> ClassSubjects { get; set; } = new List<ClassSubject>();
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
