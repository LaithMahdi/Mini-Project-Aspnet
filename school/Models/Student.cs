namespace school.Models
{
    public class Student
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime? DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public string? CinNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? SecondPhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

        // FK
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
