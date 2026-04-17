using System.Security.Claims;

namespace school.Models
{
    public class Section : ITrackTimestamps
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Classe> Classes { get; set; } = new List<Classe>();
    }
}
