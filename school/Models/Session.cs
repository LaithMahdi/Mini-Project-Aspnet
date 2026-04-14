using System.Text.RegularExpressions;

namespace school.Models
{
    public class Session
    {
        public int Id { get; set; }
        public string DayOfWeek { get; set; } = string.Empty; 
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public SessionStatus Status { get; set; } = SessionStatus.PLANNED;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // FK
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;

        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;

        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;

        // Audit log
        public ICollection<SessionAuditLog> AuditLogs { get; set; } = new List<SessionAuditLog>();
    }
}
