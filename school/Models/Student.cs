using System.ComponentModel.DataAnnotations;
using school.Models.Validation;

namespace school.Models
{
    public class Student
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Date of Birth is required.")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [NoFutureDate(ErrorMessage = "Date of Birth cannot be in the future.")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Gender")]
        public Gender? Gender { get; set; }

        [Display(Name = "CIN Number")]
        [StringLength(20, ErrorMessage = "CIN Number cannot exceed 20 characters.")]
        public string? CinNumber { get; set; }

        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Secondary Phone")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string? SecondPhoneNumber { get; set; }

        [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters.")]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Enrollment Date")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

        // Foreign Key
        [Required]
        public Guid UserId { get; set; }

        public Guid? ClassId { get; set; }

        // Navigation Property
        public User User { get; set; } = null!;
        public Classe? Class { get; set; }
    }
}
