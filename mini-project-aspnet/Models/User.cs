using System.ComponentModel.DataAnnotations;

namespace mini_project_aspnet.Models
{
    public enum UserRole
    {
        Admin,
        Teacher,
        Student
    }

    public class User : BaseEntity
    {
        [Key]
        public Guid id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(150)]
        public string fullName { get; set; }

        [Required]
        public UserRole role { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        [MaxLength(255)]
        public string createdBy { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string email { get; set; }
    }
}
