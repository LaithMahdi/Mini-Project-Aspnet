using System.ComponentModel.DataAnnotations;

namespace school.Models
{
    public class Student
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        public Gender? Gender { get; set; }

        [Display(Name = "CIN Number")]
        [StringLength(20, ErrorMessage = "CIN Number cannot exceed 20 characters.")]
        public string? CinNumber { get; set; }

        [Display(Name = "Phone Number")]
        [Phone]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Secondary Phone")]
        [Phone]
        public string? SecondPhoneNumber { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Enrollment Date")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

        // Foreign Key
        [Required]
        public Guid UserId { get; set; }

        // Navigation Property
        public User User { get; set; } = null!;
    }
}
