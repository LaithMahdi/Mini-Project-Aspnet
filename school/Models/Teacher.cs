using System.Security.Claims;
using static System.Collections.Specialized.BitVector32;

namespace school.Models
{
    public class Teacher
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Specialization { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? HireDate { get; set; }
        public decimal? Salary { get; set; }
        public bool IsActive { get; set; } = true;

        // FK
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // Navigation
        public ICollection<Classe> ReferentClasses { get; set; } = new List<Classe>();
        public ICollection<ClassSubject> ClassSubjects { get; set; } = new List<ClassSubject>();
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
