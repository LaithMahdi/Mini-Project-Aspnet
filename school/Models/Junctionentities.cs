namespace school.Models;

public class ClassSubject
{
    public int Id { get; set; }
    public Guid ClassId { get; set; }
    public Classe Class { get; set; } = null!;
    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;
    public Guid? TeacherId { get; set; }
    public Teacher? Teacher { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}

public class SessionAuditLog
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public Session Session { get; set; } = null!;
    public string Action { get; set; } = string.Empty;  // CREATED, MODIFIED, CANCELLED
    public string? Details { get; set; }
    public Guid ActorId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
