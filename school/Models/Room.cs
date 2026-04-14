using static System.Collections.Specialized.BitVector32;

namespace school.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public RoomType Type { get; set; }
        public int Capacity { get; set; }
        public string? Building { get; set; }
        public int? Floor { get; set; }
        public bool HasProjector { get; set; } = false;
        public bool HasComputers { get; set; } = false;
        public bool IsAvailable { get; set; } = true;
        public string? Equipment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
