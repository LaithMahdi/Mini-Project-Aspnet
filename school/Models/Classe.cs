using System.Text.RegularExpressions;

namespace school.Models
{
    public class Classe
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string? Filiere { get; set; }
        public int MaxCapacity { get; set; }
        public bool IsArchived { get; set; } = false;
        public string AcademicYear { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // FK
        public int? SectionId { get; set; }
        public Section? Section { get; set; }

        public Guid? ReferentTeacherId { get; set; }
        public Teacher? ReferentTeacher { get; set; }

        // Navigation
        public ICollection<ClassSubject> ClassSubjects { get; set; } = new List<ClassSubject>();
    }
}
