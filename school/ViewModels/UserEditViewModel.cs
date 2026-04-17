using school.Models;
using System.ComponentModel.DataAnnotations;

namespace school.ViewModels
{
    public class UserEditViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public Role Role { get; set; }
        public Gender? Gender { get; set; }
        public bool IsActive { get; set; } = true;
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        // Teacher fields
        public string? Specialization { get; set; }
        public DateTime? HireDate { get; set; }
        public decimal? Salary { get; set; }

        // Student fields
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        public string? CinNumber { get; set; }
        public string? SecondPhoneNumber { get; set; }
        public string? Address { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EnrollmentDate { get; set; }
    }
}
