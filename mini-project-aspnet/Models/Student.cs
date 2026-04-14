using System.ComponentModel.DataAnnotations;

namespace mini_project_aspnet.Models
{
    public enum Gender
    {
        Man,
        Woman
    }

    public class Student : User
    {
        [MaxLength(500)]
        public string? avatarUrl { get; set; }

        public DateTime dateOfBirth { get; set; }

        [Required]
        public Gender gender { get; set; }

        [Required]
        [MaxLength(20)]
        public string cinNumber { get; set; }

        [Required]
        [MaxLength(20)]
        public string phoneNumber { get; set; }

        [MaxLength(20)]
        public string? secondPhoneNumber { get; set; }

        [MaxLength(500)]
        public string? address { get; set; }

        public bool isActive { get; set; } = true;

        [Required]
        public DateTime enrollmentDate { get; set; }
    }
}
