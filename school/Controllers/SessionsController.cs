using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using school.Models;

namespace school.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SessionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sessions
        public async Task<IActionResult> Index(
            string? search,
            DateOnly? sessionDate,
            SessionStatus? status,
            Guid? teacherId,
            int? subjectId,
            string? startTime,
            string? endTime,
            int page = 1)
        {
            const int pageSize = 15;

            var sessionsQuery = _context.Sessions
                .Include(s => s.Room)
                .Include(s => s.Subject)
                .Include(s => s.Teacher)
                    .ThenInclude(t => t.User)
                .AsQueryable();

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

            if (!string.IsNullOrWhiteSpace(startTime) && TimeOnly.TryParse(startTime, out var parsedStartTime))
            {
                sessionsQuery = sessionsQuery.Where(s => s.StartTime >= parsedStartTime);
            }

            if (!string.IsNullOrWhiteSpace(endTime) && TimeOnly.TryParse(endTime, out var parsedEndTime))
            {
                sessionsQuery = sessionsQuery.Where(s => s.EndTime <= parsedEndTime);
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
            ViewBag.StartTime = startTime;
            ViewBag.EndTime = endTime;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.FilteredTotal = filteredTotal;

            PopulateFilterOptions(status, teacherId, subjectId);

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

            return View(session);
        }

        // GET: Sessions/Create
        public IActionResult Create()
        {
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
                Console.WriteLine($"Creating session: Date={session.SessionDate}, StartTime={session.StartTime}, EndTime={session.EndTime}, Status={session.Status}, IsActive={session.IsActive}, RoomId={session.RoomId}, SubjectId={session.SubjectId}, TeacherId={session.TeacherId}");
            if (ModelState.IsValid)
            {
                                // print the session details to the console for debugging
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
            if (id != session.Id)
            {
                return NotFound();
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
            var session = await _context.Sessions.FindAsync(id);
            if (session != null)
            {
                _context.Sessions.Remove(session);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SessionExists(int id)
        {
            return _context.Sessions.Any(e => e.Id == id);
        }

        private void PopulateFilterOptions(SessionStatus? selectedStatus, Guid? selectedTeacherId, int? selectedSubjectId)
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

            ViewBag.RoomOptions = new SelectList(roomOptions, "Id", "Display", roomId);
            ViewBag.SubjectOptions = new SelectList(subjectOptions, "Id", "Display", subjectId);
            ViewBag.TeacherOptions = new SelectList(teacherOptions, "Id", "Display", teacherId);
        }
    }
}
