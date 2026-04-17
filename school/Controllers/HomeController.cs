using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using school.Models;
using school.ViewModels;
using System.Diagnostics;

namespace school.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
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
