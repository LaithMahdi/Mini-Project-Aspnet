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
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public StudentsController(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        // GET: Students
        public async Task<IActionResult> Index(string? search, Guid? classId, Gender? gender, DateTime? birthDate, int page = 1)
        {
            const int pageSize = 15;
            var isTeacher = User.IsInRole(Role.Teacher.ToString());
            var currentTeacherId = await GetCurrentTeacherIdAsync();

            IQueryable<Student> applicationDbContext = _context.Students
                .Include(s => s.User)
                .Include(s => s.Class);

            if (isTeacher)
            {
                if (!currentTeacherId.HasValue)
                {
                    return Forbid();
                }

                applicationDbContext = applicationDbContext
                    .Where(s => s.ClassId != null && (
                        s.Class!.ReferentTeacherId == currentTeacherId.Value ||
                        s.Class.ClassSubjects.Any(cs => cs.TeacherId == currentTeacherId.Value)));
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                applicationDbContext = applicationDbContext.Where(s =>
                    (s.User.FullName != null && s.User.FullName.ToLower().Contains(term)) ||
                    (s.User.UserName != null && s.User.UserName.ToLower().Contains(term)) ||
                    (s.CinNumber != null && s.CinNumber.ToLower().Contains(term)) ||
                    (s.PhoneNumber != null && s.PhoneNumber.ToLower().Contains(term)) ||
                    (s.Class != null && s.Class.Name.ToLower().Contains(term)));
            }

            if (classId.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(s => s.ClassId == classId.Value);
            }

            if (gender.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(s => s.Gender == gender.Value);
            }

            if (birthDate.HasValue)
            {
                var targetDate = birthDate.Value.Date;
                applicationDbContext = applicationDbContext.Where(s => s.DateOfBirth.HasValue && s.DateOfBirth.Value.Date == targetDate);
            }

            var filteredTotal = await applicationDbContext.CountAsync();
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

            var students = await applicationDbContext
                .OrderBy(s => s.User.FullName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalActive = await applicationDbContext.CountAsync(s => s.IsActive);
            var withClassCount = await applicationDbContext.CountAsync(s => s.ClassId != null);

            IQueryable<Classe> classesOptionsQuery = _context.Classes.OrderBy(c => c.Name);
            if (isTeacher && currentTeacherId.HasValue)
            {
                classesOptionsQuery = classesOptionsQuery.Where(c =>
                    c.ReferentTeacherId == currentTeacherId.Value ||
                    c.ClassSubjects.Any(cs => cs.TeacherId == currentTeacherId.Value));
            }

            var classOptions = await classesOptionsQuery
                .Select(c => new { c.Id, c.Name })
                .ToListAsync();

            ViewBag.IsTeacher = isTeacher;
            ViewBag.Search = search;
            ViewBag.ClassId = classId;
            ViewBag.Gender = gender;
            ViewBag.BirthDate = birthDate;
            ViewBag.ClassOptions = new SelectList(classOptions, "Id", "Name", classId);
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.FilteredTotal = filteredTotal;
            ViewBag.ActiveCount = totalActive;
            ViewBag.WithClassCount = withClassCount;

            return View(students);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.User)
                .Include(s => s.Class)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            if (User.IsInRole(Role.Teacher.ToString()))
            {
                var currentTeacherId = await GetCurrentTeacherIdAsync();
                if (!currentTeacherId.HasValue || student.ClassId == null)
                {
                    return Forbid();
                }

                var authorized = await _context.Classes
                    .AnyAsync(c => c.Id == student.ClassId &&
                                   (c.ReferentTeacherId == currentTeacherId.Value ||
                                    c.ClassSubjects.Any(cs => cs.TeacherId == currentTeacherId.Value)));

                if (!authorized)
                {
                    return Forbid();
                }
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            if (User.IsInRole(Role.Teacher.ToString()))
            {
                return Forbid();
            }

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["ClassId"] = new SelectList(_context.Classes.OrderBy(c => c.Name), "Id", "Name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateOfBirth,Gender,CinNumber,PhoneNumber,SecondPhoneNumber,Address,IsActive,EnrollmentDate,UserId,ClassId")] Student student)
        {
            if (User.IsInRole(Role.Teacher.ToString()))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                student.Id = Guid.NewGuid();
                _context.Add(student);
                await _context.SaveChangesAsync();

                // Notification: Student added to class
                if (student.ClassId != null)
                {
                    var classe = await _context.Classes.FindAsync(student.ClassId);
                    if (classe != null && classe.ReferentTeacherId.HasValue)
                    {
                        var studentUser = await _context.Users.FindAsync(student.UserId);
                        var teacher = await _context.Teachers.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == classe.ReferentTeacherId.Value);
                        if (teacher != null)
                        {
                            await _notificationService.SendNotificationAsync(
                                teacher.UserId,
                                "New Student in Class",
                                $"Student {studentUser?.FullName} has been added to your class {classe.Name}.",
                                $"/Students/Details/{student.Id}"
                            );
                        }
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", student.UserId);
            ViewData["ClassId"] = new SelectList(_context.Classes.OrderBy(c => c.Name), "Id", "Name", student.ClassId);
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (User.IsInRole(Role.Teacher.ToString()))
            {
                return Forbid();
            }

            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", student.UserId);
            ViewData["ClassId"] = new SelectList(_context.Classes.OrderBy(c => c.Name), "Id", "Name", student.ClassId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,DateOfBirth,Gender,CinNumber,PhoneNumber,SecondPhoneNumber,Address,IsActive,EnrollmentDate,UserId,ClassId")] Student student)
        {
            if (User.IsInRole(Role.Teacher.ToString()))
            {
                return Forbid();
            }

            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldStudent = await _context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == student.Id);
                    var oldClassId = oldStudent?.ClassId;

                    _context.Update(student);
                    await _context.SaveChangesAsync();

                    // Notification: Class changed
                    if (student.ClassId != oldClassId)
                    {
                        var studentUser = await _context.Users.FindAsync(student.UserId);

                        // Notify old teacher if removed
                        if (oldClassId.HasValue)
                        {
                            var oldClasse = await _context.Classes.FindAsync(oldClassId.Value);
                            if (oldClasse != null && oldClasse.ReferentTeacherId.HasValue)
                            {
                                var oldTeacher = await _context.Teachers.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == oldClasse.ReferentTeacherId.Value);
                                if (oldTeacher != null)
                                {
                                    await _notificationService.SendNotificationAsync(
                                        oldTeacher.UserId,
                                        "Student Removed from Class",
                                        $"Student {studentUser?.FullName} has been removed from your class {oldClasse.Name}."
                                    );
                                }
                            }
                        }

                        // Notify new teacher if added
                        if (student.ClassId.HasValue)
                        {
                            var newClasse = await _context.Classes.FindAsync(student.ClassId.Value);
                            if (newClasse != null && newClasse.ReferentTeacherId.HasValue)
                            {
                                var newTeacher = await _context.Teachers.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == newClasse.ReferentTeacherId.Value);
                                if (newTeacher != null)
                                {
                                    await _notificationService.SendNotificationAsync(
                                        newTeacher.UserId,
                                        "New Student in Class",
                                        $"Student {studentUser?.FullName} has been added to your class {newClasse.Name}.",
                                        $"/Students/Details/{student.Id}"
                                    );
                                }
                            }
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", student.UserId);
            ViewData["ClassId"] = new SelectList(_context.Classes.OrderBy(c => c.Name), "Id", "Name", student.ClassId);
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (User.IsInRole(Role.Teacher.ToString()))
            {
                return Forbid();
            }

            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.User)
                .Include(s => s.Class)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (User.IsInRole(Role.Teacher.ToString()))
            {
                return Forbid();
            }

            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(Guid id)
        {
            return _context.Students.Any(e => e.Id == id);
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
