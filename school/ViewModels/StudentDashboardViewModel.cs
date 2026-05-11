using System;
using System.Collections.Generic;
using school.Models;

namespace school.ViewModels
{
    public class StudentDashboardViewModel
    {
        public string StudentName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public Guid? ClassId { get; set; }
        
        public int Month { get; set; }
        public int Year { get; set; }
        
        public DateOnly MonthStartDate { get; set; }
        public DateOnly GridStartDate { get; set; }
        public DateOnly GridEndDate { get; set; }
        
        public List<StudentCalendarDay> Days { get; set; } = new List<StudentCalendarDay>();
        
        public int TotalSessionsThisMonth { get; set; }
        public int PlannedCount { get; set; }
        public int CancelledCount { get; set; }
        public int PostponedCount { get; set; }
    }

    public class StudentCalendarDay
    {
        public DateOnly Date { get; set; }
        public bool IsCurrentMonth { get; set; }
        public bool IsToday { get; set; }
        public List<StudentCalendarSession> Sessions { get; set; } = new List<StudentCalendarSession>();
    }

    public class StudentCalendarSession
    {
        public int Id { get; set; }
        public DateOnly SessionDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public SessionStatus Status { get; set; }
    }
}
