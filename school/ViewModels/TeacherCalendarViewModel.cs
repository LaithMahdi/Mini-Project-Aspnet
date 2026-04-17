using school.Models;

namespace school.ViewModels
{
    public class TeacherCalendarViewModel
    {
        public string TeacherName { get; set; } = "-";
        public int Month { get; set; }
        public int Year { get; set; }
        public DateOnly MonthStartDate { get; set; }
        public DateOnly GridStartDate { get; set; }
        public DateOnly GridEndDate { get; set; }
        public List<TeacherCalendarDay> Days { get; set; } = new();
        public int TotalSessionsThisMonth { get; set; }
        public int PlannedCount { get; set; }
        public int CancelledCount { get; set; }
        public int PostponedCount { get; set; }
        public List<TeacherClassItem> MyClasses { get; set; } = new();
        public List<TeacherStudentItem> MyStudents { get; set; } = new();
        public int TotalStudentsInMyClasses { get; set; }
    }

    public class TeacherClassItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "-";
        public string Level { get; set; } = "-";
        public string AcademicYear { get; set; } = "-";
    }

    public class TeacherStudentItem
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = "-";
        public string UserName { get; set; } = "-";
        public string ClassName { get; set; } = "-";
    }

    public class TeacherCalendarDay
    {
        public DateOnly Date { get; set; }
        public bool IsCurrentMonth { get; set; }
        public bool IsToday { get; set; }
        public List<TeacherCalendarSession> Sessions { get; set; } = new();
    }

    public class TeacherCalendarSession
    {
        public int Id { get; set; }
        public DateOnly SessionDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string SubjectName { get; set; } = "-";
        public string RoomName { get; set; } = "-";
        public SessionStatus Status { get; set; }
    }
}
