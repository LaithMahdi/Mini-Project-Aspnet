using Microsoft.AspNetCore.Identity;

namespace school.Models
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;
        public Role Role { get; set; }
        public string? Password { get; set; }
        public Gender? Gender { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Navigation
        public Student? Student { get; set; }
        public Teacher? Teacher { get; set; }
    }
}
