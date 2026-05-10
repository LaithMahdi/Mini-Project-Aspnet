using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using school.Models;
using school.Services;

namespace school.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISessionScheduleService _scheduleService;

        public SessionsController(ApplicationDbContext context, ISessionScheduleService scheduleService)
        {
            _context = context;
            _scheduleService = scheduleService;
        }

        // GET: Sessions
        public async Task<IActionResult> Index(
            string? search,
            DateOnly? sessionDate,
            SessionStatus? status,
            Guid? teacherId,
            int? subjectId,
            string? timeSlot,
            int page = 1)
        {
            const int pageSize = 15;
            var isTeacher = User.IsInRole(Role.Teacher.ToString());
            var currentTeacherId = await GetCurrentTeacherIdAsync();

            var sessionsQuery = _context.Sessions
                .Include(s => s.Room)
                .Include(s => s.Subject)
                .Include(s => s.Teacher)
                    .ThenInclude(t => t.User)
                .AsQueryable();

            if (isTeacher)
            {
                if (!currentTeacherId.HasValue)
                {
                    return Forbid();
                }

                sessionsQuery = sessionsQuery.Where(s => s.TeacherId == currentTeacherId.Value);
                teacherId = currentTeacherId.Value;
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                sessionsQuery = sessionsQuery.Where(s =>
                    (s.Room != null && s.Room.Name.ToLower().Contains(term)) ||
                    (s.Subject != null && (s.Subject.Name.ToLower().Contains(term) || s.Subject.Code.ToLower().Contains(term))) ||
                    (s.Teacher != null && s.Teacher.User != null && s.Teacher.User.FullName.ToLower().Contains(term)));
            }

            if (sessionDate.HasValue)
            {
                sessionsQuery = sessionsQuery.Where(s => s.SessionDate == sessionDate.Value);
            }

            if (status.HasValue)
            {
                sessionsQuery = sessionsQuery.Where(s => s.Status == status.Value);
            }

            if (teacherId.HasValue)
            {
                sessionsQuery = sessionsQuery.Where(s => s.TeacherId == teacherId.Value);
            }

            if (subjectId.HasValue)
            {
                sessionsQuery = sessionsQuery.Where(s => s.SubjectId == subjectId.Value);
            }

            if (!string.IsNullOrWhiteSpace(timeSlot))
            {
                var times = timeSlot.Split('-');
                if (times.Length == 2 && TimeOnly.TryParse(times[0], out var parsedStartTime) && TimeOnly.TryParse(times[1], out var parsedEndTime))
                {
                    sessionsQuery = sessionsQuery.Where(s => s.StartTime == parsedStartTime && s.EndTime == parsedEndTime);
                }
            }

            var filteredTotal = await sessionsQuery.CountAsync();
            var totalPages = filteredTotal == 0
                ? 1
                : (int)Math.Ceiling(filteredTotal / (double)pageSize);

            if (page < 1)
            {
                page = 1;
            }
            else if (page > totalPages)
            {
                page = totalPages;
            }

            var sessions = await sessionsQuery
                .OrderByDescending(s => s.SessionDate)
                .ThenByDescending(s => s.StartTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.SessionDate = sessionDate;
            ViewBag.Status = status;
            ViewBag.TeacherId = teacherId;
            ViewBag.SubjectId = subjectId;
            ViewBag.TimeSlot = timeSlot;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.FilteredTotal = filteredTotal;
            ViewBag.IsTeacher = isTeacher;

            PopulateFilterOptions(status, teacherId, subjectId, timeSlot);

            return View(sessions);
        }

        // GET: Sessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions
                .Include(s => s.Room)
                .Include(s => s.Subject)
                .Include(s => s.Teacher)
                    .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (session == null)
            {
                return NotFound();
            }

            if (User.IsInRole(Role.Teacher.ToString()))
            {
                var currentTeacherId = await GetCurrentTeacherIdAsync();
                if (!currentTeacherId.HasValue || session.TeacherId != currentTeacherId.Value)
                {
                    return Forbid();
                }
            }

            ViewBag.IsTeacher = User.IsInRole(Role.Teacher.ToString());

            return View(session);
        }

        // GET: Sessions/Create
        public IActionResult Create()
        {
            if (User.IsInRole(Role.Teacher.ToString()))
            {
                return Forbid();
            }

            PopulateEditOptions();
            return View(new Session
            {
                SessionDate = DateOnly.FromDateTime(DateTime.Today),
                StartTime = new TimeOnly(8, 0),
                EndTime = new TimeOnly(10, 0),
                Status = SessionStatus.PLANNED,
                IsActive = true
            });
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SessionDate,StartTime,EndTime,Status,IsActive,RoomId,SubjectId,TeacherId")] Session session)
        {
            Console.WriteLine("\n--- INCOMING SESSION DATA ---");
            Console.WriteLine($"SessionDate: {session.SessionDate:yyyy-MM-dd}");
            Console.WriteLine($"StartTime: {session.StartTime:hh\\:mm\\:ss}");
            Console.WriteLine($"EndTime: {session.EndTime:hh\\:mm\\:ss}");
            Console.WriteLine($"Status: {session.Status}");
            Console.WriteLine($"IsActive: {session.IsActive}");
            Console.WriteLine($"RoomId: {session.RoomId}");
            Console.WriteLine($"SubjectId: {session.SubjectId}");
            Console.WriteLine($"TeacherId: {session.TeacherId}");
            if (User.IsInRole(Role.Teacher.ToString()))
            {
                return Forbid();
            }

            // Validate end time is after start time
            if (session.EndTime <= session.StartTime)
            {
                ModelState.AddModelError("EndTime", "End Time must be after Start Time.");
            }

            // Validate room availability - check for conflicts
            if (session.RoomId > 0)
            {
                var roomConflict = await _context.Sessions
                    .Where(s => s.RoomId == session.RoomId
                        && s.SessionDate == session.SessionDate
                        && s.Status != SessionStatus.CANCELLED
                        && (s.StartTime < session.EndTime && s.EndTime > session.StartTime))
                    .FirstOrDefaultAsync();

                if (roomConflict != null)
                {
                    ModelState.AddModelError("RoomId", $"Room is not available. It's already scheduled from {roomConflict.StartTime:HH\\:mm} to {roomConflict.EndTime:HH\\:mm} on this date.");
                }
            }

            // Validate teacher availability - check for conflicts
            if (session.TeacherId != Guid.Empty)
            {
                var teacherConflict = await _context.Sessions
                    .Where(s => s.TeacherId == session.TeacherId
                        && s.SessionDate == session.SessionDate
                        && s.Status != SessionStatus.CANCELLED
                        && ((s.StartTime < session.EndTime && s.EndTime > session.StartTime)))
                    .FirstOrDefaultAsync();

                if (teacherConflict != null)
                {
                    ModelState.AddModelError("TeacherId", $"Teacher is not available. They have another session from {teacherConflict.StartTime:HH\\:mm} to {teacherConflict.EndTime:HH\\:mm} on this date.");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(session);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateEditOptions(session.Status, session.RoomId, session.SubjectId, session.TeacherId);
            return View(session);
        }

        // GET: Sessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (User.IsInRole(Role.Teacher.ToString()))
            {
                return Forbid();
            }

            if (id == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }
            PopulateEditOptions(session.Status, session.RoomId, session.SubjectId, session.TeacherId);
            return View(session);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SessionDate,StartTime,EndTime,Status,IsActive,RoomId,SubjectId,TeacherId")] Session session)
        {
            if (User.IsInRole(Role.Teacher.ToString()))
            {
                return Forbid();
            }

            if (id != session.Id)
            {
                return NotFound();
            }

            // Validate end time is after start time
            if (session.EndTime <= session.StartTime)
            {
                ModelState.AddModelError("EndTime", "End Time must be after Start Time.");
            }

            // Validate room availability - check for conflicts (excluding current session)
            if (session.RoomId > 0)
            {
                var roomConflict = await _context.Sessions
                    .Where(s => s.Id != session.Id  // Exclude current session
                        && s.RoomId == session.RoomId
                        && s.SessionDate == session.SessionDate
                        && s.Status != SessionStatus.CANCELLED
                        && (s.StartTime < session.EndTime && s.EndTime > session.StartTime))
                    .FirstOrDefaultAsync();

                if (roomConflict != null)
                {
                    ModelState.AddModelError("RoomId", $"Room is not available. It's already scheduled from {roomConflict.StartTime:HH\\:mm} to {roomConflict.EndTime:HH\\:mm} on this date.");
                }
            }

            // Validate teacher availability - check for conflicts (excluding current session)
            if (session.TeacherId != Guid.Empty)
            {
                var teacherConflict = await _context.Sessions
                    .Where(s => s.Id != session.Id  // Exclude current session
                        && s.TeacherId == session.TeacherId
                        && s.SessionDate == session.SessionDate
                        && s.Status != SessionStatus.CANCELLED
                        && (s.StartTime < session.EndTime && s.EndTime > session.StartTime))
                    .FirstOrDefaultAsync();

                if (teacherConflict != null)
                {
                    ModelState.AddModelError("TeacherId", $"Teacher is not available. They have another session from {teacherConflict.StartTime:HH\\:mm} to {teacherConflict.EndTime:HH\\:mm} on this date.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingSession = await _context.Sessions.FindAsync(id);
                    if (existingSession == null)
                    {
                        return NotFound();
                    }

                    existingSession.SessionDate = session.SessionDate;
                    existingSession.StartTime = session.StartTime;
                    existingSession.EndTime = session.EndTime;
                    existingSession.Status = session.Status;
                    existingSession.IsActive = session.IsActive;
                    existingSession.RoomId = session.RoomId;
                    existingSession.SubjectId = session.SubjectId;
                    existingSession.TeacherId = session.TeacherId;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SessionExists(session.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateEditOptions(session.Status, session.RoomId, session.SubjectId, session.TeacherId);
            return View(session);
        }

        // GET: Sessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (User.IsInRole(Role.Teacher.ToString()))
            {
                return Forbid();
            }

            if (id == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions
                .Include(s => s.Room)
                .Include(s => s.Subject)
                .Include(s => s.Teacher)
                    .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (User.IsInRole(Role.Teacher.ToString()))
            {
                return Forbid();
            }

            var session = await _context.Sessions.FindAsync(id);
            if (session != null)
            {
                _context.Sessions.Remove(session);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id, string? returnUrl = null)
        {
            var session = await _context.Sessions
                .FirstOrDefaultAsync(s => s.Id == id);

            if (session == null)
            {
                return NotFound();
            }

            if (User.IsInRole(Role.Teacher.ToString()))
            {
                var currentTeacherId = await GetCurrentTeacherIdAsync();
                if (!currentTeacherId.HasValue || session.TeacherId != currentTeacherId.Value)
                {
                    return Forbid();
                }
            }

            session.Status = SessionStatus.CANCELLED;
            session.IsActive = false;
            await _context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SessionExists(int id)
        {
            return _context.Sessions.Any(e => e.Id == id);
        }

        private void PopulateFilterOptions(SessionStatus? selectedStatus, Guid? selectedTeacherId, int? selectedSubjectId, string? selectedTimeSlot = null)
        {
            var statusOptions = Enum.GetValues<SessionStatus>()
                .Select(s => new
                {
                    Value = ((int)s).ToString(),
                    Text = s.ToString()
                })
                .ToList();

            var selectedStatusValue = selectedStatus.HasValue ? ((int)selectedStatus.Value).ToString() : null;
            ViewBag.StatusOptions = new SelectList(statusOptions, "Value", "Text", selectedStatusValue);

            var teacherOptions = _context.Teachers
                .Include(t => t.User)
                .OrderBy(t => t.User.FullName)
                .Select(t => new
                {
                    t.Id,
                    Display = t.User.FullName + (string.IsNullOrWhiteSpace(t.Specialization) ? string.Empty : " - " + t.Specialization)
                })
                .ToList();

            var subjectOptions = _context.Subjects
                .OrderBy(s => s.Name)
                .Select(s => new
                {
                    s.Id,
                    Display = s.Name + " (" + s.Code + ")"
                })
                .ToList();

            ViewBag.TeacherOptions = new SelectList(teacherOptions, "Id", "Display", selectedTeacherId);
            ViewBag.SubjectOptions = new SelectList(subjectOptions, "Id", "Display", selectedSubjectId);

            // Get available session slots for filter
            var sessionSlots = _scheduleService.GetAvailableSlots();
            var slotOptions = sessionSlots.Select(slot => new
            {
                Value = $"{slot.StartTime:HH\\:mm}-{slot.EndTime:HH\\:mm}",
                Text = slot.DisplayName
            }).ToList();

            ViewBag.SessionSlots = new SelectList(slotOptions, "Value", "Text", selectedTimeSlot);
        }

        private void PopulateEditOptions(SessionStatus? selectedStatus = null, int? roomId = null, int? subjectId = null, Guid? teacherId = null)
        {
            var statusOptions = Enum.GetValues<SessionStatus>()
                .Select(s => new
                {
                    Value = ((int)s).ToString(),
                    Text = s.ToString()
                })
                .ToList();

            var selectedStatusValue = selectedStatus.HasValue ? ((int)selectedStatus.Value).ToString() : null;
            ViewBag.StatusOptions = new SelectList(statusOptions, "Value", "Text", selectedStatusValue);

            var roomOptions = _context.Rooms
                .OrderBy(r => r.Name)
                .Select(r => new
                {
                    r.Id,
                    Display = r.Name + " - " + r.Type
                })
                .ToList();

            var subjectOptions = _context.Subjects
                .OrderBy(s => s.Name)
                .Select(s => new
                {
                    s.Id,
                    Display = s.Name + " (" + s.Code + ")"
                })
                .ToList();

            var teacherOptions = _context.Teachers
                .Include(t => t.User)
                .OrderBy(t => t.User.FullName)
                .Select(t => new
                {
                    t.Id,
                    Display = t.User.FullName + (string.IsNullOrWhiteSpace(t.Specialization) ? string.Empty : " - " + t.Specialization)
                })
                .ToList();

            // Get available session slots
            var sessionSlots = _scheduleService.GetAvailableSlots();
            var slotOptions = sessionSlots.Select(slot => new
            {
                Value = $"{slot.StartTime:HH\\:mm}-{slot.EndTime:HH\\:mm}",
                Display = slot.DisplayName
            }).ToList();

            ViewBag.RoomOptions = new SelectList(roomOptions, "Id", "Display", roomId);
            ViewBag.SubjectOptions = new SelectList(subjectOptions, "Id", "Display", subjectId);
            ViewBag.TeacherOptions = new SelectList(teacherOptions, "Id", "Display", teacherId);
            ViewBag.SessionSlots = new SelectList(slotOptions, "Value", "Display");
        }

        private async Task<Guid?> GetCurrentTeacherIdAsync()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return null;
            }

            return await _context.Teachers
                .Where(t => t.UserId == userId && t.IsActive)
                .Select(t => (Guid?)t.Id)
                .FirstOrDefaultAsync();
        }
    }
}
