using school.Models;

namespace school.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalStudents { get; set; }
        public int TotalClasses { get; set; }
        public int TotalSessions { get; set; }
        public int ActiveSessions { get; set; }
        public int CancelledSessions { get; set; }
        public int PostponedSessions { get; set; }
        public int AvailableRooms { get; set; }
        public int ActiveSubjects { get; set; }
        public List<SessionDashboardItem> LatestSessions { get; set; } = new();
    }

    public class SessionDashboardItem
    {
        public int Id { get; set; }
        public DateOnly SessionDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public SessionStatus Status { get; set; }
        public bool IsActive { get; set; }
        public string TeacherName { get; set; } = "-";
        public string SubjectName { get; set; } = "-";
        public string RoomName { get; set; } = "-";
    }
}
