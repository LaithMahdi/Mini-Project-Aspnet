using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using school.Models;
using school.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace school.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? month = null, int? year = null)
        {
            if (User.IsInRole(Role.Teacher.ToString()))
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!Guid.TryParse(userIdClaim, out var userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                var teacher = await _context.Teachers
                    .Include(t => t.User)
                    .FirstOrDefaultAsync(t => t.UserId == userId && t.IsActive && t.User.IsActive);

                if (teacher == null)
                {
                    return View("TeacherCalendar", new TeacherCalendarViewModel());
                }

                var now = DateOnly.FromDateTime(DateTime.Today);
                var selectedYear = year ?? now.Year;
                var selectedMonth = month ?? now.Month;

                if (selectedMonth is < 1 or > 12)
                {
                    selectedMonth = now.Month;
                }

                if (selectedYear < 2000 || selectedYear > 2100)
                {
                    selectedYear = now.Year;
                }

                var monthStart = new DateOnly(selectedYear, selectedMonth, 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);

                var startOffset = ((int)monthStart.DayOfWeek + 6) % 7;
                var gridStart = monthStart.AddDays(-startOffset);

                var endOffset = 6 - (((int)monthEnd.DayOfWeek + 6) % 7);
                var gridEnd = monthEnd.AddDays(endOffset);

                var sessions = await _context.Sessions
                    .Include(s => s.Subject)
                    .Include(s => s.Room)
                    .Where(s => s.TeacherId == teacher.Id && s.SessionDate >= gridStart && s.SessionDate <= gridEnd)
                    .OrderBy(s => s.SessionDate)
                    .ThenBy(s => s.StartTime)
                    .Select(s => new TeacherCalendarSession
                    {
                        Id = s.Id,
                        SessionDate = s.SessionDate,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        SubjectName = s.Subject.Name + " (" + s.Subject.Code + ")",
                        RoomName = s.Room.Name,
                        Status = s.Status
                    })
                    .ToListAsync();

                var grouped = sessions
                    .GroupBy(s => s.SessionDate)
                    .ToDictionary(g => g.Key, g => g.ToList());

                var teacherCalendarModel = new TeacherCalendarViewModel
                {
                    TeacherName = teacher.User.FullName,
                    Month = selectedMonth,
                    Year = selectedYear,
                    MonthStartDate = monthStart,
                    GridStartDate = gridStart,
                    GridEndDate = gridEnd,
                    TotalSessionsThisMonth = 0,
                    PlannedCount = sessions.Count(s => s.Status == SessionStatus.PLANNED),
                    CancelledCount = sessions.Count(s => s.Status == SessionStatus.CANCELLED),
                    PostponedCount = sessions.Count(s => s.Status == SessionStatus.POSTPONED)
                };

                var cursor = gridStart;
                while (cursor <= gridEnd)
                {
                    teacherCalendarModel.Days.Add(new TeacherCalendarDay
                    {
                        Date = cursor,
                        IsCurrentMonth = cursor.Month == selectedMonth,
                        IsToday = cursor == now,
                        Sessions = grouped.TryGetValue(cursor, out var daySessions)
                            ? daySessions
                            : new List<TeacherCalendarSession>()
                    });

                    cursor = cursor.AddDays(1);
                }

                teacherCalendarModel.TotalSessionsThisMonth = teacherCalendarModel.Days
                    .Where(d => d.IsCurrentMonth)
                    .Sum(d => d.Sessions.Count);

                return View("TeacherCalendar", teacherCalendarModel);
            }

            var model = new AdminDashboardViewModel
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalTeachers = await _context.Teachers.CountAsync(),
                TotalStudents = await _context.Students.CountAsync(),
                TotalClasses = await _context.Classes.CountAsync(),
                TotalSessions = await _context.Sessions.CountAsync(),
                ActiveSessions = await _context.Sessions.CountAsync(s => s.Status == SessionStatus.PLANNED && s.IsActive),
                CancelledSessions = await _context.Sessions.CountAsync(s => s.Status == SessionStatus.CANCELLED),
                PostponedSessions = await _context.Sessions.CountAsync(s => s.Status == SessionStatus.POSTPONED),
                AvailableRooms = await _context.Rooms.CountAsync(r => r.IsAvailable),
                ActiveSubjects = await _context.Subjects.CountAsync(s => s.IsActive)
            };

            model.LatestSessions = await _context.Sessions
                .Include(s => s.Teacher)
                    .ThenInclude(t => t.User)
                .Include(s => s.Subject)
                .Include(s => s.Room)
                .OrderByDescending(s => s.SessionDate)
                .ThenByDescending(s => s.StartTime)
                .Take(6)
                .Select(s => new SessionDashboardItem
                {
                    Id = s.Id,
                    SessionDate = s.SessionDate,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    Status = s.Status,
                    IsActive = s.IsActive,
                    TeacherName = s.Teacher.User.FullName,
                    SubjectName = s.Subject.Name + " (" + s.Subject.Code + ")",
                    RoomName = s.Room.Name
                })
                .ToListAsync();

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
