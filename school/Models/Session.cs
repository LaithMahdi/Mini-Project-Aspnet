using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace school.Models
{
    public class Session
    {
        public int Id { get; set; }

        [Display(Name = "Session Date")]
        [DataType(DataType.Date)]
        public DateOnly SessionDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public SessionStatus Status { get; set; } = SessionStatus.PLANNED;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // FK
        public int RoomId { get; set; }

        [ValidateNever]
        public Room Room { get; set; } = null!;

        public int SubjectId { get; set; }

        [ValidateNever]
        public Subject Subject { get; set; } = null!;

        public Guid TeacherId { get; set; }

        [ValidateNever]
        public Teacher Teacher { get; set; } = null!;

        // Audit log
        [ValidateNever]
        public ICollection<SessionAuditLog> AuditLogs { get; set; } = new List<SessionAuditLog>();
    }
}
