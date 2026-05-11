using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using school.Models.Validation;

namespace school.Models
{
    public class Session
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Session Date is required.")]
        [Display(Name = "Session Date")]
        [DataType(DataType.Date)]
        [NoPastDate(ErrorMessage = "Session Date cannot be in the past.")]
        public DateOnly SessionDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [Required(ErrorMessage = "Start Time is required.")]
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        public TimeOnly StartTime { get; set; }

        [Required(ErrorMessage = "End Time is required.")]
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        public TimeOnly EndTime { get; set; }

        [Display(Name = "Status")]
        public SessionStatus Status { get; set; } = SessionStatus.PLANNED;

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Updated Date")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // FK
        [Required(ErrorMessage = "Class is required.")]
        [Display(Name = "Class")]
        public Guid ClassId { get; set; }

        [ValidateNever]
        public Classe Class { get; set; } = null!;

        [Required(ErrorMessage = "Room is required.")]
        [Display(Name = "Room")]
        public int RoomId { get; set; }

        [ValidateNever]
        public Room Room { get; set; } = null!;

        [Required(ErrorMessage = "Subject is required.")]
        [Display(Name = "Subject")]
        public int SubjectId { get; set; }

        [ValidateNever]
        public Subject Subject { get; set; } = null!;

        [Required(ErrorMessage = "Teacher is required.")]
        [Display(Name = "Teacher")]
        public Guid TeacherId { get; set; }

        [ValidateNever]
        public Teacher Teacher { get; set; } = null!;

        // Audit log
        [ValidateNever]
        public ICollection<SessionAuditLog> AuditLogs { get; set; } = new List<SessionAuditLog>();
    }
}
